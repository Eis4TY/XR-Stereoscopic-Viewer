using UnityEngine;

public class MediaAttributes : MonoBehaviour
{
    [Header("Media Attributes")]
    [SerializeField] private string imagePath;   // 图片路径
    [SerializeField] private bool hasDepth;      // 是否有深度
    [SerializeField] private string depthPath;   // 深度图路径
    [SerializeField] private bool isVideo;       // 是否是视频

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
