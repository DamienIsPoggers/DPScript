using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Music : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;
    public AudioClip song;
    bool musicPlaying;
    float volume = 1f;

    public static Battle_Music Instance;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        
    }

    public void setSong(AudioClip s)
    {
        musicPlaying = true;
        musicSource.clip = s;
        musicSource.Play();
    }

    public void songVolume(float vol)
    {
        volume = vol;
    }

    public void playMusic(bool b)
    {
        if (b)
        {
            if (musicSource.clip == null)
            {
                musicSource.clip = song;
                musicSource.Play();
            }
            musicSource.UnPause();
        }
        else
            musicSource.Pause();

        musicPlaying = b;
    }

    public void resetSong()
    {
        musicPlaying = true;
        musicSource.clip = song;
        musicSource.Play();
    }
}
