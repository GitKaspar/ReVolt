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

    public GameObject DroneBox1;
    private TriggerBox triggerBoxComponent2;

    public GameObject DroneBox2;
    private TriggerBox triggerBoxComponent3;

    public GameObject DropIndicatorPanel;
    public GameObject IndicatorImage;


    private string[] tutorialPhrases =
    {
        "scooter's ready. time to see, what this baby can do!",
        "whoa. easy there!",
         "",
        "this looks like a dead end.",
        "damn! out of battery!",
        "much better.",
        "should go look for places to spread my message.",
        "careful now! police drone ahead!"
    };
   private string[] tutorialInstructions =
    {
        "hold down 'w' to move forward. scroll mouse wheel up to increase scooter speed.",
        "scroll mouse wheel down to decrease scooter speed. press 'space' to break.", // Kind of convoluted
        "use 'a'/'d' for steering.",
        "use 's' to reverse. alternatively, you can jump low obstacles with 'shift'.",
        "look for the blue charging stations to charge scooter battery.",
        "remember to keep the battery charged or scooter speed will be severely limited.",
        "pay attention to the drop indicator bar at the top left. it will fill up more the closer you get to a drop. drops are marked in pink. find the closest one!",
        "stay out of the drone's light cone to avoid detection. you can also sneak up on drones and disable them with 'q', but it's a high risk action."
    };

    private string[] tutorialInstructionsGamePad =
    {
        "hold left stick up to move forward. use 'RT' up to increase scooter speed.",
        "use 'LT' to decrease scooter speed. press 'LB' to break.", // Kind of convoluted
        "move the left stick sideways for steering.",
        "hold left stick down to reverse. alternatively, you can jump low obstacles with 'Y'.",
        "look for the blue charging stations to charge scooter battery.",
        "remember to keep the battery charged or scooter speed will be severely limited.",
        "pay attention to the drop indicator bar at the top left. it will fill up more the closer you get to a drop. drops are marked in pink. find the closest one!",
        "stay out of the drone's light cone to avoid detection. you can also sneak up on drones and disable them with 'X', but it's a high risk action."
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
        
        triggerBoxComponent2 = DroneBox1.GetComponent<TriggerBox>();
        triggerBoxComponent3 = DroneBox2.GetComponent<TriggerBox>();

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
                        StartCoroutine(TextBlock(tutorialIndex, 2f, 3f));
                        tutorialIndex++;
                    }
                    break;
                case 1:
                    {
                        if (scooterBattery.CurrentCapacity < 90 && Player.GetComponent<Rigidbody>().velocity.magnitude > 10f)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 1f));
                            tutorialIndex++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (scooterBattery.CurrentCapacity < 80)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 1f));
                            tutorialIndex++;
                        }
                    }
                    break;
                case 3:
                    {
                        if (triggerBoxComponent1.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 3f));
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
                            StartCoroutine(TextBlock(tutorialIndex, 1f, 4f));
                            tutorialIndex++;
                        }

                    }
                    break;
                case 6:
                    {
                        DropIndicatorPanel.SetActive(true);
                        IndicatorImage.SetActive(true);
                        StartCoroutine(TextBlock(tutorialIndex, 5f, 3f));
                        tutorialIndex++;
                    }
                    break;
                case 7:
                    {
                        if (triggerBoxComponent2.HasEntered || triggerBoxComponent3.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                        break;
                    }
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
