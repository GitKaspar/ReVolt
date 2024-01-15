using System;
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

    private AudioSource riotSource;
    private AudioSource winMusicSource;
    private AudioClipGroup winMusic;

    private void Awake()
    {
        revolutionMessage.text = ProgressManager.Instance.revolutionMessage;
        background = GetComponent<Image>();
        winMusic = SoundController.SoundInstance.WorkshopMusic;
    }
    private void OnEnable()
    {
        StartCoroutine(FadeToBlack());
    }

    private void OnDestroy()
    {
        if (riotSource != null)
        {
            riotSource.Stop();
            riotSource.ignoreListenerPause = false;
        }

        if (winMusic != null)
        {
            winMusicSource.Stop();
            winMusicSource.ignoreListenerPause = false;
            winMusic.SetVolume(0.5f);
        }
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

        riotSource = SoundController.SoundInstance.Riot.Play();
        riotSource.ignoreListenerPause = true;
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

        ContentCover.enabled = false;
        winMusicSource = SoundController.SoundInstance.WorkshopMusic.Play();
        winMusicSource.ignoreListenerPause = true;
        winMusic.SetVolume(0f);

        StartCoroutine(FadeInMusic());
    }

    public IEnumerator FadeInMusic()
    {
        while (winMusic.VolumeMax < 0.5f)
        {
            winMusic.SetVolume(winMusic.VolumeMax + Time.unscaledDeltaTime * fadeSpeed);
            yield return null;
        }
    }
}
