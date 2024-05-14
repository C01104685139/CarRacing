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

    public static SoundController instance; // SoundController.instance.PlaySound("");물리구현에 추가해서 효과음 설정

    void Start()
    {
        BS_Slider.value = PlayerPrefs.GetFloat("BGM", 0.75f);
    }

    void Awake()
    {
        instance = this;
       // bgm_player = GameObject.Find("BGM").GetComponent<AudioSource>();
       // efs_player = GameObject.Find("EffectSound").GetComponent<AudioSource>();

        BS_Slider.onValueChanged.AddListener(ChangeBgmSound); //슬라이더로 BGM조절
        ES_Slider.onValueChanged.AddListener(ChangeEfsSound); //슬라이더로 효과음조절
    }

    public AudioClip[] audio_clips;

    public void PlaySound(string type) //효과음 clip index와 매치시켜서 연결하기
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

    public void ChangeBgmSound(float volume) //슬라이더 BGM 볼륨설정
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume)* 20);
    }
    public void ChangeEfsSound(float volume) //슬라이더 효과음 볼륨설정
    {
        mixer.SetFloat("EffectSound", Mathf.Log10(volume)* 20);
    }
}
