using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayNext : MonoBehaviour
{
    public ContentControl contentControl;
    private Texture2D downloadedTexture, thumbnail;

    public IPD_Adjustment ipd_Adjustment;

    public void LastMedia() //上一张
    {
        if (contentControl.MediaObjs.Count > 0)
        {
            int lastIndex;
            if (contentControl.CurrentIndex == 0)
            {
                lastIndex = contentControl.MediaObjs.Count - 1;
            }
            else
            {
                lastIndex = contentControl.CurrentIndex - 1;
            }
            MediaAttributes currentMedia = contentControl.MediaObjs[lastIndex].transform.GetComponent<MediaAttributes>();

            ipd_Adjustment.CurrentImage = currentMedia;
            ipd_Adjustment.targetSlider.value = currentMedia.ImageIPD;
            ipd_Adjustment.ChangeImgIPD(currentMedia.ImageIPD);

            SetContent(currentMedia);
            contentControl.CurrentIndex = lastIndex;
        }
    }

    public void NextMedia() //下一张
    {
        if (contentControl.MediaObjs.Count > 0)
        {
            int nextIndex = (contentControl.CurrentIndex + 1) % contentControl.MediaObjs.Count;
            MediaAttributes currentMediaAttributes = contentControl.MediaObjs[nextIndex].transform.GetComponent<MediaAttributes>();

            ipd_Adjustment.CurrentImage = currentMediaAttributes;
            ipd_Adjustment.targetSlider.value = currentMediaAttributes.ImageIPD;
            ipd_Adjustment.ChangeImgIPD(currentMediaAttributes.ImageIPD);

            SetContent(currentMediaAttributes);
            contentControl.CurrentIndex = nextIndex;
        }
    }


    private void SetContent(MediaAttributes mediaAttributes)
    {
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
