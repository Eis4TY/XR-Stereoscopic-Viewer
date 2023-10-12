using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayNext : MonoBehaviour
{
    public ContentControl contentControl;
    private Texture2D downloadedTexture, downloadedTexture_D;

    public void LastMedia()
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
            SetContent(currentMedia);
            contentControl.CurrentIndex = lastIndex;
        }
    }

    public void NextMedia()
    {
        if (contentControl.MediaObjs.Count > 0)
        {
            int nextIndex = (contentControl.CurrentIndex + 1) % contentControl.MediaObjs.Count;
            MediaAttributes currentMediaAttributes = contentControl.MediaObjs[nextIndex].transform.GetComponent<MediaAttributes>();
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
            if (mediaAttributes.HasDepth)
            {
                string url_D = mediaAttributes.DepthPath;
                contentControl.isDepth = true;
                StartCoroutine(DownloadTexture_D(url_D));
            }
            else
            {
                contentControl.isDepth = false;
            }

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
            }
            // 获取下载的纹理
            downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
            // 应用新纹理
            contentControl.Set_img(downloadedTexture);
        }
    }
    private IEnumerator DownloadTexture_D(string Url) //加载深度图片
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Url))
        {
            yield return uwr.SendWebRequest();
            //删除旧纹理
            if (downloadedTexture_D != null)
            {
                Destroy(downloadedTexture_D);
                downloadedTexture_D = null;
            }
            // 获取下载的纹理
            downloadedTexture_D = DownloadHandlerTexture.GetContent(uwr);
            // 应用新纹理
            contentControl.Set_imgDeep(downloadedTexture_D);
        }
    }
}
