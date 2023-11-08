using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowToolTip : MonoBehaviour
{
    public GameObject tip2_gallery, tip2_content;

    void Start()
    {
        tip2_gallery.SetActive(false);
        tip2_content.SetActive(false);
    }

    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            tip2_gallery.SetActive(true);
            tip2_content.SetActive(true);
        }
        else
        {
            tip2_gallery.SetActive(false);
            tip2_content.SetActive(false);
        }
    }
}
