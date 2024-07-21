using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public Image loader;

    private void Start()
    {
        loader.fillAmount = 0;

        StartCoroutine(LoadGameSceneAsync());
    }
    public IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        while (!operation.isDone)
        {
            loader.fillAmount = operation.progress;
            yield return null;
        }

    }
}
