using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class IPD_Adjustment : MonoBehaviour
{
    public Slider targetSlider;
    public TextMeshProUGUI valueText;  
    public Transform leftPlane, rightPlane;

    private void Start()
    {
        // ȷ�����õ�Slider��������Ӽ����¼�
        if (targetSlider)
        {
            targetSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        UpdateTextValue(targetSlider.value);
    }
    
    private void OnSliderValueChanged(float IPDvalue)
    {
        leftPlane.localPosition = new Vector3(-IPDvalue / 2, 0.0f, 0.0f);
        rightPlane.localPosition = new Vector3(IPDvalue / 2, 0.0f, 0.0f);

        UpdateTextValue(IPDvalue * 100);
    }

    private void UpdateTextValue(float value)
    {
        if (valueText) // ����Ƿ���TMP�ı����������
        {
            valueText.text = "IPD: " + value.ToString("0"); // ���ｫ��ֵ������λС��������԰����޸�
        }
    }
}
