using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //Audio mixer helps balance overall sounds in game
    public AudioMixer mixer;

    public AudioSource bgMusic;

    public AudioClip[] gameAudioclips;
    public AudioSource gameAudio;

    public AudioClip[] uiAudioclips;
    public AudioSource uiAudio;

    void Awake()
    {
        MuteEnvironmentAudio();
    }

    public void PlayBgMusic()
    {
        bgMusic.Play();
    }

    public void StopBgMusic()
    {
        bgMusic.Stop();
    }

    public void GameOverAudio()
    {
        gameAudio.PlayOneShot(gameAudioclips[0], 0.5f);
    }

    public void StartEndButtonAudio()
    {
        uiAudio.PlayOneShot(uiAudioclips[0], 0.5f);
    }

    public void DefaultButtonAudio()
    {
        uiAudio.PlayOneShot(uiAudioclips[1], 0.5f);
    }

    public void MuteEnvironmentAudio()
    {
        mixer.SetFloat("EnvVolume", -80f);
    }

    public void UnmuteEnvironmentAudio()
    {
        mixer.SetFloat("EnvVolume", 0f);
    }
}
