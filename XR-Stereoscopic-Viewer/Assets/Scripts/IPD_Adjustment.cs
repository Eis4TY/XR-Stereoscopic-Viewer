using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPD_Adjustment : MonoBehaviour
{
    [Range(-0.5f, 1.5f)]
    public float IPD = 0;

    public Transform leftPlane, rightPlane;
    void Update()
    {
        leftPlane.localPosition = new Vector3(-IPD / 2, 0.0f, 0.0f);
        rightPlane.localPosition = new Vector3(IPD / 2, 0.0f, 0.0f);
    }
}
