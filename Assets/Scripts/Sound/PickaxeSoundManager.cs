using UnityEngine;

public class PickaxeSoundManager : MonoBehaviour
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
