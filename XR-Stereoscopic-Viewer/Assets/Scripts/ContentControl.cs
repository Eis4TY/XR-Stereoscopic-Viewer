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
    [Space]
    [Header("Photo DeepMap")]
    public Texture2D imgDeep;
    [Space]
    [Header("Video")]
    public string videoURL;
    //private float videosize_W, videosize_H;
    [Space]
    [Space]
    [Space]
    public GameObject planeObj_L;
    public GameObject planeObj_R;

    public Material Plane_L;
    public Material Plane_R;
    [Space]
    public GameObject InnerWallObj_L;
    public GameObject InnerWallObj_R;

    public Material InnerWall_L;
    public Material InnerWall_R;
    [Space]
    public Transform blurMask;
    public RectTransform ViewWindow;
    [Space]
    public bool isVideo = false;
    [Space]
    public bool isDepth = false;

    [Header("DeepControl")]
    [Range(0.0f, 2.5f)]
    private float deepValue = 0;
    public GameObject deepSlider;

    private List<GameObject> mediaObjs = new List<GameObject>();
    private int currentIndex = -1;

    void OnValidate()
    {
        ChangeContent();
    }

    private void ChangeContent()
    {
        if (isVideo)  //VIDEO
        {
            deepSlider.SetActive(false);
            Plane_L.SetFloat("_Bulge", 0);
            Plane_R.SetFloat("_Bulge", 0);

            PlayVideo(planeObj_L, videoURL);
            PlayVideo(planeObj_R, videoURL);
            PlayVideo(InnerWallObj_L, videoURL);
            PlayVideo(InnerWallObj_R, videoURL);
        }
        else  //PHOTO
        {
            StopVideo(planeObj_L);
            StopVideo(planeObj_R);
            StopVideo(InnerWallObj_L);
            StopVideo(InnerWallObj_R);

            Plane_L.SetTextureScale("_Texture2D", new Vector2(0.5f, 1.0f));
            Resize(img.width, img.height);

            Plane_L.SetTexture("_Texture2D", img);
            Plane_R.SetTexture("_Texture2D", img);

            if (isDepth & imgDeep != null)
            {
                deepSlider.SetActive(true);
                Plane_L.SetTexture("_Depth", imgDeep);
                Plane_R.SetTexture("_Depth", imgDeep);

                Plane_L.SetFloat("_Bulge", deepValue);
                Plane_R.SetFloat("_Bulge", deepValue);
            }
            else
            {
                deepSlider.SetActive(false);
                Plane_L.SetFloat("_Bulge", 0);
                Plane_R.SetFloat("_Bulge", 0);
            }

            InnerWall_L.SetTexture("_Texture2D", img);
            InnerWall_R.SetTexture("_Texture2D", img);
        }
    }


    private int preparedPlayersCount = 0; //同时播放视频
    private List<VideoPlayer> playersToPlay = new List<VideoPlayer>();

    private void PlayVideo(GameObject playObj, string url)
    {
        VideoPlayer player = playObj.GetComponent<VideoPlayer>();
        player.enabled = true;
        Renderer texRenderer = playObj.GetComponent<Renderer>();
        player.source = VideoSource.Url;
        player.url = url;
        
        player.prepareCompleted += PlayerPrepareCompleted; // 添加事件监听
        player.Prepare(); // 开始准备视频

        player.renderMode = VideoRenderMode.MaterialOverride;
        player.targetMaterialRenderer = texRenderer;
        player.targetMaterialProperty = "_Texture2D";
    }
    
    private void PlayerPrepareCompleted(VideoPlayer player)
    {
        Resize(player.width, player.height);

        preparedPlayersCount++;
        playersToPlay.Add(player);

        if (preparedPlayersCount == 4) // 当四个视频都已经准备好
        {
            foreach (var p in playersToPlay)
            {
                p.Play();
            }

            // 重置计数器和列表，以备后续的播放请求
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
        // 比较当前帧与上一帧的变化
        if (img != lastImg || deepValue != lastDeepValue || videoURL != lastVideoURL)
        {
            ChangeContent();

            lastImg = img;
            lastDeepValue = deepValue;
            lastVideoURL = videoURL;
        }
    }
    
    private Texture lastImg;
    private string lastVideoURL;
    private float lastDeepValue;

    public float DeepValue { get => deepValue; set => deepValue = value; }
    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public List<GameObject> MediaObjs { get => mediaObjs; set => mediaObjs = value; }

    private void Resize(float img_width, float img_height) //设置图像显示的宽高比
    {
        Vector3 scaleValue;

        if (img_width / 2 > img_height)
        {
            scaleValue = new Vector3((img_width / 2) / img_height, 1, 1);
            planeObj_L.transform.localScale = scaleValue;
            planeObj_R.transform.localScale = scaleValue;
            blurMask.localScale = scaleValue;
            ViewWindow.sizeDelta = new Vector2(((img_width / 2) / img_height) * 0.8f, 0.8f);
        }
        else
        {
            scaleValue = new Vector3(1, img_height / (img_width / 2), 1);
            planeObj_L.transform.localScale = scaleValue;
            planeObj_R.transform.localScale = scaleValue;
            blurMask.localScale = scaleValue;
            ViewWindow.sizeDelta = new Vector2(0.8f, (img_height / (img_width / 2)) * 0.8f);
        }
    }

    public void Set_img(Texture2D image)
    {
        img = image;
    }
    public void Set_imgDeep(Texture2D image)
    {
        imgDeep = image;
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