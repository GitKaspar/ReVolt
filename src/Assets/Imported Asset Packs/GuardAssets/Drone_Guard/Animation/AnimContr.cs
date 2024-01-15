using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimContr : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /* Disabled so people couldn't access this.
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            anim.Play("ShutDown");
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            anim.Play("WakeUp");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            anim.Play("Destroyed");
        }
        */
    }
}
