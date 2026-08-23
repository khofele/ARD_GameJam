using UnityEngine;

public class soundSettings : MonoBehaviour
{
    private bool isPlaying = true;
    public AudioSource audioSource;
    public void ToggleSound()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
        }
        else
        {
            audioSource.Play();
            isPlaying = true;
        }
            
    }
}
