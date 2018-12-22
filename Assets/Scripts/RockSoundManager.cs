using UnityEngine;

public class RockSoundManager : MonoBehaviour
{
    private AudioSource[] audioSources;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void PlaySound(int soundId)
    {
        audioSources[soundId].Play();
    }
}
