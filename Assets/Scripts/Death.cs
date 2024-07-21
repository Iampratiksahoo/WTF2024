using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] ParticleSystem deathSmoke;
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] float deathSoundVol = .3f;

    [ContextMenu("Die")]
    public void Die()
    {
        ParticleSystem death = Instantiate(deathSmoke);
        death.transform.position = transform.position;  
        death.transform.localScale = scale;
        death.Play();

        SoundManager.Instance.PlayOnBackup(SoundManager.Instance.playerDeath, deathSoundVol);
        gameObject.SetActive(false);
        // DestroyImmediate(gameObject);
    }

}
