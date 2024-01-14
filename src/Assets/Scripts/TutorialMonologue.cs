using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialMonologue : MonoBehaviour
{
   public GameObject TutorialPanel;
   public TextMeshProUGUI DiegeticText;
   public TextMeshProUGUI InstructionText;
   public GameObject Player;
   private Battery scooterBattery;
   private int tutorialIndex;

   private string[] tutorialPhrases =
    {
        "Scooter's ready. Time to see, what this baby can do!",
        "",
        "Whoa. Easy there!", // Lower battery (not drained), higher speed
        "Damn! Out of battery.",
        "Much better.",
        "Careful now! It's that damn police robot!", // Epic line, grandpa!
        "Out of battery. Again...",
        "Whew. Close one."
    };
   private string[] tutorialInstructions =
    {
        "Mouse wheel up/right trigger increase scooter speed. Use W/left stick up to accelerate.",
        "A and D/left stick are used for steering.", // When to trigger? Player has some speed, but hasn't run out of battery yet.
        "Mouse wheel down/left trigger decrease scooter speed. S/left stick down lower speed. Space/left bumper.",
        "Might be able to use ye olde blue charging station down the road.",
        "Gotta remember to keep that battery charged.",
        "Gotta avoid its scanner light.", 
        "Where art thou, my charging station?",
        "We'll get them soon enough. Gotta make my drop."
    };

    // Start is called before the first frame update
    void Start()
    {
        tutorialIndex = 0;
        scooterBattery = Player.GetComponent<Battery>();
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
                        StartCoroutine(TextBlock(tutorialIndex, 1f));
                        tutorialIndex++;
                    }
                    break;
                case 1:
                    {
                        if (scooterBattery.CurrentCapacity < 80)
                        {
                            StartCoroutine (TextBlock(tutorialIndex, 0));
                            tutorialIndex++;
                            // Probleem: sõnum jääb pikaks ajaks nähtavale. Vaja see pärast aja möödumist kaotada.
                        }
                    }
                    break;
                case 2:
                    {

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

    private IEnumerator TextBlock(int stateIndex, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        DisplayText(TutorialPanel, DiegeticText, tutorialPhrases[stateIndex]);

        yield return new WaitForSeconds(2);

        DisplayText(TutorialPanel, InstructionText, tutorialInstructions[stateIndex]);

        yield return new WaitForSeconds(4);

        DisplayText(TutorialPanel, DiegeticText, "");
        DisplayText(TutorialPanel, InstructionText, "");

        TutorialPanel.SetActive(false);
    }

    void Confirm()
    {
        TutorialPanel.SetActive(false);
    }

}
