using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadGameSceneAsync());
    }
    public IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        while (!operation.isDone)
        {
            yield return null;
        }

    }
}
