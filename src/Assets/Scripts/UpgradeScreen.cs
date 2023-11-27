using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeScreen : MonoBehaviour
{
    public string nextSceneName;

    public void OnSpeedUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Speed);
        Debug.Log("Vroooom vroooom! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Speed));
        PlayAudio();
        SceneManager.LoadSceneAsync("milestone2Scene");
    }

    public void OnBatteryUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Battery);
        Debug.Log("Powerbank docked! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Battery));
        PlayAudio();
        SceneManager.LoadSceneAsync("milestone2Scene");
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

        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
