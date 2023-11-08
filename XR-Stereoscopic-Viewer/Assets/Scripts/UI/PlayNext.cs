using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayNext : MonoBehaviour
{
    public ContentControl contentControl;
    private Texture2D downloadedTexture, thumbnail;

    public void LastMedia() //��һ��
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

    public void NextMedia() //��һ��
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
