using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialPanel;
    public TextMeshProUGUI DiegeticText;
    public TextMeshProUGUI InstructionText;
    public GameObject Player;
    private Jump jumpComponent;
    private Battery scooterBattery;
    private int tutorialIndex;
    private bool keyboardUsed;

    public GameObject DeadEndBox;
    private TriggerBox triggerBoxComponent1;

    public GameObject DropBox1;
    private TriggerBox triggerBoxComponent2;

    public GameObject DropBox2;
    private TriggerBox triggerBoxComponent3;
    
    public GameObject DroneBox1;
    private TriggerBox triggerBoxComponent4;

    public GameObject DroneBox2;
    private TriggerBox triggerBoxComponent5;

    public GameObject ClearBox1;
    private TriggerBox triggerBoxComponent6;

    public GameObject ClearBox2;
    private TriggerBox triggerBoxComponent7;

    public GameObject DropIndicatorPanel;
    public GameObject IndicatorImage;


    private string[] tutorialPhrases =
    {
        "scooter's ready. time to see, what this baby can do!",
        "",
        "whoa. easy there!",
        "this looks like a dead end.",
        "damn! out of battery!",
        "much better.",
        "one of the drops is near.",
        "careful now! police drone ahead!",
        "phew. close call. one more drop tonight."
    };
   private string[] tutorialInstructions =
    {
        "hold down 'w' to move forward. scroll mouse wheel up to increase scooter speed.",
        "use 'a'/'d' for steering.",
        "scroll mouse wheel down to decrease scooter speed. press 'space' to break.", // Kind of convoluted
        "use 's' to reverse. alternatively, you can jump low obstacles with 'shift'.",
        "look for the blue charging stations to charge scooter battery.",
        "remember to keep the battery charged or scooter speed will be severely limited.",
        "when near a drop, the drop indicator bar at the top left will fill up according to how close you are. go towards the pink drop location.",
        "stay out of the drone's light cone to avoid detection. you can also sneak up on drones and disable them with 'q', but it's a high risk action.", 
        "drones will chase you down when they see you and imprison you, resulting in game over. finish the last drop to complete the level."
    };

    private string[] tutorialInstructionsGamePad =
    {
        "hold left stick up to move forward. use 'RT' up to increase scooter speed.",
        "move the left stick sideways for steering.",
        "use 'LT' to decrease scooter speed. press 'LB' to break.", // Kind of convoluted
        "hold left stick down to reverse. alternatively, you can jump low obstacles with 'Y'.",
        "look for the blue charging stations to charge scooter battery.",
        "remember to keep the battery charged or scooter speed will be severely limited.",
        "when near a drop, the drop indicator bar at the top left will fill up according to how close you are. go towards the pink drop location.",
        "stay out of the drone's light cone to avoid detection. you can also sneak up on drones and disable them with 'X', but it's a high risk action.",
        "drones will chase you down when they see you and imprison you, resulting in game over. finish the last drop to complete the level."
    };

    private void Awake()
    {
        Events.OnControlSchemeChange += ChangeControls;
    }

    private void OnDestroy()
    {
        Events.OnControlSchemeChange -= ChangeControls;
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorialIndex = 0;
        scooterBattery = Player.GetComponent<Battery>();

        //jumpComponent = Player.GetComponent<Jump>();

        triggerBoxComponent1 = DeadEndBox.GetComponent<TriggerBox>();

        triggerBoxComponent2 = DropBox1.GetComponent<TriggerBox>();
        triggerBoxComponent3 = DropBox2.GetComponent<TriggerBox>();
        
        triggerBoxComponent4 = DroneBox1.GetComponent<TriggerBox>();
        triggerBoxComponent5 = DroneBox2.GetComponent<TriggerBox>();

        triggerBoxComponent6 = ClearBox1.GetComponent<TriggerBox>();
        triggerBoxComponent7 = ClearBox2.GetComponent<TriggerBox>();

        DropIndicatorPanel.SetActive(false);
        IndicatorImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!TutorialPanel.activeSelf)
        {
            switch (tutorialIndex)
            {
                case 0:
                    {
                        //jumpComponent.forwardForce = 0;
                        //jumpComponent.upForce = 0;
                        StartCoroutine(TextBlock(tutorialIndex, 2f, 3.5f));
                        tutorialIndex++;
                    }
                    break;
                case 1:
                    {
                        if (scooterBattery.CurrentCapacity < 85)
                        {
                            StartCoroutine (TextBlock(tutorialIndex, 0, 0));
                            tutorialIndex++;
                        }
                    }
                    break;
                case 2:
                    {
                        if(scooterBattery.CurrentCapacity < 40 && Player.GetComponent<Rigidbody>().velocity.magnitude > 15f)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 3:
                    {
                        if (triggerBoxComponent1.HasEntered)
                        {
                            //jumpComponent.upForce = 6f;
                            //jumpComponent.forwardForce = 2f;
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 4:
                    {
                        if (scooterBattery.CurrentCapacity < 1f)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }

                    } 
                    break;
                    case 5: 
                    {
                        if (scooterBattery.CurrentCapacity > 80)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }

                    }
                    break;
                    case 6: 
                    {
                        if (triggerBoxComponent2.HasEntered || triggerBoxComponent3.HasEntered)
                        {
                            DropIndicatorPanel.SetActive(true);
                            IndicatorImage.SetActive(true);
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 7: {
                        if (triggerBoxComponent4.HasEntered || triggerBoxComponent5.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 8:
                    {
                        if (triggerBoxComponent6.HasEntered || triggerBoxComponent7.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
            }
        } 
    }

    private void DisplayText(GameObject TextPanel, TextMeshProUGUI textField, string textToDisplay)
    {
        textField.text = textToDisplay;
        if (TextPanel.activeSelf == false) { 
            TextPanel.SetActive(true);
        }
    }

    private IEnumerator TextBlock(int stateIndex, float timeToStart, float intervalTime)
    {
        yield return new WaitForSeconds(timeToStart); // Set to zero, if player says nothing (empty string)

        DisplayText(TutorialPanel, DiegeticText, tutorialPhrases[stateIndex]);

        yield return new WaitForSeconds(intervalTime); // Set to zero, if player says nothing (empty string)

        if (keyboardUsed)
            DisplayText(TutorialPanel, InstructionText, tutorialInstructions[stateIndex]);
        else
            DisplayText(TutorialPanel, InstructionText, tutorialInstructionsGamePad[stateIndex]);

        yield return new WaitForSeconds(3.5f);

        DisplayText(TutorialPanel, DiegeticText, "");

        yield return new WaitForSeconds(intervalTime);

        DisplayText(TutorialPanel, InstructionText, "");

        TutorialPanel.SetActive(false);
    }

    private void ChangeControls(string newScheme)
    {
        if (newScheme == "KeyboardMouse")
        {
            keyboardUsed = true;
        }
        else if (newScheme == "Gamepad")
        {
            keyboardUsed = false;
        }
    }
}
