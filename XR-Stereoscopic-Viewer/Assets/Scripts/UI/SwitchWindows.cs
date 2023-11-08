using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Video;

public class SwitchWindows : MonoBehaviour
{
    public float scaleDuration = 0.3f;

    private GameObject content;
    private GameObject gallery;
    private GameObject settings;
    private GalleryTransform galleryTransform;

    private Vector3 galleryOrigin = Vector3.one;
    private Vector3 contentOrigin = new Vector3(1250, 1250, 1250);
    private Vector3 settingsOrigin = new Vector3(0.75f, 0.75f, 0.75f);
    private Vector3 gallerySmall = new Vector3(0.0001f, 0.0001f, 0.0001f);

    void Start()
    {
        content = GameObject.Find("Content");
        gallery = GameObject.Find("Gallery");
        settings = GameObject.Find("Settings");
        content.transform.localScale = gallerySmall;
        gallery.transform.localScale = galleryOrigin;
        settings.transform.localScale = gallerySmall;
        //gallery.transform.position = new Vector3(0, 0, gallery.transform.position.z);
    }

    public void OpenMedia()
    {
        content = GameObject.Find("Content");
        gallery = GameObject.Find("Gallery");
        settings = GameObject.Find("Settings");
        galleryTransform = GameObject.Find("SwitchWindows").GetComponent<GalleryTransform>();
        galleryTransform.SetGallerySmall();

        content.transform.localScale = new Vector3(1250, 1250, 1250);
        Debug.Log("$$$$$$$content=" + content.transform.localScale.x);

        settings.transform.localScale = settingsOrigin;
    }

    public void CloseMedia()
    {
        content = GameObject.Find("Content");
        gallery = GameObject.Find("Gallery");
        settings = GameObject.Find("Settings");

        content.transform.localScale = gallerySmall;
        gallery.transform.localScale = galleryOrigin;
        settings.transform.localScale = gallerySmall;

        GameObject.Find("Plane_L").GetComponent<VideoPlayer>().Stop(); //Õ£÷π ”∆µ
        GameObject.Find("Plane_R").GetComponent<VideoPlayer>().Stop(); //Õ£÷π ”∆µ
    }

}
