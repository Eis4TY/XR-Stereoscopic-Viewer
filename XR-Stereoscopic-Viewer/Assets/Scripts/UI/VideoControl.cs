using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;  // Ϊ��ʹ��EventTrigger


public class VideoControl : MonoBehaviour
{
    public Slider progressSlider;
    public VideoPlayer player1;
    public VideoPlayer player2;
    [Space]
    public Image img_play;
    public Sprite icon_Play;
    public Sprite icon_Pause;
    [Space]
    public Image img_mute;
    public Sprite icon_Unmute;
    public Sprite icon_Mute;

    //private bool isDragging = false;  // �����ı�־�����ڼ��������Ƿ��϶�
    

    private void Start()
    {
        img_play.sprite = icon_Pause;
        img_mute.sprite = icon_Mute;
    }

    private void Update()
    {
        if (player1.isPlaying && player1.isPrepared)  // ֻ�е��û����϶�������ʱ���Ÿ��½�������ֵ
        {
            //double videoLength = player1.frameCount / player1.frameRate;
            progressSlider.value = (float)(player1.time / player1.length) ;
        }
    }


    public void ChangeProgress(float value)
    {
        if (player1.isPlaying)
        {
            player1.Pause();
            player2.Pause();

            player1.time = value * player1.length;
            player2.time = value * player2.length;

            player1.Play();
            player2.Play();
        }
        else
        {
            player1.time = value * player1.length;
            player2.time = value * player2.length;
        }
    }


    public void TogglePlayPause() //������ͣ
    {
        if (player1.isPlaying && player2.isPlaying)
        {
            player1.Pause();
            player2.Pause();
            img_play.sprite = icon_Play;
        }
        else
        {
            player1.Play();
            player2.Play();
            img_play.sprite = icon_Pause;
        }
    }

    public void ToggleMute() // �л�����
    {
        if (player1.GetDirectAudioMute(0)) // �����ǰ�Ѿ���
        {
            for (ushort i = 0; i < player1.audioTrackCount; i++)
            {
                player1.SetDirectAudioMute(i, false);
            }

            img_mute.sprite = icon_Unmute; 
        }
        else  // �����ǰδ����
        {
            for (ushort i = 0; i < player1.audioTrackCount; i++)
            {
                player1.SetDirectAudioMute(i, true);
            }

            img_mute.sprite = icon_Mute;
        }
    }

}
