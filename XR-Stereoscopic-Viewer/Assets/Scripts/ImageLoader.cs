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
    public Texture2D videoPlaceholder; //��Ƶ����ͼ��ʱռλͼ
    public RectTransform container; // ���UI����������һ��ScrollRect��Content
    public GameObject imagePrefab;    // һ��RawImageԤ���壬������ʾͼƬ
    public bool ReadyToBuild = true;
    [Space]
    public Texture2D img_guidance;
    public Texture2D img_default;

    private string folderPath;
    private VideoPlayer videoPlayer; // ��Ƶ������
    private RenderTexture videoTexture;  // ���ڴ洢��Ƶ֡��RenderTexture

    private Queue<string> videoPathsQueue = new Queue<string>();
    private bool isProcessing = false;


    private void Start()
    {
        videoTexture = new RenderTexture(1920, 1080, 0);
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.targetTexture = videoTexture;

        if (ReadyToBuild) //��׿���
        {
            folderPath = "/sdcard/Pictures/3dMedia";

            RequestPermissions(); //����д��Ȩ��
        }
        else //�༭������
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

        StartCoroutine(LoadImagesInBatches(imageFiles, 2)); // ��2���ļ�Ϊһ�����м���

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
                    // ����ѡ���ڴ˴������Ƶ�����߼�
                }
            }
            currentBatch++;
            yield return null; // �ȴ�һ֡�������Ҫ����ʱ��
        }
    }

    //ץȡͼƬ
    private IEnumerator LoadImage(string imagePath)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + imagePath);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.ConnectionError && uwr.result != UnityWebRequest.Result.DataProcessingError)
        {
            Texture2D RawTexture = DownloadHandlerTexture.GetContent(uwr);
            Texture2D texture = TextureUtilities.ResizeTexture(RawTexture, 205f); //��СͼƬ�ߴ�
            Destroy(RawTexture);

            GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
            imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + imagePath;
            imageInstance.GetComponent<MediaAttributes>().IsVideo = false;


            imageInstance.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = " "; //��Ƭ����ʾ��Ƶʱ��

            RawImage childRawImage = imageInstance.transform.Find("Img").GetComponent<RawImage>(); 
            childRawImage.uvRect = ResizeTexUVRect(childRawImage, texture);
        }
        else
        {
            Debug.LogError("Error loading image: " + uwr.error);
        }
        uwr.Dispose(); // �ͷ�UnityWebRequestʹ�õ���Դ
    }

    //ץȡ��Ƶ
    public void AddVideoToQueue(string videoPath)
    {
        videoPathsQueue.Enqueue(videoPath);

        if (!isProcessing)
        {
            StartCoroutine(ProcessVideos());
        }
    }
    private IEnumerator ProcessVideos() //��Ƶ������
    {
        isProcessing = true;

        while (videoPathsQueue.Count > 0)
        {
            string currentVideoPath = videoPathsQueue.Dequeue();
            yield return LoadThumbnailFromVideo(currentVideoPath);
        }

        isProcessing = false;
    }
    private IEnumerator LoadThumbnailFromVideo(string videoPath) //��Ƶ����ͼ
    {
        //����ʵ��
        GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
        imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + videoPath;
        imageInstance.GetComponent<MediaAttributes>().IsVideo = true;

        RawImage childRawImage = imageInstance.transform.Find("Img").GetComponent<RawImage>();

        if (childRawImage != null)
        {
            childRawImage.texture = videoPlaceholder;
        }

        //������Ƶ
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null; // �ȴ���Ƶ׼�����
        }

        double videoLength = videoPlayer.length;// ��ȡ��Ƶʱ��
        string minutes = Mathf.Floor((float)(videoLength / 60)).ToString("00");
        string seconds = Mathf.Floor((float)(videoLength % 60)).ToString("00");
        imageInstance.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = minutes + ":" + seconds; //��ʾ��Ƶʱ��

        RenderTexture tempRenderTexture = new RenderTexture((int)videoPlayer.width, (int)videoPlayer.height, 0);
        videoPlayer.targetTexture = tempRenderTexture; // ����VideoPlayer��Ŀ������Ϊ�´�����RenderTexture

        Texture2D videoFrame = new Texture2D((int)videoPlayer.width, (int)videoPlayer.height);
        videoPlayer.frame = 0; // ��ȡ��Ƶ�ĵ�һ֡
        videoPlayer.SetDirectAudioMute(0, true); //����

        videoPlayer.Play();
        yield return new WaitForSeconds(0.5f); //�ȴ�0.1����ȷ����һ֡����Ⱦ
        videoPlayer.Stop();

        RenderTexture.active = tempRenderTexture;  // ����tempRenderTextureΪ��ǰ��Ծ��RenderTexture
        videoFrame.ReadPixels(new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height), 0, 0);

        videoFrame.Apply();

        //��ʵ�ʵ�����ͼ���ռλͼ
        childRawImage.texture = videoFrame;
        childRawImage.uvRect = ResizeTexUVRect(childRawImage, videoFrame);

        // �ͷ���Դ
        videoPlayer.targetTexture = null;
        RenderTexture.active = null;
        Destroy(tempRenderTexture);
    }

    private Rect ResizeTexUVRect(RawImage childRawImage, Texture2D texture) { //����ͼƬ�ߴ�&λ��
        if (childRawImage != null)
        {
            childRawImage.texture = texture;

            float aspectRatio = (float)texture.width / texture.height;
            Rect newUVRect = childRawImage.uvRect;

            if (aspectRatio > 2f) //����ͼ��
            {
                newUVRect.x = 0.25f - (newUVRect.width / 2.0f);
                newUVRect.y = 0f;
                newUVRect.width = (float)texture.height / texture.width;
                newUVRect.height = 1f;

            }else if (aspectRatio == 2f) //����ͼ��
            {
                newUVRect.x = 0f;
                newUVRect.y = 0f;
                newUVRect.width = 0.5f;
                newUVRect.height = 1f;
            }
            else //����ͼ��
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

    void RequestPermissions() //���д���ⲿ�洢��Ȩ��
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

    void OnApplicationFocus(bool hasFocus) //���»�ý���ʱ
    {
        if (hasFocus && ReadyToBuild)
        {
            RequestPermissions(); //�ټ��Ȩ��
        }
    }

    void OnPermissionGranted() //�ѻ��Ȩ�ޣ���ʼд��
    {
        if (!Directory.Exists(folderPath)) //�ļ���·��������
        {
            Directory.CreateDirectory(folderPath); //�����ļ���
            SaveTextureToDisk(img_guidance, "/sdcard/Pictures/3dMedia/img_guidance.png");
            SaveTextureToDisk(img_default, "/sdcard/Pictures/3dMedia/img_default.png");
            LoadAllImages();
        }
        else
        {
            string[] entries = Directory.GetFileSystemEntries(folderPath);
            if (entries.Length == 0) //�ļ���Ϊ��
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

    void SaveTextureToDisk(Texture2D texture, string filePath) //����ͼƬ������
    {
        try
        {
            // ��ȡԭʼ�������������
            Color[] pixels = texture.GetPixels();

            // �����µ� Texture2D ����
            Texture2D readableTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

            // �������������õ��µ� Texture2D ������
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
