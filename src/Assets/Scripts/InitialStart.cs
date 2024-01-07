using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialStart : MonoBehaviour
{
    public GameObject buttons;
    private Animator animator;

    private static bool initial = true;

    // Start is called before the first frame update
    void Start()
    {
        if (initial)
        {
            animator = GetComponent<Animator>();
            buttons.SetActive(false);
            WaitForGlowUp();
            initial = false;
        }

    }

    public void WaitForGlowUp()
    {
        StartCoroutine(WaitForAnimationFinish());
    }

    public IEnumerator WaitForAnimationFinish()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"));

        EnableButtons();
    }

    public void EnableButtons()
    {
        buttons.SetActive(true);
    }
}
