using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    private List<float> audioSourceVolumes;

    private SoundEffectManager()
    {
        audioSourceVolumes = new List<float>();
    }

    private void Awake()
    {
        StaticObjects.SoundEffectManager = this;
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
        audioSources[soundId].Play();
    }

    public void UpdateVolume(float newSoundVolume)
    {
        for (int i = 0; i < audioSourceVolumes.Count; i++)
        {
            audioSources[i].volume = audioSourceVolumes[i] * newSoundVolume;
        }
    }
}
