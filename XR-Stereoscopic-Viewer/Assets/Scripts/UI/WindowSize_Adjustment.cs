using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowSize_Adjustment : MonoBehaviour
{
    public Slider targetSlider;
    public TextMeshProUGUI valueText;
    public Transform StereoImg;
    public Transform ViewWindow;
    public Transform gallery;

    public Transform content;

    private void Start()
    {
        // 确认引用到Slider，并且添加监听事件
        if (targetSlider)
        {
            targetSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        UpdateTextValue(targetSlider.value);
    }

    private void OnSliderValueChanged(float SizeValue)
    {
        gallery.localScale = new Vector3(SizeValue, SizeValue, SizeValue);
        content.localScale = new Vector3(1250*SizeValue, 1250 * SizeValue, 1250 * SizeValue);
        UpdateTextValue(SizeValue);
    }

    private void UpdateTextValue(float value)
    {
        if (valueText) // 检查是否有TMP文本组件被引用
        {
            valueText.text = "Size: " + value.ToString("1"); // 这里将数值保留两位小数，你可以按需修改
        }
    }
}
