using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deep_Adjustment : MonoBehaviour
{
    public Slider targetSlider;
    public TextMeshProUGUI valueText;
    public ContentControl contentControl;

    private void Start()
    {
        // 确认引用到Slider，并且添加监听事件
        if (targetSlider)
        {
            targetSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        UpdateTextValue(targetSlider.value);
    }

    private void OnSliderValueChanged(float deepValue)
    {
        contentControl.DeepValue = deepValue;
        UpdateTextValue(deepValue * 10);
    }

    private void UpdateTextValue(float value)
    {
        if (valueText) // 检查是否有TMP文本组件被引用
        {
            valueText.text = "Deep: " + value.ToString("0"); // 这里将数值保留0位小数，你可以按需修改
        }
    }
}
