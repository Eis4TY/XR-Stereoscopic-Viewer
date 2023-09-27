using UnityEngine;

public class MediaAttributes : MonoBehaviour
{
    [Header("Media Attributes")]
    [SerializeField] private string imagePath;   // ͼƬ·��
    [SerializeField] private bool hasDepth;      // �Ƿ������
    [SerializeField] private string depthPath;   // ���ͼ·��
    [SerializeField] private bool isVideo;       // �Ƿ�����Ƶ

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }

    public string DepthPath
    {
        get { return depthPath; }
        set { depthPath = value; }
    }

    public bool HasDepth
    {
        get { return hasDepth; }
        set { hasDepth = value; }
    }

    public bool IsVideo
    {
        get { return isVideo; }
        set { isVideo = value; }
    }

}
