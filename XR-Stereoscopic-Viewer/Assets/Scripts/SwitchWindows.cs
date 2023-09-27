using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWindows : MonoBehaviour
{
    private GameObject content;
    private GameObject gallery;
    private Vector3 galleryOrigin = new Vector3 (0.002f, 0.002f, 0.002f);
    private Vector3 contentOrigin = new Vector3(3f, 3f, 3f);

    void Start()
    {
        content = GameObject.Find("Content");
        gallery = GameObject.Find("Gallery");
        content.transform.localScale = Vector3.zero;
        gallery.transform.localScale = galleryOrigin;
        gallery.transform.position = new Vector3(0, 0, 2.05f);
    }

    public void OpenMedia()
    {
        content = GameObject.Find("Content");
        gallery = GameObject.Find("Gallery");
        content.transform.localScale = contentOrigin;
        gallery.transform.localScale = Vector3.zero;
    }

    public void CloseMedia()
    {
        content = GameObject.Find("Content");
        gallery = GameObject.Find("Gallery");
        content.transform.localScale = Vector3.zero;
        gallery.transform.localScale = galleryOrigin;
    }
}
