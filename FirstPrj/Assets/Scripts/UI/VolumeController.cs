using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class VolumeController : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle muteToggle;

    private string[] paramNames = { "MasterVolume", "BGMVolume", "SFXVolume" };

    private float lastVolume;
    private void OnEnable()
    {
        dropDown.onValueChanged.AddListener(OnChannelChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteChanged);
    }

    private void OnChannelChanged(int index)
    {
        audioMixer.GetFloat(paramNames[index], out float currentDb); //볼륨을 받아온다
        volumeSlider.SetValueWithoutNotify(DbToLinear(currentDb)); // 받아온 볼륨을 슬라이더에 적용한다
    }

    private void OnVolumeChanged(float value)
    {
        int index = dropDown.value;

        float dB;
        if(value <= 0.001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(value) * 20;
        }
        audioMixer.SetFloat(paramNames[index], dB);
    }

    private void OnMuteChanged(bool isOn)
    {
        if (isOn)
        {
            audioMixer.GetFloat(paramNames[0], out lastVolume);
            audioMixer.SetFloat(paramNames[0], -80f);
        }
        else
        {
            audioMixer.SetFloat(paramNames[0], lastVolume);
        }
    }

    private float DbToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }
}
