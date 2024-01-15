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

    public GameObject ClearBox;
    private TriggerBox triggerBoxComponent6;

   private string[] tutorialPhrases =
    {
        "Scooter's ready. Time to see, what this baby can do!",
        "",
        "Whoa. Easy there!",
        "Damn! Out of battery.",
        "Much better.",
        "This looks like a dead end.",
        "One of the drops is near.",
        "Careful now! Police drone ahead!",
        "Phew. Close call. One more drop tonight."
    };
   private string[] tutorialInstructions =
    {
        "Mouse wheel up/right trigger increase scooter speed. Use W/left stick up to accelerate.",
        "A and D/left stick are used for steering.",
        "Mouse wheel down/left trigger decrease scooter speed. Press space/left bumper to break.", // Kind of convoluted
        "Look for the blue charging stations to charge scooter battery.",
        "Remember to keep the battery charged or scooter speed will be severely limited.",
        "Use S/left stick down to move backwards. Alrernatively, you can jump low obstacles with Shift/Y button.",
        "Distance to nearest drop is displayed in the top left. Go towards the purple drop location and press the corresponding key.",
        "Avoid being seen by its scanner light to not get detected.", 
        "Drones will chase you down and impound your scooter, resulting in GAME OVER. Finish the last drop to complete the level."
    };

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

        triggerBoxComponent6 = ClearBox.GetComponent<TriggerBox>();
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
                        if (triggerBoxComponent1.HasEntered) 
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
                        if (triggerBoxComponent2.HasEntered || triggerBoxComponent3.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 7: {
                        if (triggerBoxComponent4.HasEntered || triggerBoxComponent5.HasEntered)
                        {
                            StartCoroutine(TextBlock(tutorialIndex, 0, 1.5f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 8:
                    {
                        if (triggerBoxComponent6.HasEntered)
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
