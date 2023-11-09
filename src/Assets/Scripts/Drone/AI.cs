using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    State currentState;
    Light droneLight;
    
    public Transform player;
    public Checkpoint InitialCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        droneLight = this.GetComponentInChildren<Light>();
        currentState = new Idle(this.gameObject, agent, anim, player, droneLight);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
