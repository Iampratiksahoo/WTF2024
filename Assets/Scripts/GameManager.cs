using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float cameraLerpSpeed;
    public Transform playerCamera;
    public Transform gameOverCameraPos;
    public GameObject[] disableObjectsOnEnd;

    public AudioClip gameOverAudio;

    public GameObject winUI;
    public GameObject loseUI;

    public PlayerController player;

    AudioSource _audioSource; 
    bool _isGameover = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _isGameover = false;

        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

    private void Update()
    {
        if(!_isGameover)
        {
            float progress = ZombieManager.Instance._affectedZombies.Count - 1;
            float max = PedestrianManager.Instance._pedestrians.Count;

            // Win 
            if(progress >= max)
            {
                Won();
            }
        }
        else
        {
            if(Vector3.Distance(playerCamera.position, gameOverCameraPos.position) > 0.01f)
            {
                playerCamera.position = Vector3.Lerp(playerCamera.position, gameOverCameraPos.position, cameraLerpSpeed * Time.deltaTime);
                playerCamera.rotation = Quaternion.Lerp(playerCamera.rotation, gameOverCameraPos.rotation, cameraLerpSpeed * Time.deltaTime);
            }
        }

    }

    public void Won()
    {
        GameOverCoro(true);
    }

    public void Lost()
    {
        GameOverCoro(false);
    }

    void GameOverCoro(bool won)
    {
        foreach (GameObject obj in disableObjectsOnEnd)
        {
            obj.SetActive(false);
        }

        player.controlCamera = false;

        _isGameover = true;

        _audioSource.clip = gameOverAudio;
        _audioSource.Play();

        winUI.SetActive(won);
        loseUI.SetActive(!won);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
