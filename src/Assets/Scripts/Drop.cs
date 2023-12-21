using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drop : MonoBehaviour
{
    private PlayerControls.PlayerActions actions;

    public Material MatDropped;
    public Material MatOpen;
    private bool droppable;
    [HideInInspector]
    public bool done;
    // Kaspar's addition
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        actions = ControlsInstance.GetActions();
        actions.Drop.performed += OnDrop;
    }

    public void OnDrop(InputAction.CallbackContext ctx)
    {
        if (droppable && !done)
        {
            DoDrop();
            GameController.GameControllerInstance.PromtPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!done && other.CompareTag("Player"))
        {
            droppable = true;

            GameController.GameControllerInstance.PromtText.text = "press 'e' to drop";
            GameController.GameControllerInstance.PromtPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            droppable = false;
            GameController.GameControllerInstance.PromtPanel.SetActive(false);
        }
    }

    private void DoDrop()
    {
        done = true;
        AudioClip chosenClip = SoundController.SoundInstance.Drops.Clips[Random.Range(0, SoundController.SoundInstance.Drops.Clips.Count)];
        source.PlayOneShot(chosenClip);
        Events.DropDone(this);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (var rend in renderers)
        {
            rend.material = MatDropped;
        }
    }
}