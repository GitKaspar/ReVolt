using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDrone : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    StateDrone currentState;
    public static Transform Checkpoint;
    public Transform guardPoint;


    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player);
        Checkpoint = guardPoint;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
