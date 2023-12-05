using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;



public class ClickControl : MonoBehaviour
{
    private GameObject content;
    private ContentControl contentControl;

    private GameObject GalleryGrid;

    private Texture2D downloadedTexture;
    private Texture2D thumbnail;

    private IPD_Adjustment ipd_Adjustment;


    private void Start()
    {
        content = GameObject.Find("Content");
        contentControl = content.GetComponent<ContentControl>();

        GalleryGrid = GameObject.Find("Grid");

        ipd_Adjustment = GameObject.Find("AppFunctions").GetComponent<IPD_Adjustment>();
    }


    public void SetContent()
    {
        MediaAttributes mediaAttributes = this.GetComponent<MediaAttributes>();
        ipd_Adjustment.CurrentImage = mediaAttributes;
        ipd_Adjustment.targetSlider.value = mediaAttributes.ImageIPD;
        ipd_Adjustment.ChangeImgIPD(mediaAttributes.ImageIPD);

        string url = mediaAttributes.ImagePath;
        
        if (mediaAttributes.IsVideo)
        {
            contentControl.Set_isVideo(true);
            contentControl.Set_videoURL(url);
        }
        else
        {
            contentControl.Set_isVideo(false);
            

            // Ӧ��������
            StartCoroutine(DownloadTexture(url));
            }

        //����ý��index
        contentControl.MediaObjs.Clear(); //���list
        LoadGallery(GalleryGrid.transform);
        contentControl.CurrentIndex = contentControl.MediaObjs.IndexOf(this.gameObject);
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
            //ɾ��������
            if (downloadedTexture != null)
            {
                Destroy(downloadedTexture);
                downloadedTexture = null;
                Destroy(thumbnail);
                thumbnail = null;
            }
            // ��ȡ���ص�����
            downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
            downloadedTexture.wrapMode = TextureWrapMode.Clamp;  // ����wrapModeΪClamp
            thumbnail = TextureUtilities.ResizeTexture(downloadedTexture, 20f); //��СͼƬ�ߴ�

            // Ӧ��������
            contentControl.Set_img(downloadedTexture);

            contentControl.Set_thumbnail(thumbnail);
        }
    }
}
