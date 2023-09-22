using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class SlidePlay : MonoBehaviour
{
    public bool isPlay = false;
    public ContentControl contentControl;

    private Texture currentImage_L, currentImage_R;
    private string rootPath = "Assets/Resources/StereoImages/@Mr.Maginary";
    private string[] folders;
    private int currentIndex = 0;

    void Start()
    {
        // 获取所有子文件夹路径
        folders = Directory.GetDirectories(rootPath)
                          .OrderBy(x => x).ToArray();

        // 加载第一个文件夹的_L文件
        LoadImageFromFolder(folders[0]);


    }

    // Update is called once per frame
    void Update()
    {
        if (isPlay)
        {
            if (Time.time - lastSwitchTime > 6)
            {
                lastSwitchTime = Time.time;

                // 切换到下一文件夹
                currentIndex++;
                if (currentIndex >= folders.Length)
                {
                    currentIndex = 0;
                }

                // 加载图片
                LoadImageFromFolder(folders[currentIndex]);

                contentControl.Set_img_L(currentImage_L);
                contentControl.Set_img_R(currentImage_R);
            }
        }
    }

    void LoadImageFromFolder(string folder)
    {
        // 找到文件夹中名字带_L的第一张图片
        string imagePath_L = Directory.GetFiles(folder)
                                    .FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).EndsWith("_L"));

        string imagePath_R = Directory.GetFiles(folder)
                                    .FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).EndsWith("_R"));

        // 加载纹理
        currentImage_L = Resources.Load<Texture>(imagePath_L);
        currentImage_R = Resources.Load<Texture>(imagePath_R);
    }

    float lastSwitchTime;
}

