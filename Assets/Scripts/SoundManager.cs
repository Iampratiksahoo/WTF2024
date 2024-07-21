using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }

    public AudioClip shoot; 

    AudioSource _ambientSource;

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

        _ambientSource = GetComponent<AudioSource>();
    }

   public void SetMusicEnabled(bool enabled)
   {
        if(enabled)
        {
            if(!_ambientSource.isPlaying)
            {
                _ambientSource.Play();
            }
        }
        else
        {
            if (_ambientSource.isPlaying)
            {
                _ambientSource.Stop();
            }
        }
   }
}
