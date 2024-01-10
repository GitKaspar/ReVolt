using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeScreen : MonoBehaviour
{

    private void Start()
    {
        ProgressManager.Instance.workshopVisited = true;
        AudioListener.pause = false;
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


    public void PlayAudio()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        StartCoroutine(WaitForSound(audioSource));
    }

    public IEnumerator WaitForSound(AudioSource source)
    {
        yield return new WaitUntil(() => source.isPlaying == false);

        LoadNextScene();
    }

    public void LoadNextScene()
    {
        string nextSceneName = ProgressManager.Instance.GetNextLevelName();
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
