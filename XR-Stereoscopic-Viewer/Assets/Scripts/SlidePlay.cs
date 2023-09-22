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
        // ��ȡ�������ļ���·��
        folders = Directory.GetDirectories(rootPath)
                          .OrderBy(x => x).ToArray();

        // ���ص�һ���ļ��е�_L�ļ�
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

                // �л�����һ�ļ���
                currentIndex++;
                if (currentIndex >= folders.Length)
                {
                    currentIndex = 0;
                }

                // ����ͼƬ
                LoadImageFromFolder(folders[currentIndex]);

                contentControl.Set_img_L(currentImage_L);
                contentControl.Set_img_R(currentImage_R);
            }
        }
    }

    void LoadImageFromFolder(string folder)
    {
        // �ҵ��ļ��������ִ�_L�ĵ�һ��ͼƬ
        string imagePath_L = Directory.GetFiles(folder)
                                    .FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).EndsWith("_L"));

        string imagePath_R = Directory.GetFiles(folder)
                                    .FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).EndsWith("_R"));

        // ��������
        currentImage_L = Resources.Load<Texture>(imagePath_L);
        currentImage_R = Resources.Load<Texture>(imagePath_R);
    }

    float lastSwitchTime;
}

