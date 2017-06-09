using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiaoBoxTool  {


    public static IEnumerator FadeUISprite(UISprite target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;
            yield return null;
        }


    }

    public static IEnumerator FadePanel(UIPanel target, float duration,  float targetalpha)
    {
        if (target == null)
            yield break;

        float alpha = target.alpha;
        float t = 0;
        for ( t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            float newalpha = Mathf.SmoothStep(alpha, targetalpha, t);
            target.alpha = newalpha;
            yield return null;
        }
        float finalalpha = Mathf.SmoothStep(alpha, targetalpha, t);
        target.alpha = finalalpha;
    }
    public static IEnumerator FadeText(UILabel target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;
        float t=0f;
        for ( t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;
            yield return null;
        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
        target.color = finalColor;
    }
    
    public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.material.color.a;

        float t = 0f;
        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.material.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
        target.material.color = finalColor;
    }
}
