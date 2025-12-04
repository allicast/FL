using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public IEnumerator FadeIn(float duration)
    {
        Color c = fadeImage.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = t / duration;
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(c.r, c.g, c.b, 1);
    }
    public IEnumerator FadeOut(float duration)
    {
        Color c = fadeImage.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / duration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(c.r, c.g, c.b, 0);
    }
}
