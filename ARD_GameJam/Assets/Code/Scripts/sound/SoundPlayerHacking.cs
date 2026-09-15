using System;
using UnityEngine;

public class SoundPlayerHacking : MonoBehaviour
{
    public AudioClip m_errorAudioClip;
    public AudioClip[] m_hackingAudioClips;
    private AudioSource m_audioSource;
    private enum SoundSets
    {
        W, A, S, D, T, F, G, H, I, J, K, L
    }
    private void PlayHackingInputSound(QuickTimeBinding binding)
    {
        string s = binding.BindingInputActionReference.action.name;
        if (Enum.TryParse<SoundSets>(s, out var set))
            m_audioSource.clip = m_hackingAudioClips[(int)set];
        else
            m_audioSource.clip = m_errorAudioClip;
        m_audioSource.Play();
    }
    private void PlayCorrectOrFailSound(bool b)
    {
        if (b)
            return;
        else
            m_audioSource.clip = m_errorAudioClip;
        m_audioSource.Play();
    }
    private void MuteAudioSource(bool b)
    {
            m_audioSource.mute = b;
    }
    private void PlayEnemyHackedSound(CharStates charState)
    {
        // TODO sounds that play when you take over another robot
        m_audioSource.clip = null;
    }
    private void SubscribeEvents()
    {
        HackingManager.OnNewQuickTimeBinding += PlayHackingInputSound;
        HackingManager.OnHackingInputCorrect += PlayCorrectOrFailSound;
        soundSettings.OnIsMuted += MuteAudioSource;
        HackingManager.OnEnemyHacked += PlayEnemyHackedSound;
    }
    private void UnsubscribeEvents()
    {
        HackingManager.OnNewQuickTimeBinding -= PlayHackingInputSound;
        HackingManager.OnHackingInputCorrect -= PlayCorrectOrFailSound;
        soundSettings.OnIsMuted -= MuteAudioSource;
        HackingManager.OnEnemyHacked -= PlayEnemyHackedSound;
    }
    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnsubscribeEvents();
    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = null;
    }
}
