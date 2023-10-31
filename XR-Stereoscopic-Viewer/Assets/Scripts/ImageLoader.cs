using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;
using Meta.Voice.Hub;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ImageLoader : MonoBehaviour
{
    public Texture2D videoPlaceholder;
    public RectTransform container; // ���UI����������һ��ScrollRect��Content
    public GameObject imagePrefab;    // һ��RawImageԤ���壬������ʾͼƬ
    public bool ReadyToBuild = true;

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
            currentBatch++;
            yield return null; // �ȴ�һ֡�������Ҫ����ʱ��
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
            Texture2D texture = TextureUtilities.ResizeTexture(RawTexture, 256f);
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

            imageInstance.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = " "; //��ʾ��Ƶʱ��

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
        //����ʵ��
        GameObject imageInstance = Instantiate<GameObject>(imagePrefab, container);
        imageInstance.GetComponent<MediaAttributes>().ImagePath = "file://" + videoPath;
        imageInstance.GetComponent<MediaAttributes>().IsVideo = true;


        RawImage childRawImage = imageInstance.transform.Find("Img").GetComponent<RawImage>();

        if (childRawImage != null)
        {
            childRawImage.texture = videoPlaceholder;
            //childRawImage.uvRect = ResizeTexUVRect(childRawImage, videoPlaceholder);
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
}
