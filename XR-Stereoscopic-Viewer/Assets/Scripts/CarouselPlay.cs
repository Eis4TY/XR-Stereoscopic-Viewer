using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class CarouselPlay : MonoBehaviour
{
    /*
    public bool isPlay = false;
    public ContentControl contentControl;
    public float switchTime = 6f; // 换图时间
    public string rootPath = "StereoImages/TempPhotos";

    private Texture2D[] images; // 用于存储所有要显示的图片
    private int currentIndex = 0; // 当前显示图片的索引

    void Start()
    {
        images = Resources.LoadAll<Texture2D>(rootPath);

        if (images.Length > 0)
        {
            contentControl.Set_img(images[0]);
        }
    }

    void Update()
    {
        if (isPlay)
        {
            if (Time.time - lastSwitchTime > switchTime)
            {
                lastSwitchTime = Time.time;

                // 切换到下一张图
                currentIndex++;
                if (currentIndex >= images.Length)
                {
                    currentIndex = 0;
                }

                contentControl.Set_img(images[currentIndex]);
            }
        }
    }


    float lastSwitchTime;
    */
}

