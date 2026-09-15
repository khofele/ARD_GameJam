using System;
using UnityEngine;

public class soundSettings : MonoBehaviour
{
    private bool isPlaying = true;
    public AudioSource audioSource;

    public static event Action<bool> OnIsMuted;

    public void ToggleSound()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
            OnIsMuted?.Invoke(true);
        }
        else
        {
            audioSource.UnPause();
            isPlaying = true;
            OnIsMuted?.Invoke(false);
        }
    }
}
