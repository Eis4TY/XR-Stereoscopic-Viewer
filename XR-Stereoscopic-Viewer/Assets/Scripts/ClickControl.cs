using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;



public class ClickControl : MonoBehaviour
{
    private ContentControl contentControl;
    private GameObject content;
    private GameObject GalleryGrid;

    private void Start()
    {
        content = GameObject.Find("Content");
        contentControl = content.GetComponent<ContentControl>();

        GalleryGrid = GameObject.Find("Grid");
    }


    public void SetContent()
    {
        MediaAttributes mediaAttributes = this.GetComponent<MediaAttributes>();
        string url = mediaAttributes.ImagePath;
        
        if (mediaAttributes.IsVideo)
        {
            contentControl.Set_isVideo(true);
            contentControl.Set_videoURL(url);
        }
        else
        {
            contentControl.Set_isVideo(false);
            if (gameObject.GetComponent<MediaAttributes>().HasDepth)
            {
                string url_D = mediaAttributes.DepthPath;
                contentControl.isDepth = true;
                StartCoroutine(DownloadTexture_D(url_D));
            }
            else
            {
                contentControl.isDepth = false;
            }

            // Ӧ��������
            StartCoroutine(DownloadTexture(url));
        }

        //����ý��index
        contentControl.MediaObjs.Clear(); //���list
        LoadGallery(GalleryGrid.transform);
        contentControl.CurrentIndex = contentControl.MediaObjs.IndexOf(this.gameObject);
        Debug.Log($"����{contentControl.MediaObjs.Count}��ý�壬��ǰ�ǵ�{contentControl.CurrentIndex}��");
    }

    private void LoadGallery(Transform parent) //�������
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.activeSelf)  // ����ʹ�� child.gameObject.activeInHierarchy
            {
                contentControl.MediaObjs.Add(child.gameObject);
            }
        }
    }

    private IEnumerator DownloadTexture(string Url) //����ͼƬ
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Url))
        {
            yield return uwr.SendWebRequest();
            // ��ȡ���ص�����
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
            // Ӧ��������
            contentControl.Set_img(downloadedTexture);
        }
    }
    private IEnumerator DownloadTexture_D(string Url) //�������ͼƬ
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Url))
        {
            yield return uwr.SendWebRequest();
            // ��ȡ���ص�����
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
            // Ӧ��������
            contentControl.Set_imgDeep(downloadedTexture);
        }
    }
}
