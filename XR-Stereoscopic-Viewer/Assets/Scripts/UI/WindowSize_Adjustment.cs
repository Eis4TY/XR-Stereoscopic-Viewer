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
        // ȷ�����õ�Slider��������Ӽ����¼�
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
        if (valueText) // ����Ƿ���TMP�ı����������
        {
            valueText.text = "Size: " + value.ToString("0"); // ���ｫ��ֵ������λС��������԰����޸�
        }
    }
}
