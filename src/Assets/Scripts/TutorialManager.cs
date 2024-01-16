using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
   public GameObject TutorialPanel;
   public TextMeshProUGUI DiegeticText;
   public TextMeshProUGUI InstructionText;
   public GameObject Player;
    private Jump jumpComponent;
    private Battery scooterBattery;
   private int tutorialIndex;

    public GameObject DeadEndBox1;
    private TriggerBox triggerBoxComponent1;

    public GameObject DeadEndBox2;
    private TriggerBox triggerBoxComponent2;


    public GameObject DropBox1;
    private TriggerBox triggerBoxComponent3;

    public GameObject DropBox2;
    private TriggerBox triggerBoxComponent4;
    
    public GameObject DroneBox1;
    private TriggerBox triggerBoxComponent5;

    public GameObject DroneBox2;
    private TriggerBox triggerBoxComponent6;

    public GameObject ClearBox1;
    private TriggerBox triggerBoxComponent7;

    public GameObject ClearBox2;
    private TriggerBox triggerBoxComponent8;


    private string[] tutorialPhrases =
    {
        "Scooter's ready. Time to see, what this baby can do!",
        "",
        "Whoa. Easy there!",
        "Damn! Out of battery!",
        "Much better.",
        "This looks like a dead end.",
        "One of the drops is near.",
        "Careful now! Police drone ahead!",
        "Phew. Close call. One more drop tonight."
    };
   private string[] tutorialInstructions =
    {
        "Mouse wheel up/RT increase scooter speed. Use W/left stick to move forward.",
        "A & D/left stick are used for steering.",
        "Mouse wheel down/LT decrease scooter speed. Press space/LB to break.", // Kind of convoluted
        "Look for the blue charging stations to charge scooter battery.",
        "Remember to keep the battery charged or scooter speed will be severely limited.",
        "Use S/left stick down to move backwards. Alrernatively, you can jump low obstacles with Shift/Y.",
        "When near a drop, the drop indicator bar at the top left will fill up in relation to how close you are. Go towards the pink drop location.",
        "Stay out of the drone's light cone to avoid detection. You can also sneak up on drones to disable them, but it's a high risk action.", 
        "Drones will chase you down when they see you and imprison you, resulting in GAME OVER. Finish the last drop to complete the level."
    };

    // Start is called before the first frame update
    void Start()
    {
        tutorialIndex = 0;
        scooterBattery = Player.GetComponent<Battery>();

        //jumpComponent = Player.GetComponent<Jump>();

        triggerBoxComponent1 = DeadEndBox1.GetComponent<TriggerBox>();
        triggerBoxComponent2 = DeadEndBox1.GetComponent<TriggerBox>();

        triggerBoxComponent3 = DropBox1.GetComponent<TriggerBox>();
        triggerBoxComponent4 = DropBox2.GetComponent<TriggerBox>();
        
        triggerBoxComponent5 = DroneBox1.GetComponent<TriggerBox>();
        triggerBoxComponent6 = DroneBox2.GetComponent<TriggerBox>();

        triggerBoxComponent7 = ClearBox1.GetComponent<TriggerBox>();
        triggerBoxComponent8 = ClearBox2.GetComponent<TriggerBox>();

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
                        StartCoroutine(TextBlock(tutorialIndex, 2f, 4f));
                        tutorialIndex++;
                    }
                    break;
                case 1:
                    {
                        if (scooterBattery.CurrentCapacity < 75)
                        {
                            StartCoroutine (TextBlock(tutorialIndex, 0, 0));
                            tutorialIndex++;
                        }
                    }
                    break;
                case 2:
                    {
                        if(scooterBattery.CurrentCapacity < 30 && Player.GetComponent<Rigidbody>().velocity.magnitude > 15f)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 3:
                    {
                        if(scooterBattery.CurrentCapacity < 1f)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 4:
                    {
                        if(scooterBattery.CurrentCapacity > 80)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    } 
                    break;
                    case 5: 
                    {
                        if (triggerBoxComponent1.HasEntered || triggerBoxComponent2.HasEntered) 
                        {
                            //jumpComponent.upForce = 6f;
                            //jumpComponent.forwardForce = 2f;
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 6: 
                    {
                        if (triggerBoxComponent3.HasEntered || triggerBoxComponent4.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 7: {
                        if (triggerBoxComponent5.HasEntered || triggerBoxComponent6.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 1.5f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 8:
                    {
                        if (triggerBoxComponent7.HasEntered || triggerBoxComponent8.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 1.5f));
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

        DisplayText(TutorialPanel, InstructionText, tutorialInstructions[stateIndex]);

        yield return new WaitForSeconds(4);

        DisplayText(TutorialPanel, DiegeticText, "");

        yield return new WaitForSeconds(intervalTime);

        DisplayText(TutorialPanel, InstructionText, "");

        TutorialPanel.SetActive(false);
    }
}
