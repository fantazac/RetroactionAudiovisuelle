using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    private List<float> audioSourceVolumes;

    private float fadeSpeed;

    private BGMManager()
    {
        fadeSpeed = 0.6f;
        audioSourceVolumes = new List<float>();
    }

    private void Awake()
    {
        StaticObjects.BGMManager = this;
    }

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSourceVolumes.Add(audioSource.volume);
        }

        PlayBGM(0);
    }

    public void PlaySound(int soundId)
    {
        audioSources[soundId].Play();
    }

    public void PlayBGM(int soundId)
    {
        AudioSource playingAudioSource = null;
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                playingAudioSource = audioSource;
                break;
            }
        }

        if (playingAudioSource != null)
        {
            StartCoroutine(FadeSounds(playingAudioSource, audioSources[soundId]));
        }
        else
        {
            audioSources[soundId].Play();
        }
    }

    private IEnumerator FadeSounds(AudioSource playingAudioSource, AudioSource toPlayAudioSource)
    {
        float finalVolume = toPlayAudioSource.volume;
        float playingFinalVolume = playingAudioSource.volume;
        toPlayAudioSource.volume = 0;

        while (finalVolume < playingAudioSource.volume)
        {
            playingAudioSource.volume -= Time.deltaTime * fadeSpeed;

            yield return null;
        }

        toPlayAudioSource.Play();

        while (toPlayAudioSource.volume < finalVolume && playingAudioSource.volume > 0)
        {
            if (toPlayAudioSource.volume < finalVolume)
            {
                toPlayAudioSource.volume += Time.deltaTime * fadeSpeed;
            }
            if (playingAudioSource.volume > 0)
            {
                playingAudioSource.volume -= Time.deltaTime * fadeSpeed;
            }

            yield return null;
        }

        toPlayAudioSource.volume = finalVolume;
        playingAudioSource.Stop();
        playingAudioSource.volume = playingFinalVolume;
    }

    public void UpdateVolume(float newSoundVolume)
    {
        for (int i = 0; i < audioSourceVolumes.Count; i++)
        {
            audioSources[i].volume = audioSourceVolumes[i] * newSoundVolume;
        }
    }
}
