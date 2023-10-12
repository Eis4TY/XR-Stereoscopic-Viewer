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
        // ȷ�����õ�Slider��������Ӽ����¼�
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
        if (valueText) // ����Ƿ���TMP�ı����������
        {
            valueText.text = "Deep: " + value.ToString("0"); // ���ｫ��ֵ����0λС��������԰����޸�
        }
    }
}
