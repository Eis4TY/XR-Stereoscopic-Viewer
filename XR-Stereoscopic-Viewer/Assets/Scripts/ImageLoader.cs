using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;
using Meta.Voice.Hub;
using System.Collections.Generic;

public class ImageLoader : MonoBehaviour
{
    public RectTransform container; // ���UI����������һ��ScrollRect��Content
    public GameObject imagePrefab;    // һ��RawImageԤ���壬������ʾͼƬ
    public bool ReadyToBuild = true;

    public Sprite sprite_video, sprite_photo;

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


        if (ReadyToBuild)
        {
            folderPath = Application.persistentDataPath + "/Media/"; //��׿���
        }
        else
        {
            folderPath = Application.dataPath + "/Media/"; //�༭������
        }
        LoadAllImages();
    }
    /*
    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
        {
            LoadAllImages();
        }
    }
    */
    private void LoadAllImages()
    {
        string[] imageFiles = Directory.GetFiles(folderPath);

        foreach (string imagePath in imageFiles)
        {
            string extension = Path.GetExtension(imagePath).ToLower();
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);

            if (extension == ".png" || extension == ".jpg")
            {
                // �ж��Ƿ������ͼ
                if (!filenameWithoutExtension.EndsWith("_D"))
                {
                    StartCoroutine(LoadImage(imagePath, imageFiles)); 
                }
            }
            else if (extension == ".mp4")
            {
                //StartCoroutine(LoadThumbnailFromVideo(imagePath));
                AddVideoToQueue(imagePath);
            }
        }
    }

    //ץȡͼƬ
    private IEnumerator LoadImage(string imagePath,  string[] allImages)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://" + imagePath);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.ConnectionError && uwr.result != UnityWebRequest.Result.DataProcessingError)
        {
            Texture2D RawTexture = DownloadHandlerTexture.GetContent(uwr);
            Texture2D texture = TextureUtilities.ResizeTexture(RawTexture, 254f);
            Destroy(RawTexture);

            GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
            imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + imagePath;
            imageInstance.GetComponent<MediaAttributes>().IsVideo = false;

            // Ѱ�Ҷ�Ӧ�����ͼ
            string depthImagePath = null;
            string imageNameWithDepth = Path.GetFileNameWithoutExtension(imagePath) + "_D" + Path.GetExtension(imagePath);
            if (System.Array.Exists(allImages, item => item.EndsWith(imageNameWithDepth)))
            {
                imageInstance.GetComponent<MediaAttributes>().HasDepth = true;
                depthImagePath = Path.Combine(folderPath, imageNameWithDepth);
                imageInstance.GetComponent<MediaAttributes>().DepthPath = "file://" + depthImagePath;
            }
            else
            {
                imageInstance.GetComponent<MediaAttributes>().HasDepth = false;
            }

            imageInstance.transform.Find("Icon").GetComponent<Image>().sprite = sprite_photo; //�滻����ͼIcon

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
    private IEnumerator ProcessVideos()
    {
        isProcessing = true;

        while (videoPathsQueue.Count > 0)
        {
            string currentVideoPath = videoPathsQueue.Dequeue();
            yield return LoadThumbnailFromVideo(currentVideoPath);
        }

        isProcessing = false;
    }
    private IEnumerator LoadThumbnailFromVideo(string videoPath)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null; // �ȴ���Ƶ׼�����
        }

        RenderTexture tempRenderTexture = new RenderTexture((int)videoPlayer.width, (int)videoPlayer.height, 0);
        videoPlayer.targetTexture = tempRenderTexture; // ����VideoPlayer��Ŀ������Ϊ�´�����RenderTexture

        Texture2D videoFrame = new Texture2D((int)videoPlayer.width, (int)videoPlayer.height);
        videoPlayer.frame = 0; // ��ȡ��Ƶ�ĵ�һ֡
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null; // �ȴ�һ֡ʱ��
        }

        videoPlayer.Stop();

        RenderTexture.active = tempRenderTexture;  // ����tempRenderTextureΪ��ǰ��Ծ��RenderTexture
        videoFrame.ReadPixels(new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height), 0, 0);

        videoFrame.Apply();

        GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
        imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + videoPath;
        imageInstance.GetComponent<MediaAttributes>().IsVideo = true;

        RawImage childRawImage = imageInstance.transform.Find("Img").GetComponent<RawImage>();

        imageInstance.transform.Find("Icon").GetComponent<Image>().sprite = sprite_video; //�滻����ͼIcon

        if (childRawImage != null)
        {
            childRawImage.texture = videoFrame;
            childRawImage.uvRect = ResizeTexUVRect(childRawImage, videoFrame);
        }

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

            newUVRect.width = (float)texture.height / texture.width;
            if (aspectRatio > 2f)
            {
                newUVRect.x = 0.25f - (newUVRect.width / 2.0f);
            }
            else
            {
                newUVRect.x = (newUVRect.width / 2.0f) - 0.25f;
            }

            return newUVRect;
        }
        return new Rect(0, 0, 1, 1);
    }
}