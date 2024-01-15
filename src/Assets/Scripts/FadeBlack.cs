using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    public Image ContentCover;
    public GameObject Content;
    public TextMeshProUGUI revolutionMessage;
    public float fadeSpeed;
    private Image background;

    private void Awake()
    {
        revolutionMessage.text = ProgressManager.Instance.revolutionMessage;
        background = GetComponent<Image>();
    }
    private void OnEnable()
    {
        StartCoroutine(FadeToBlack());
    }

    public IEnumerator FadeToBlack()
    {
        yield return new WaitForEndOfFrame();

        Color color;
        float newAlpha = 0;

        while (newAlpha < 1)
        {
            newAlpha += fadeSpeed * Time.unscaledDeltaTime;
            color = new Color(0, 0, 0, newAlpha);
            background.color = color;
            yield return null;
        }

        StartCoroutine(FadeToTransparent());
    }

    public IEnumerator FadeToTransparent()
    {
        yield return new WaitForEndOfFrame();

        ContentCover.color = Color.black;
        Content.SetActive(true);

        Color color;
        float newAlpha = 1;

        while (newAlpha > 0)
        {
            newAlpha -= fadeSpeed * Time.unscaledDeltaTime;
            color = new Color(0, 0, 0, newAlpha);
            ContentCover.color = color;
            yield return null;
        }
    }
}
