using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Player : MonoBehaviour
{
    [SerializeField] AudioSource tictoc;
    [SerializeField] AudioSource bell;

    public static Sound_Player instance;
    bool is_playing_tictoc = false;

    private void Awake()
    {
        instance = this;
    }
    
    public void Play_Tictoc()
    {
        if (tictoc.isPlaying)
            return;
        tictoc.Play();
        is_playing_tictoc = true;
    }

    public void Play_Bell()
    {
        bell.Play();
    }

    public void Stop_Tictoc()
    {
        tictoc.Stop();
        is_playing_tictoc = false;
    }

    public void Pause_Tictoc()
    {
        tictoc.Pause();
    }

    public void Continue_Tictoc()
    {
        if (is_playing_tictoc)
            tictoc.Play();
    }
}
