using UnityEngine;

public class MediaAttributes : MonoBehaviour
{
    [Header("Media Attributes")]
    [SerializeField] private string imagePath;   // ͼƬ·��
    [SerializeField] private float imageIPD = 0;   // ͼƬ·��
    [SerializeField] private bool isVideo;       // �Ƿ�����Ƶ

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }

    public float ImageIPD
    {
        get { return imageIPD; }
        set { imageIPD = value; }
    }

    public bool IsVideo
    {
        get { return isVideo; }
        set { isVideo = value; }
    }
}
