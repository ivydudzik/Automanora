using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField]
    private Image FadeImage;

    // Fade code from Ketra Games https://www.youtube.com/watch?v=vkOhefMbrFg
    
    public IEnumerator FadeInCoroutine(float fadeInDuration)
    {
        Color startColor = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1);
        Color targetColor = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 0);

        yield return FadeCoroutine(startColor, targetColor, fadeInDuration);
        
        gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float fadeOutDuration)
    {
        Color startColor = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 0);
        Color targetColor = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1);
        
        gameObject.SetActive(true);
        yield return FadeCoroutine(startColor, targetColor, fadeOutDuration);
    }

    public IEnumerator FadeCoroutine(Color startColor, Color targetColor, float fadeDuration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / fadeDuration;
            FadeImage.color = Color.Lerp(startColor, targetColor, elapsedPercentage);
            yield return null;
            elapsedTime += Time.deltaTime;
        }      
    }
}
