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

            // Ӧ��������
            StartCoroutine(DownloadTexture(url));
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
            }
            // ��ȡ���ص�����
            downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
            // Ӧ��������
            contentControl.Set_img(downloadedTexture);
        }
    }
    private IEnumerator DownloadTexture_D(string Url) //�������ͼƬ
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Url))
        {
            yield return uwr.SendWebRequest();
            //ɾ��������
            if (downloadedTexture_D != null)
            {
                Destroy(downloadedTexture_D);
                downloadedTexture_D = null;
            }
            // ��ȡ���ص�����
            downloadedTexture_D = DownloadHandlerTexture.GetContent(uwr);
            // Ӧ��������
            contentControl.Set_imgDeep(downloadedTexture_D);
        }
    }
}
