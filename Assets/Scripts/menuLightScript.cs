using System.Collections;
using UnityEngine;

public class menuLightScript : MonoBehaviour
{
    public Vector2 timeRange = Vector2.one;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
            animator.Play($"menulight_" + Random.Range(0, 3));
        }
    }
}