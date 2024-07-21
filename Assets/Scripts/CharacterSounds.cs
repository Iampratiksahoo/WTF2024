using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class CharacterSounds : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    public void PlayFootstep()
    {
        if(audioSource != null)
        {
            if(audioSource.isPlaying) 
            {
                audioSource.Stop();
            }
            audioSource.clip = SoundManager.Instance.GetFootstep();
            audioSource.volume = SoundManager.Instance.globalFootstepVol;
            audioSource.Play();
        }
    }

    private void OnValidate()
    {
        var aSrc = GetComponent<AudioSource>();
        aSrc.loop = false;
        aSrc.playOnAwake = false;
        aSrc.spatialBlend = 1;
    }
}