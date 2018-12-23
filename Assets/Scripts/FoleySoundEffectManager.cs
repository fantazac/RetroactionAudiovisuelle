using System.Collections.Generic;
using UnityEngine;

public class FoleySoundEffectManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    private List<float> audioSourceVolumes;

    private FoleySoundEffectManager()
    {
        audioSourceVolumes = new List<float>();
    }

    private void Awake()
    {
        StaticObjects.FoleySoundEffectManager = this;
    }

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSourceVolumes.Add(audioSource.volume);
        }
    }

    public void PlaySound(int soundId)
    {
        if (!audioSources[soundId].isPlaying)
        {
            audioSources[soundId].Play();
        }
    }

    public void StopSound(int soundId)
    {
        if (audioSources[soundId].isPlaying)
        {
            audioSources[soundId].Stop();
        }
    }

    public void UpdateVolume(float newSoundVolume)
    {
        for (int i = 0; i < audioSourceVolumes.Count; i++)
        {
            audioSources[i].volume = audioSourceVolumes[i] * newSoundVolume;
        }
    }
}
