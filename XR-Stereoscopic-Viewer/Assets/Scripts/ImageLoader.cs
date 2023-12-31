using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Android;



public class ImageLoader : MonoBehaviour
{
    public Texture2D videoPlaceholder; //视频缩略图临时占位图
    public RectTransform container; // 你的UI容器，例如一个ScrollRect的Content
    public GameObject imagePrefab;    // 一个RawImage预制体，用于显示图片
    public bool ReadyToBuild = true;
    [Space]
    public Texture2D img_guidance;
    public Texture2D img_default;

    private string folderPath;
    private VideoPlayer videoPlayer; // 视频播放器
    private RenderTexture videoTexture;  // 用于存储视频帧的RenderTexture

    private Queue<string> videoPathsQueue = new Queue<string>();
    private bool isProcessing = false;


    private void Start()
    {
        videoTexture = new RenderTexture(1920, 1080, 0);
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.targetTexture = videoTexture;

        if (ReadyToBuild) //安卓打包
        {
            folderPath = "/sdcard/Pictures/3dMedia";

            RequestPermissions(); //请求写入权限
        }
        else //编辑器调试
        {
            folderPath = Application.dataPath + "/Media/"; 
            LoadAllImages();
        }
    }

    private void LoadAllImages()
    {
        string[] imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                   .OrderBy(filePath => File.GetLastWriteTime(filePath))
                                   .ToArray();

        StartCoroutine(LoadImagesInBatches(imageFiles, 2)); // 以2个文件为一批进行加载

    }
    IEnumerator LoadImagesInBatches(string[] imageFiles, int batchSize)
    {
        int currentBatch = 0;

        while (currentBatch * batchSize < imageFiles.Length)
        {
            int batchEnd = Mathf.Min((currentBatch + 1) * batchSize, imageFiles.Length);
            for (int i = currentBatch * batchSize; i < batchEnd; i++)
            {
                string imagePath = imageFiles[i];
                string extension = Path.GetExtension(imagePath).ToLower();

                if (extension == ".png" || extension == ".jpg")
                {
                    yield return StartCoroutine(LoadImage(imagePath));
                }
                else if (extension == ".mp4")
                {
                    AddVideoToQueue(imagePath);
                    // 可以选择在此处添加视频加载逻辑
                }
            }
            currentBatch++;
            yield return null; // 等待一帧或根据需要更长时间
        }
    }

    //抓取图片
    private IEnumerator LoadImage(string imagePath)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + imagePath);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.ConnectionError && uwr.result != UnityWebRequest.Result.DataProcessingError)
        {
            Texture2D RawTexture = DownloadHandlerTexture.GetContent(uwr);
            Texture2D texture = TextureUtilities.ResizeTexture(RawTexture, 205f); //缩小图片尺寸
            Destroy(RawTexture);

            GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
            imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + imagePath;
            imageInstance.GetComponent<MediaAttributes>().IsVideo = false;


            imageInstance.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = " "; //照片不显示视频时长

            RawImage childRawImage = imageInstance.transform.Find("Img").GetComponent<RawImage>(); 
            childRawImage.uvRect = ResizeTexUVRect(childRawImage, texture);
        }
        else
        {
            Debug.LogError("Error loading image: " + uwr.error);
        }
        uwr.Dispose(); // 释放UnityWebRequest使用的资源
    }

    //抓取视频
    public void AddVideoToQueue(string videoPath)
    {
        videoPathsQueue.Enqueue(videoPath);

        if (!isProcessing)
        {
            StartCoroutine(ProcessVideos());
        }
    }
    private IEnumerator ProcessVideos() //视频加载流
    {
        isProcessing = true;

        while (videoPathsQueue.Count > 0)
        {
            string currentVideoPath = videoPathsQueue.Dequeue();
            yield return LoadThumbnailFromVideo(currentVideoPath);
        }

        isProcessing = false;
    }
    private IEnumerator LoadThumbnailFromVideo(string videoPath) //视频缩略图
    {
        //创建实例
        GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
        imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + videoPath;
        imageInstance.GetComponent<MediaAttributes>().IsVideo = true;

        RawImage childRawImage = imageInstance.transform.Find("Img").GetComponent<RawImage>();

        if (childRawImage != null)
        {
            childRawImage.texture = videoPlaceholder;
        }

        //加载视频
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null; // 等待视频准备完毕
        }

        double videoLength = videoPlayer.length;// 获取视频时长
        string minutes = Mathf.Floor((float)(videoLength / 60)).ToString("00");
        string seconds = Mathf.Floor((float)(videoLength % 60)).ToString("00");
        imageInstance.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = minutes + ":" + seconds; //显示视频时长

        RenderTexture tempRenderTexture = new RenderTexture((int)videoPlayer.width, (int)videoPlayer.height, 0);
        videoPlayer.targetTexture = tempRenderTexture; // 设置VideoPlayer的目标纹理为新创建的RenderTexture

        Texture2D videoFrame = new Texture2D((int)videoPlayer.width, (int)videoPlayer.height);
        videoPlayer.frame = 0; // 获取视频的第一帧
        videoPlayer.SetDirectAudioMute(0, true); //静音

        videoPlayer.Play();
        yield return new WaitForSeconds(0.5f); //等待0.1秒以确保第一帧被渲染
        videoPlayer.Stop();

        RenderTexture.active = tempRenderTexture;  // 设置tempRenderTexture为当前活跃的RenderTexture
        videoFrame.ReadPixels(new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height), 0, 0);

        videoFrame.Apply();

        //用实际的缩略图替代占位图
        childRawImage.texture = videoFrame;
        childRawImage.uvRect = ResizeTexUVRect(childRawImage, videoFrame);

        // 释放资源
        videoPlayer.targetTexture = null;
        RenderTexture.active = null;
        Destroy(tempRenderTexture);
    }

    private Rect ResizeTexUVRect(RawImage childRawImage, Texture2D texture) { //调整图片尺寸&位置
        if (childRawImage != null)
        {
            childRawImage.texture = texture;

            float aspectRatio = (float)texture.width / texture.height;
            Rect newUVRect = childRawImage.uvRect;

            if (aspectRatio > 2f) //横向图像
            {
                newUVRect.x = 0.25f - (newUVRect.width / 2.0f);
                newUVRect.y = 0f;
                newUVRect.width = (float)texture.height / texture.width;
                newUVRect.height = 1f;

            }else if (aspectRatio == 2f) //方形图像
            {
                newUVRect.x = 0f;
                newUVRect.y = 0f;
                newUVRect.width = 0.5f;
                newUVRect.height = 1f;
            }
            else //竖向图像
            {
                newUVRect.x = 0f;
                newUVRect.y = 0.33f;
                newUVRect.width = 0.5f;
                newUVRect.height = 0.66f;
            }

            return newUVRect;
        }
        return new Rect(0, 0, 1, 1);
    }

    void RequestPermissions() //检查写入外部存储的权限
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        else
        {
            OnPermissionGranted();
        }
    }

    void OnApplicationFocus(bool hasFocus) //重新获得焦点时
    {
        if (hasFocus && ReadyToBuild)
        {
            RequestPermissions(); //再检查权限
        }
    }

    void OnPermissionGranted() //已获得权限，开始写入
    {
        if (!Directory.Exists(folderPath)) //文件夹路径不存在
        {
            Directory.CreateDirectory(folderPath); //创建文件夹
            SaveTextureToDisk(img_guidance, "/sdcard/Pictures/3dMedia/img_guidance.png");
            SaveTextureToDisk(img_default, "/sdcard/Pictures/3dMedia/img_default.png");
            LoadAllImages();
        }
        else
        {
            string[] entries = Directory.GetFileSystemEntries(folderPath);
            if (entries.Length == 0) //文件夹为空
            {
                SaveTextureToDisk(img_guidance, "/sdcard/Pictures/3dMedia/img_guidance.png");
                SaveTextureToDisk(img_default, "/sdcard/Pictures/3dMedia/img_default.png");
                LoadAllImages();
            }
            else
            {
                LoadAllImages();
            }
        }

    }

    void SaveTextureToDisk(Texture2D texture, string filePath) //保存图片到本机
    {
        try
        {
            // 获取原始纹理的像素数据
            Color[] pixels = texture.GetPixels();

            // 创建新的 Texture2D 对象
            Texture2D readableTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

            // 将像素数据设置到新的 Texture2D 对象中
            readableTexture.SetPixels(pixels);
            readableTexture.Apply();


            Graphics.CopyTexture(texture, readableTexture);
            byte[] textureBytes = readableTexture.EncodeToPNG();
            if (textureBytes != null)
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllBytes(filePath, textureBytes);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception while saving texture: " + e.Message);
        }
    }
}
