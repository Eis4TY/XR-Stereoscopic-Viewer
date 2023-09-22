using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgGroup : MonoBehaviour
{
    public Texture img_L1, img_R1;
    public Texture img_L2, img_R2;
    public Texture img_L3, img_R3;
    public Texture img_L4, img_R4;
    public Texture img_L5, img_R5;
    public Texture img_L6, img_R6;
    public Texture img_L7, img_R7;
    public Texture img_L8, img_R8;
    public Texture img_L9, img_R9;

    private Texture[] textureGroups_L;
    private Texture[] textureGroups_R;

    [Space]
    public ContentControl contentControl;

    private int currentIndex = 0;

    void Start()
    {
        textureGroups_L = new Texture[] { img_L1, img_L2, img_L3, img_L4, img_L5, img_L6, img_L7, img_L8, img_L9 };
        textureGroups_R = new Texture[] { img_R1, img_R2, img_R3, img_R4, img_R5, img_R6, img_R7, img_R8, img_R9 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSwitchTime > 6)
        {
            lastSwitchTime = Time.time;

            // 切换到下一文件夹
            currentIndex++;
            if (currentIndex >= textureGroups_L.Length)
            {
                currentIndex = 0;
            }

            contentControl.Set_img_L(textureGroups_L[currentIndex]);
            contentControl.Set_img_R(textureGroups_R[currentIndex]);
        }
    }
    float lastSwitchTime;
}
