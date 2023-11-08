using UnityEngine;

public class MediaAttributes : MonoBehaviour
{
    [Header("Media Attributes")]
    [SerializeField] private string imagePath;   // ͼƬ·��
    [SerializeField] private bool isVideo;       // �Ƿ�����Ƶ

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }


    public bool IsVideo
    {
        get { return isVideo; }
        set { isVideo = value; }
    }

}
