using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeScreen : MonoBehaviour
{
    private AudioSource musicSource;

    private void Start()
    {
        AudioListener.pause = false;
        Cursor.visible = true;
        musicSource = SoundController.SoundInstance.WorkshopMusic.Play();
    }

    public void OnSpeedUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Speed);
        Debug.Log("Vroooom vroooom! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Speed));
        PlayAudio();
    }

    public void OnBatteryUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Battery);
        Debug.Log("Powerbank docked! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Battery));
        PlayAudio();
    }

    public void OnStealthUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Stealth);
        Debug.Log("Powerbank docked! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Stealth));
        PlayAudio();
    }



    public void PlayAudio()
    {
        AudioSource audioSource = SoundController.SoundInstance.Upgrade.Play();
        StartCoroutine(WaitForSound(audioSource));
    }

    public IEnumerator WaitForSound(AudioSource source)
    {
        yield return new WaitUntil(() => source.isPlaying == false);

        LoadNextScene();
    }

    public void LoadNextScene()
    {
        ProgressManager.Instance.workshopVisited = true;
        string nextSceneName = ProgressManager.Instance.GetNextLevelName();
        Cursor.visible = false;
        musicSource.Stop();
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
