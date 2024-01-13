using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialMonologue : MonoBehaviour
{
   public GameObject TutorialPanel;
   public TextMeshProUGUI TutorialText;
   public TextMeshProUGUI TutorialSubtext;
    public GameObject Player;
   private Battery scooterBattery;
    private bool outOfBattery;
    private bool again;
    private bool alerted;
    private bool passed; 

   private string[] tutorialStates =
    {
        "Scooter should be ready to go now. Time to see, what this baby can do.",
        "Damn! Out of battery.",
        "Much better.",
        "Careful now! It's that damn police robot!", // Epic line, grandpa!
        "Out of battery. Again...",
        "Whew. Close one."
    };
   private string[] tutorialSubtext =
    {
        "Full speed ahead.",
        "Might be able to use ye olde blue charging station down the road.",
        "Gotta remember to keep that battery charged.",
        "Gotta avoid its scanner light.", 
        "Where art thou, my charging station?",
        "We'll get them soon enough. Gotta make my drop."
    };

    // Start is called before the first frame update
    void Start()
    {
        passed = false;
        outOfBattery = false;
        alerted = false;
        again = false;
        StartCoroutine(TextBlock(0));
        scooterBattery = Player.GetComponent<Battery>();
    }

    // Update is called once per frame
    void Update()
    {
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
                StartCoroutine(TextBlock(2));
            }
        }
        if (Player.GetComponent<Rigidbody>().velocity.magnitude >= 18f)
        { DisplayText(TutorialPanel, TutorialSubtext, "Whee!");
            DisplayText(TutorialPanel, TutorialText, "");
        }
        if (Player.transform.position.x < 430f && Player.transform.position.z > 420 && !alerted)
        { 
            alerted = true;    
            StartCoroutine(TextBlock(3));
        }
        if (Player.transform.position.x < 370f && Player.transform.position.z < 410 && !passed && alerted)
        { 
            passed = true;
            StartCoroutine(TextBlock(5));
        }
    }

    private void DisplayText(GameObject TextPanel, TextMeshProUGUI textField, string textToDisplay)
    {
        textField.text = textToDisplay;
        if (TextPanel.activeSelf == false) { 
            TextPanel.SetActive(true);
        }
    }

    private IEnumerator TextBlock(int i)
    {
        yield return new WaitForSeconds(1);

        DisplayText(TutorialPanel, TutorialText, tutorialStates[i]);

        yield return new WaitForSeconds(2);

        DisplayText(TutorialPanel, TutorialSubtext, tutorialSubtext[i]);

        yield return new WaitForSeconds(6);

        DisplayText(TutorialPanel, TutorialSubtext, "");

        TutorialPanel.SetActive(false);
    }

}
