using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollProgress : MonoBehaviour
{

    public ScrollRect scrollRect;
    private float scrollProgress;

    public void ScrollRecord()
    {
        scrollProgress = scrollRect.horizontalNormalizedPosition;
        Debug.Log("������¼��" + scrollProgress);
    }

    public void ScrollSet(float scrollValue)
    {
        scrollRect.horizontalNormalizedPosition = scrollValue;
    }
}
