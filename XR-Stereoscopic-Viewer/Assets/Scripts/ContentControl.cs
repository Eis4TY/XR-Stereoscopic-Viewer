using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using System.Drawing;


public class ContentControl : MonoBehaviour
{
    [Header("Photo")]
    public Texture img_L;
    public Texture img_R;
    [Space]
    [Header("Photo DeepMap")]
    public Texture2D imgDeep_L;
    public Texture2D imgDeep_R;
    [Space]
    [Header("Video")]
    public VideoClip clip_L;
    public VideoClip clip_R;
    [Space]
    [Space]
    [Space]
    public GameObject plane_L;
    public GameObject plane_R;
    private Material Plane_L;
    private Material Plane_R;
    [Space]
    public Material InnerWall_L;
    public Material InnerWall_R;
    [Space]
    public Transform blurMask;
    public RectTransform ViewWindow;
    [Space]
    public bool isVideo = false;
    public bool isDepth = false;
    [Range(0, 5)]
    public float deepValue = 0;

    
    void OnValidate()
    {
        ChangeContent();
    }

    private void ChangeContent()
    {
        Resize();

        //PHOTO
        Plane_L = plane_L.GetComponent<MeshRenderer>().sharedMaterial;
        Plane_R = plane_R.GetComponent<MeshRenderer>().sharedMaterial;

        Plane_L.SetTexture("_MainTex", img_L);
        Plane_R.SetTexture("_MainTex", img_R);
        
        if (isDepth & imgDeep_L != null)
        {
            Plane_L.SetTexture("_depth", imgDeep_L);
            Plane_R.SetTexture("_depth", imgDeep_R);

            Plane_L.SetFloat("_scale", deepValue);
            Plane_R.SetFloat("_scale", deepValue);
        }
        else
        {
            Plane_L.SetFloat("_scale", 0);
            Plane_R.SetFloat("_scale", 0);
        }
        
        InnerWall_L.mainTexture = img_L;
        InnerWall_R.mainTexture = img_R;

        //VIDEO
        VideoPlayer video_L = plane_L.transform.GetComponent<VideoPlayer>();
        VideoPlayer video_R = plane_R.transform.GetComponent<VideoPlayer>();
        if (img_L is RenderTexture & img_R is RenderTexture)
        {
            Debug.Log("是rendertexture！！！！");
            video_L.enabled = true;
            video_L.clip = clip_L;

            video_R.enabled = true;
            video_R.clip = clip_R;

            video_L.Play();
            video_R.Play();
        }
        else
        {
            video_L.enabled = false; 
            video_R.enabled = false;
        }
    }


    void Update()
    {
        // 比较当前帧与上一帧的变化
        if (img_L != lastImgL)
        {
            ChangeContent();

            lastImgL = img_L;
        }
    }
    private Texture lastImgL;

    private void Resize() //设置图像显示的宽高比
    {
        float img_width = img_L.width;
        float img_height = img_L.height;

        Vector3 scaleValue;

        if(img_width > img_height)
        {
            scaleValue = new Vector3(img_width / img_height, 1, 1);
            plane_L.transform.localScale = scaleValue;
            plane_R.transform.localScale = scaleValue;
            blurMask.localScale = scaleValue;
            ViewWindow.sizeDelta = new Vector2((img_width / img_height) * 0.7f, 0.7f);
        }
        else
        {
            scaleValue = new Vector3(1,img_height / img_width, 1);
            plane_L.transform.localScale = scaleValue;
            plane_R.transform.localScale = scaleValue;
            blurMask.localScale = scaleValue;
            ViewWindow.sizeDelta = new Vector2(0.7f, (img_height / img_width) * 0.7f);
        }
    }

    public void Set_img_L(Texture image)
    {
        img_L = image;
    }
    public void Set_img_R(Texture image)
    {
        img_R = image;
    }
}

