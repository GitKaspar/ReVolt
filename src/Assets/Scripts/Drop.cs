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

    public GameObject NewspaperPrefab;
    private Transform player;

    private string dropButton = "e";
    private void Awake()
    {
        Events.OnControlSchemeChange += ChangeButton;
    }

    private void OnDestroy()
    {
        Events.OnControlSchemeChange -= ChangeButton;
    }

    private void ChangeButton(string newScheme)
    {
        if (newScheme == "KeyboardMouse")
        {
            dropButton = "e";
        }
        else if (newScheme == "Gamepad")
        {
            dropButton = "A";
        }
    }

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
            player = other.transform;

            GameController.GameControllerInstance.PromtText.text = "press '" + dropButton + "' to drop";
            GameController.GameControllerInstance.PromtPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            droppable = false;
            player = null;
            GameController.GameControllerInstance.PromtPanel.SetActive(false);
        }
    }

    private void DoDrop()
    {
        done = true;
        AudioClip chosenClip = SoundController.SoundInstance.Drops.Clips[Random.Range(0, SoundController.SoundInstance.Drops.Clips.Count)];
        source.PlayOneShot(chosenClip);
        Events.DropDone(this);

        GameObject newspaper = Instantiate(NewspaperPrefab, player.position + Vector3.up * 1.5f, Quaternion.Euler(90,0,0));
        newspaper.GetComponent<Rigidbody>().AddForce((transform.position - player.position + Vector3.up) * 20, ForceMode.Impulse); //throw newspaper at drop

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (var rend in renderers)
        {
            rend.enabled = false;
            //rend.material = MatDropped;
        }
    }
}