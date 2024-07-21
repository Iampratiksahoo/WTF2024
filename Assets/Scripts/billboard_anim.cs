using System.Collections;
using UnityEngine;

public class billboard_anim : MonoBehaviour
{
    public AudioSource source;
    public GameObject[] ons;
    public AudioClip on_clip;
    public AudioClip off_clip;

    public float delay = 1f;

    private void Start()
    {
        StartCoroutine(anim());
    }

    IEnumerator anim()
    {
        while (true)
        {
            foreach (var on in ons)
            {
                if(on.activeInHierarchy)
                {
                    source.clip = off_clip;
                    on.SetActive(false);
                }
                else
                {
                    source.clip = on_clip;
                    on.SetActive(true);
                }
                source.Play();
            }
            yield return new WaitForSeconds(delay);
        }
    }
}