using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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

    public GameObject ChargingStations;
    public GameObject InvisibleWalls;

    private static bool[] dropsDone;
    private static int staticTutorialIndex;
    private static bool isRetry = false;

    private string[] tutorialPhrases =
    {
        "Scooter's ready. Time to see, what this baby can do!",
        "Whoa. Easy there!",
         "",
        "this looks like a dead end",
        "Damn! Out of battery!",
        "much better",
        "should keep a lookout for places to spread the word",
        "Careful now! Police drone ahead!",
        "",
        "",
        ""
    };
   private string[] tutorialInstructions =
    {
        "Hold down 'w' to move forward. Scroll mouse wheel up to increase scooter speed.",
        "Scroll mouse wheel down to decrease scooter speed. Press 'space' to break.", // Kind of convoluted
        "use 'a'/'d' for steering",
        "Use 's' to reverse. Alternatively, you can jump low obstacles with 'shift'.",
        "look for the blue charging stations to charge scooter battery",
        "remember to keep the battery charged or scooter speed will be severely limited",
        "Pay attention to the drop indicator bar at the top left. It will fill up more the closer you get to a drop. Drops are marked in pink. Find the closest one!",
        "Stay out of the drone's light cone to avoid detection. You can also sneak up on drones and disable them with 'q', but it's a high risk action.",
        "Use the mouse and 'right mouse button' to look around. Press 'f' to toggle flashlight",
        "find the last drop to finish the level",
        "You got busted, but we got you. go finish up delivering to the drops. And don't get caught this time!"
    };

    private string[] tutorialInstructionsGamePad =
    {
        "Hold left stick up to move forward. Use 'RT' up to increase scooter speed.",
        "Use 'LT' to decrease scooter speed. Press 'LB' to break.", // Kind of convoluted
        "move the left stick sideways for steering",
        "Hold left stick down to reverse. Alternatively, you can jump low obstacles with 'Y'.",
        "look for the blue charging stations to charge scooter battery",
        "remember to keep the battery charged or scooter speed will be severely limited",
        "Pay attention to the drop indicator bar at the top left. It will fill up more the closer you get to a drop. Drops are marked in pink. Find the closest one!",
        "Stay out of the drone's light cone to avoid detection. You can also sneak up on drones and disable them with 'X', but it's a high risk action.",
        "Use the right stick to look around. Press 'RB' to toggle flashlight",
        "find the last drop to finish the level",
        "You got busted, but we got you. Go finish up delivering to the drops. And don't get caught this time!"
    };

    private void Awake()
    {
        Events.OnControlSchemeChange += ChangeControls;
        Events.OnEndGame += StoreProgress;
        Events.OnDropDone += DropDone;
    }

    private void DropDone(Drop drop)
    {
        if (dropsDone == null) dropsDone = new bool[2];
        Drop[] drops = GetComponent<GameController>().Drops;
        for (int i = 0; i < drops.Length; i++)
        {
            if (drops[i] == drop)
                dropsDone[i] = true;
        }
    }

    private void StoreProgress(bool isWin)
    {
        if (!isWin)
        {
            isRetry = true;
            staticTutorialIndex = tutorialIndex;
        }
    }

    private void OnDestroy()
    {
        Events.OnControlSchemeChange -= ChangeControls;
        Events.OnEndGame -= StoreProgress;
        Events.OnDropDone -= DropDone;
    }

    // Start is called before the first frame update
    void Start()
    {
        triggerBoxComponent1 = DeadEndBox.GetComponent<TriggerBox>();
        triggerBoxComponent2 = DroneBox1.GetComponent<TriggerBox>();
        triggerBoxComponent3 = DroneBox2.GetComponent<TriggerBox>();
        scooterBattery = Player.GetComponent<Battery>();

        if (isRetry)
        {
            tutorialIndex = staticTutorialIndex;
            Drop[] drops = GetComponent<GameController>().Drops;
            for (int i = 0; i < drops.Length; i++)
            {
                if (dropsDone[i])
                {
                    Player.transform.position = drops[i].transform.position + Vector3.up + Vector3.forward;
                    drops[i].DoTutorialRetryDrop(Player.transform);
                }
            }

            DropIndicatorPanel.SetActive(true);
            IndicatorImage.SetActive(true);
            ChargingStations.SetActive(true);
            InvisibleWalls.SetActive(false);

            StartCoroutine(TextBlock(10, 1f, 2f));

        }
        else
        {
            tutorialIndex = 0;
            DropIndicatorPanel.SetActive(false);
            IndicatorImage.SetActive(false);
            ChargingStations.SetActive(false);
        }

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
                        StartCoroutine(TextBlock(tutorialIndex, 1f, 3f));
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
                            InvisibleWalls.SetActive(false);
                            StartCoroutine(TextBlock(tutorialIndex, 0, 3f));
                            tutorialIndex++;
                        }
                    }
                    break;
                case 4:
                    {
                        if (scooterBattery.CurrentCapacity < 1f)
                        {
                            ChargingStations.SetActive(true);
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
                            // Gets triggered too early! Player has not entered the trigger box, but the game believes he has?
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                        break;
                    }
                case 8:
                    {
                        StartCoroutine(TextBlock(tutorialIndex, 10f, 0));
                        tutorialIndex++;
                    }
                    break;
                case 9:
                    {
                        StartCoroutine(TextBlock(tutorialIndex, 10f, 0));
                        tutorialIndex++;
                    }
                    break;
            }
        }
    }

    private void DisplayText(TextMeshProUGUI textField, string textToDisplay)
    {
        textField.text = textToDisplay;
    }

    private IEnumerator TextBlock(int stateIndex, float timeToStart, float intervalTime)
    {
        TutorialPanel.SetActive(true);

        yield return new WaitForSeconds(timeToStart); // Set to zero, if player says nothing (empty string)

        DisplayText(DiegeticText, tutorialPhrases[stateIndex]);

        yield return new WaitForSeconds(intervalTime); // Set to zero, if player says nothing (empty string)

        if (keyboardUsed)
            DisplayText(InstructionText, tutorialInstructions[stateIndex]);
        else
            DisplayText(InstructionText, tutorialInstructionsGamePad[stateIndex]);

        yield return new WaitForSeconds(3.5f);

        DisplayText(DiegeticText, "");

        yield return new WaitForSeconds(intervalTime);

        DisplayText(InstructionText, "");

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
