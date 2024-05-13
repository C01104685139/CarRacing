using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider audioSlider;

    public void BGMControl()
    {
        float sound = audioSlider.value;

        if(sound == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", sound);
    }

    public void EffectSoundControl()
    {
        float sound = audioSlider.value;

        if(sound == -40f) masterMixer.SetFloat("EffectSound", -80);
        else masterMixer.SetFloat("EffectSound", sound);
    }
}
