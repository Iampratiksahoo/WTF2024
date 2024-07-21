using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }

    public AudioSource bkp_source;
    public AudioClip shoot;
    public AudioClip[] footstep;
    public AudioClip[] growls;
    public AudioClip bite;
    public AudioClip playerDeath;

    [Range(0f, 1f)] public float globalFootstepVol = .35f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetMusicEnabled(bool enabled)
    {
        AudioListener.volume = enabled ? 1 : 0; 
    }

    public AudioClip GetFootstep()
    {
        return footstep[Random.Range(0, footstep.Length)];
    }

    public AudioClip GetGrowls()
    {
        return growls[Random.Range(0, growls.Length)];
    }

    public void PlayOnBackup(AudioClip clip, float vol)
    {
        bkp_source.volume = vol;
        bkp_source.clip = clip;
        bkp_source.Play();
    }
}
