using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource musicWin;
    public AudioSource musicLose;
    public AudioSource musicCongrats;
    public static SoundManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void OnBackGroundMusic()
    {
        backgroundMusic.Play();
    }
    public void OffBackGroundMusic()
    {
        backgroundMusic.Pause();
    }
    public void OnMusicWin()
    {
        musicWin.Play();
    }

    public void OffMusicWin()
    {
        musicWin.Stop();
    }
    public void OnMusicLose()
    {
        musicLose.Play();
    }

    public void OffMusicLose()
    {
        musicLose.Stop();
    }
    public void OnMusicCongrats()
    {
        musicCongrats.Play();
    }
    public void OffMusicCongrats()
    {
        musicCongrats.Stop();
    }

}
