using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SwitchEnvironment : MonoBehaviour
{
    public Camera Camera_L, Camera_R;
    public OVRPassthroughLayer Passthrough;

    public void SwitchToCubemap(bool isOn)
    {
        if (!isOn)
            return;

        Camera_L.clearFlags = CameraClearFlags.Skybox;
        Camera_R.clearFlags = CameraClearFlags.Skybox;
        Passthrough.enabled = false;
        Shader.SetGlobalFloat("_staticValue", 0.0f);
    }

    public void SwitchToPassthrough(bool isOn)
    {
        if (!isOn)
            return;

        Camera_L.clearFlags = CameraClearFlags.SolidColor;
        Camera_R.clearFlags = CameraClearFlags.SolidColor;
        Passthrough.enabled = true;
        Shader.SetGlobalFloat("_staticValue", 0.0f);
    }

    public void SwitchToPassthroughWithTransparent(bool isOn)
    {
        if (!isOn)
            return;

        Camera_L.clearFlags = CameraClearFlags.SolidColor;
        Camera_R.clearFlags = CameraClearFlags.SolidColor;
        Passthrough.enabled = true;
        Shader.SetGlobalFloat("_staticValue", 1.0f);
    }
}
