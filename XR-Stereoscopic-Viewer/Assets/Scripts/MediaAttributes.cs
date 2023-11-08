using UnityEngine;

public class MediaAttributes : MonoBehaviour
{
    [Header("Media Attributes")]
    [SerializeField] private string imagePath;   // 图片路径
    [SerializeField] private bool isVideo;       // 是否是视频

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
