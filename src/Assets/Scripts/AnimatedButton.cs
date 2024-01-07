using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(BlinkingTextAnimation))]
public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField, Space(10f)]
    private UnityEvent _onClick;

    private Button _thisButton;

    private BlinkingTextAnimation hoverAnim;
    private ClickedButtonAnimation clickAnim;
    [SerializeField]
    private bool useClickAnimation = true;
    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        hoverAnim = GetComponent<BlinkingTextAnimation>();

        if (useClickAnimation)
        {
            clickAnim = GetComponent<ClickedButtonAnimation>();

            _thisButton.onClick.AddListener(PlayClickAnimation);
            clickAnim.OnFinished += CallAction;
        }
    }

    private void PlayClickAnimation()
    {
        SoundController.SoundInstance.ButtonClick(); //Give instant feedback that button was pressed
        GetComponent<BlinkingTextAnimation>().enabled = false; //Disable blinking -> make text appear again
        clickAnim.enabled = true; //Enable script that controls button pressed animation
    }

    private void CallAction()
    {
        _onClick.Invoke();
    }


    //Start hover animation on entering button with pointer or selecting it by other means, stop it on unhover/deselect

    public void OnPointerEnter(PointerEventData eventData)
    {
       hoverAnim.enabled = true;      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverAnim.enabled = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        hoverAnim.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        hoverAnim.enabled = false;
    }
}
