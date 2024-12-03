using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private FadeTransition Fader;

    [SerializeField]
    private float fadeDuration;

    private IEnumerator Start()
    {
        yield return Fader.FadeInCoroutine(fadeDuration);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {

        yield return Fader.FadeOutCoroutine(fadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
