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
    public float SpeedModifier = 1f;
    [Range(0.25f, 1)]
    //[HideInInspector]
    public float StealthModifier;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        droneLight = this.GetComponentInChildren<Light>();
        audioSource = this.GetComponent<AudioSource>();
        currentState = new Idle(this.gameObject, agent, anim, player, droneLight, audioSource, SpeedModifier, StealthModifier);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}