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
    public float switchTime = 6f; // ��ͼʱ��
    public string rootPath = "StereoImages/TempPhotos";

    private Texture2D[] images; // ���ڴ洢����Ҫ��ʾ��ͼƬ
    private int currentIndex = 0; // ��ǰ��ʾͼƬ������

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

                // �л�����һ��ͼ
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

