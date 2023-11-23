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
    AudioSource audioSource;

    public Transform player;
    public Checkpoint InitialCheckpoint;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        droneLight = this.GetComponentInChildren<Light>();
        audioSource = this.GetComponent<AudioSource>();
        currentState = new Idle(this.gameObject, agent, anim, player, droneLight, audioSource);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}