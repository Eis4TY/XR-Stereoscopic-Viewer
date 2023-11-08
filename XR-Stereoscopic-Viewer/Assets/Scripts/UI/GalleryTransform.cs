using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GalleryTransform : MonoBehaviour
{
    public RectTransform windowUI;
    [Space]
    public Image Image;
    [Space]
    public GameObject shadow;
    public GameObject maskedViewport;
    public GameObject controls;
    public GameObject frame;
    [Space]
    public GridLayoutGroup gridLayout;

    private ScrollRect scrollRect;
    private RectMask2D rectMask;

    void Start()
    {
        scrollRect = maskedViewport.GetComponent<ScrollRect>();
        rectMask = maskedViewport.GetComponent<RectMask2D>();

        SetGalleryLarge();
    }

    public void SetGallerySmall()
    {
        Image.enabled = false;
        shadow.SetActive(false);
        controls.SetActive(false);
        frame.SetActive(false);
        maskedViewport.GetComponent<Image>().enabled = false;

        scrollRect.vertical = false;   // 禁用垂直滚动
        scrollRect.horizontal = true;  // 启用水平滚动

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        gridLayout.spacing = new Vector2Int(20, 0); //添加间距

        rectMask.softness = new Vector2Int(100, 0); //添加两端遮罩

        windowUI.sizeDelta = new Vector2Int(1048, 205); //缩小窗口
        windowUI.localScale = new Vector3(0.3f, 0.3f, 1);
        windowUI.anchoredPosition = new Vector2Int(0, -360);

        float gridX = gridLayout.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        gridLayout.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(gridX, 0); //修正Grid的Y值

        MasksState(gridLayout.gameObject, true);
    }

    public void SetGalleryLarge()
    {
        Image.enabled = true;
        shadow.SetActive(true);
        controls.SetActive(true);
        frame.SetActive(true);
        maskedViewport.GetComponent<Image>().enabled = true;

        scrollRect.vertical = true;   // 禁用垂直滚动
        scrollRect.horizontal = false;  // 启用水平滚动

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 5;

        gridLayout.spacing = new Vector2Int(4, 0); //还原间距

        rectMask.softness = new Vector2Int(0, 0); //还原两端遮罩

        windowUI.sizeDelta = new Vector2Int(1048, 720); //还原窗口
        windowUI.localScale = Vector3.one;
        windowUI.anchoredPosition = Vector2.zero;

        gridLayout.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); //修正Grid的X值
        MasksState(gridLayout.gameObject, false);
        
    }

    void MasksState(GameObject obj, bool isOpen)
    {
        // Get all Mask components on this GameObject and its children
        Mask[] masks = obj.GetComponentsInChildren<Mask>();

        // Loop through the array and disable each Mask component
        foreach (Mask mask in masks)
        {
            mask.enabled = isOpen;
        }
    }
}
