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
            

            // 应用新纹理
            StartCoroutine(DownloadTexture(url));
            }

        //设置媒体index
        contentControl.MediaObjs.Clear(); //清空list
        LoadGallery(GalleryGrid.transform);
        contentControl.CurrentIndex = contentControl.MediaObjs.IndexOf(this.gameObject);
    }

    private void LoadGallery(Transform parent) //加载相册
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.activeSelf)  // 或者使用 child.gameObject.activeInHierarchy
            {
                contentControl.MediaObjs.Add(child.gameObject);
            }
        }
    }

    private IEnumerator DownloadTexture(string Url) //加载图片
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Url))
        {
            yield return uwr.SendWebRequest();
            //删除旧纹理
            if (downloadedTexture != null)
            {
                Destroy(downloadedTexture);
                downloadedTexture = null;
                Destroy(thumbnail);
                thumbnail = null;
            }
            // 获取下载的纹理
            downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
            downloadedTexture.wrapMode = TextureWrapMode.Clamp;  // 设置wrapMode为Clamp
            thumbnail = TextureUtilities.ResizeTexture(downloadedTexture, 20f); //缩小图片尺寸

            // 应用新纹理
            contentControl.Set_img(downloadedTexture);

            contentControl.Set_thumbnail(thumbnail);
        }
    }
}
