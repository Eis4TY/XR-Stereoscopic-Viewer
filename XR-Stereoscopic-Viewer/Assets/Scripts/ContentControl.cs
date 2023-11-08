using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using System.Drawing;
using UnityEngine.UI;



public class ContentControl : MonoBehaviour
{
    [Header("Photo")]
    public Texture2D img;
    private Texture2D thumbnail;
    [Space]
    [Header("Video")]
    public bool isVideo = false;
    public string videoURL;
    //private float videosize_W, videosize_H;
    [Space]
    [Space]
    [Space]
    public GameObject planeObj_L;
    public Material Plane_L;
    [Space]
    public GameObject planeObj_R;
    public Material Plane_R;
    [Space]
    [Space]
    [Space]
    public GameObject blurWall_L;
    public Material blur_L;
    [Space]
    public GameObject blurWall_R;
    public Material blur_R;
    [Space]
    [Space]
    [Space]
    public RectTransform ViewWindow;
    public ScrollRect scrollRect;
    public GameObject videoController;
    public RectTransform progressSlider;



    private List<GameObject> mediaObjs = new List<GameObject>();
    private int currentIndex = -1;

    void OnValidate()
    {
        ChangeContent();
    }

    private void ChangeContent()
    {
        if (mediaObjs.Count > 0) {
            scrollRect.horizontalNormalizedPosition = (float)(currentIndex) / mediaObjs.Count;
        }

        if (isVideo)  //VIDEO
        {
            PlayVideo(planeObj_L, videoURL);
            PlayVideo(planeObj_R, videoURL);

            blurWall_L.SetActive(false);
            blurWall_R.SetActive(false);
            videoController.SetActive(true);
            
        }
        else  //PHOTO
        {
            StopVideo(planeObj_L);
            StopVideo(planeObj_R);
            videoController.SetActive(false);

            ResizeWindow(img.width, img.height);

            Plane_L.SetTexture("_MainTex", img);
            Plane_R.SetTexture("_MainTex", img);

            blurWall_L.SetActive(true);
            blurWall_R.SetActive(true);

            blur_L.SetTexture("_MainTex", thumbnail);
            blur_R.SetTexture("_MainTex", thumbnail);
        }
    }


    private int preparedPlayersCount = 0; //ͬʱ������Ƶ
    private List<VideoPlayer> playersToPlay = new List<VideoPlayer>();

    private void PlayVideo(GameObject playObj, string url)
    {
        VideoPlayer player = playObj.GetComponent<VideoPlayer>();
        player.enabled = true;
        Renderer texRenderer = playObj.GetComponent<Renderer>();
        player.source = VideoSource.Url;
        player.url = url;
        
        player.prepareCompleted += PlayerPrepareCompleted; // ����¼�����
        player.Prepare(); // ��ʼ׼����Ƶ

        player.renderMode = VideoRenderMode.MaterialOverride;
        player.targetMaterialRenderer = texRenderer;
        player.targetMaterialProperty = "_MainTex";
    }
    
    private void PlayerPrepareCompleted(VideoPlayer player)
    {
        ResizeWindow(player.width, player.height);
        progressSlider.sizeDelta = new Vector2(ViewWindow.sizeDelta.x * 1000 - 200, 16);

        preparedPlayersCount++;
        playersToPlay.Add(player);

        if (preparedPlayersCount == 2) // ��2����Ƶ���Ѿ�׼����
        {
            foreach (var p in playersToPlay)
            {
                p.Play();
            }

            // ���ü��������б��Ա������Ĳ�������
            preparedPlayersCount = 0;
            playersToPlay.Clear();
        }
    }

    private void StopVideo(GameObject playObj)
    {
        VideoPlayer player = playObj.GetComponent<VideoPlayer>();
        player.Stop();
        player.enabled = false;
    }
    
    void Update()
    {
        if (img != lastImg || videoURL != lastVideoURL) // �Ƚϵ�ǰ֡����һ֡�ı仯
        {
            ChangeContent();

            lastImg = img;
            lastVideoURL = videoURL;
        }
    }

    public void forceUpdateContent()
    {
        ChangeContent();
    }

    private Texture lastImg;
    private string lastVideoURL;

    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public List<GameObject> MediaObjs { get => mediaObjs; set => mediaObjs = value; }

    private void ResizeWindow(float img_width, float img_height) //������ʾ���ڵĿ�߱�
    {
        Vector3 scaleValue;

        scaleValue = new Vector3((img_width / 2) / img_height, 1, 1);
        planeObj_L.transform.localScale = scaleValue;
        planeObj_R.transform.localScale = scaleValue;
        ViewWindow.sizeDelta = new Vector2(((img_width / 2) / img_height) * 0.8f, 0.8f);
    }

    public void Set_img(Texture2D image)
    {
        img = image;
    }
    public void Set_thumbnail(Texture2D image)
    {
        thumbnail = image;
    }
    
    public void Set_isVideo(bool IsVideo)
    {
        isVideo = IsVideo;
    }
    public void Set_videoURL(string url)
    {
        videoURL = url;
    }
}