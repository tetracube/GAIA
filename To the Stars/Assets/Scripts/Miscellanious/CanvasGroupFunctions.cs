using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CanvasGroup))]
public class CanvasGroupFunctions : MonoBehaviour 
{
    private CanvasGroup m_canvasGroup;

    void Awake()
    {
        m_canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    public void VanishGroup(float time)
    {
        StartCoroutine("Vanish", time);
    }

    IEnumerator Vanish(float time)
    {
        float timeStarted = Time.unscaledTime;
        float originalAlpha = m_canvasGroup.alpha;

        while(Time.unscaledTime - timeStarted < time)
        {
            m_canvasGroup.alpha  = Mathf.Lerp(originalAlpha, 0, (Time.unscaledTime - timeStarted) / time);
            yield return null;
        }

        m_canvasGroup.alpha = 0;
    }

    public void AppearGroup(float time)
    {
        StartCoroutine("Appear", time);
    }

    IEnumerator Appear(float time)
    {
        float timeStarted = Time.unscaledTime;

        while (Time.unscaledTime - timeStarted < time)
        {
            m_canvasGroup.alpha = Mathf.Lerp(0, 1, (Time.unscaledTime - timeStarted) / time);
            yield return null;
        }

        m_canvasGroup.alpha = 1;
    }
}
