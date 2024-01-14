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



   private string[] tutorialPhrases =
    {
        "Scooter's ready. Time to see, what this baby can do!",
        "",
        "Whoa. Easy there!", // Lower battery (not drained), higher speed
        "Damn! Out of battery.",
        "Much better.",
        "This looks like a dead end.", // When to trigger? Index 4 and a collision space?
        "One of the drops is near.",
        "Careful now! It's that damn police robot!", // Epic line, grandpa!
        "Out of battery. Again...",
        "Whew. Close one."
    };
   private string[] tutorialInstructions =
    {
        "Mouse wheel up/right trigger increase scooter speed. Use W/left stick up to accelerate.",
        "A and D/left stick are used for steering.", // When to trigger? Player has some speed, but hasn't run out of battery yet.
        "Mouse wheel down/left trigger decrease scooter speed. Press space/left bumper to break.", // Kind of convoluted
        "Look for the blue charging stations to charge scooter battery.",
        "Remember to keep the battery charged or scooter speed will be severely limited.",
        "Use S/left stick down to move backwards. Alrernatively, you can jump low obstacles with Shift/Y button.",
        "Distance to nearest drop is displayed in the top left. Go towards the purple drop location and press the corresponding key.",
        "Gotta avoid its scanner light.", 
        "Where art thou, my charging station?",
        "We'll get them soon enough. Gotta make my drop."
    };

    // Start is called before the first frame update
    void Start()
    {
        tutorialIndex = 0;
        scooterBattery = Player.GetComponent<Battery>();

        jumpComponent = Player.GetComponent<Jump>();
        triggerBoxComponent1 = DeadEndBox.GetComponent<TriggerBox>();

        triggerBoxComponent2 = DropBox1.GetComponent<TriggerBox>();
        triggerBoxComponent3 = DropBox2.GetComponent<TriggerBox>();
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
                        jumpComponent.forwardForce = 0;
                        jumpComponent.upForce = 0;
                        StartCoroutine(TextBlock(tutorialIndex, 2f, 4f));
                        tutorialIndex++;
                    }
                    break;
                case 1:
                    {
                        if (scooterBattery.CurrentCapacity < 70)
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
                            Debug.Log("Box reports positive contact");
                            jumpComponent.upForce = 6f;
                            jumpComponent.forwardForce = 2f;
                            StartCoroutine(TextBlock(tutorialIndex, 0, 2f));
                            tutorialIndex++;
                        }
                    }
                    break;
                    case 6: 
                    {
                        if (triggerBoxComponent2.HasEntered || triggerBoxComponent3.HasEntered)
                        {

                        }
                    }
                    break;
            }
        } 



        /*

        if (scooterBattery.CurrentCapacity < 1f && !outOfBattery)
        {
            if (!again)
            {
                outOfBattery = true;
                again = true;
                StartCoroutine(TextBlock(1));
            } else
            {
                outOfBattery = true;
                StartCoroutine(TextBlock(4));
            }
        }

        if (scooterBattery.CurrentCapacity >= 80f && outOfBattery)
        {
            outOfBattery = false;
            if (!again)
            {
                StartCoroutine(TextBlock(2 ));
            }
        }
        if (Player.GetComponent<Rigidbody>().velocity.magnitude >= 18f)
        { DisplayText(TutorialPanel, DiegeticText, "Whee!");
            DisplayText(TutorialPanel, InstructionText, "");
        }
        if (Player.transform.position.x < 430f && Player.transform.position.z > 420 && !alerted)
        { 
            alerted = true;    
            StartCoroutine(TextBlock(3, ));
        }
        if (Player.transform.position.x < 370f && Player.transform.position.z < 410 && !passed && alerted)
        { 
            passed = true;
            StartCoroutine(TextBlock(5));
        }
        */
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
