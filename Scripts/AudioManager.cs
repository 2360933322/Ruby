using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour{
    public static AudioManager instance { get; private set; }


    private AudioSource audioS;

    void Start()
    {
        instance = this;
        audioS = GetComponent<AudioSource>();
    }
    /// <summary>
    /// 播放指定的音效
    /// </summary>
    /// <param name="clip"></param>
    public void AudioPlay(AudioClip clip)
    {
        audioS.PlayOneShot(clip);
    }
}
