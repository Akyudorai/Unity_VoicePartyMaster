using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private enum AudioChannel 
    {
        BGM, Ambience1, Ambience2
    }

    [Header("BGM")]
    public List<AudioClip> BGM = new List<AudioClip>();
    public AudioSource BGM_Source;
    private int bgm_index;
    private float bgm_duration;

    private void Start()
    {
        // Initialize BGM
        if (BGM.Count > 0) {
            bgm_index = Random.Range(0, BGM.Count);
            BGM_Source.clip = BGM[bgm_index];
            Play(AudioChannel.BGM);
        }


    }

    private void Update()
    {
        // Update BGM
        if (bgm_duration > 0) bgm_duration -= Time.deltaTime;
        if (bgm_duration <= 0.0f) {
            if (BGM.Count > bgm_index + 1) {
                bgm_index++;
                BGM_Source.clip = BGM[bgm_index];
            } else {
                bgm_index = 0;
                BGM_Source.clip = BGM[bgm_index];
                Play(AudioChannel.BGM);
            }
        }
    }

    private void Play(AudioChannel channel)
    {
        switch (channel) 
        {
            case AudioChannel.BGM:
                bgm_duration = BGM_Source.clip.length;
                BGM_Source.Play();
                break;
            case AudioChannel.Ambience1:
                break;
            case AudioChannel.Ambience2:
                break;
        }
    }
}
