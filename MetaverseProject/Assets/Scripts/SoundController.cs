using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    AudioSource bgm_player;
    AudioSource efs_player;

    public AudioMixer mixer;
    public Slider BS_Slider;
    public Slider ES_Slider;

    public static SoundController instance; // SoundController.instance.PlaySound("");���������� �߰��ؼ� ȿ���� ����

    void Start()
    {
        BS_Slider.value = PlayerPrefs.GetFloat("BGM", 0.75f);
    }

    void Awake()
    {
        instance = this;
       // bgm_player = GameObject.Find("BGM").GetComponent<AudioSource>();
       // efs_player = GameObject.Find("EffectSound").GetComponent<AudioSource>();

        BS_Slider.onValueChanged.AddListener(ChangeBgmSound); //�����̴��� BGM����
        ES_Slider.onValueChanged.AddListener(ChangeEfsSound); //�����̴��� ȿ��������
    }

    public AudioClip[] audio_clips;

    public void PlaySound(string type) //ȿ���� clip index�� ��ġ���Ѽ� �����ϱ�
    {
        int index = 0;

        switch (type) {
            case "Boost" : index = 0; break;
            case "Crash" : index = 1; break;
            case "Drift" : index = 2; break;
        }

        efs_player.clip = audio_clips[index];
        efs_player.Play();
    }

    public void ChangeBgmSound(float volume) //�����̴� BGM ��������
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume)* 20);
    }
    public void ChangeEfsSound(float volume) //�����̴� ȿ���� ��������
    {
        mixer.SetFloat("EffectSound", Mathf.Log10(volume)* 20);
    }
}
