using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDisplay : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    void Update()
    {
        bool isLeftHandTracked = OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) && OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch);
        bool isRightHandTracked = OVRInput.IsControllerConnected(OVRInput.Controller.RTouch) && OVRInput.GetControllerPositionTracked(OVRInput.Controller.RTouch);

        if (isLeftHandTracked)
        {
            leftHand.SetActive(true);
        }
        else
        {
            leftHand.SetActive(false);
        }

        if (isRightHandTracked)
        {
            rightHand.SetActive(true);
        }
        else
        {
            rightHand.SetActive(false);
        }
    }
}
