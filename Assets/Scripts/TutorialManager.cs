using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorial_box;
    public TextMeshProUGUI tutorial_text;
    public PlayerController player;
    public GameObject ped;
    public GameObject progressBar; 

    public const string TUTORIAL_PREF_KEY = "FIRST_PLAY_THROUGH";
    private void Awake()
    {
        // It's not the first playthough of the player
        if(PlayerPrefs.GetInt(TUTORIAL_PREF_KEY, 0) != 0)
        {
            tutorial_box.SetActive(false);
            progressBar.SetActive(true);
            Destroy(gameObject);
        }
        PlayerPrefs.SetInt(TUTORIAL_PREF_KEY, 1);
    }

    IEnumerator Start()
    {
        // Enable the tutorial box 
        tutorial_box.SetActive(true);
        ped.SetActive(false);
        progressBar.SetActive(false);

        // Move tutorial 
        tutorial_text.text = "Click on the ground to move to any location";
        yield return new WaitUntil(() => player._agent.velocity.magnitude != 0);
        yield return new WaitUntil(() => player._agent.velocity.magnitude == 0);

        // Attack tutorial 
        ped.SetActive(true);
        tutorial_text.text = "Go near this human and press E to turn them to zombie";
        yield return new WaitUntil(() => player._zombieCreator._currentVictim != null);
        yield return new WaitForSecondsRealtime(3f);

        // Progress bar 
        tutorial_text.text = "The top bar indicates the human to zombie ratio";
        progressBar.SetActive(true);
        yield return new WaitForSeconds(.5f);
        progressBar.SetActive(false);
        yield return new WaitForSeconds(.5f);
        progressBar.SetActive(true);
        yield return new WaitForSeconds(.5f);
        progressBar.SetActive(false);
        yield return new WaitForSeconds(.5f);
        progressBar.SetActive(true);
        yield return new WaitForSeconds(3f);

        // Police 
        tutorial_text.text = "Keep an eye for the cops, they do not like you!!";
        yield return new WaitForSecondsRealtime(3f);

        // Turn All 
        tutorial_text.text = "Turn all the humans into zombies!";
        yield return new WaitForSeconds(2.5f);

        tutorial_box.SetActive(false);
        DestroyImmediate(gameObject);
    }
}