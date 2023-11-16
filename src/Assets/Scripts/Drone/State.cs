using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class State
{
    public enum STATE { 
        IDLE, 
        PATROL, 
        PURSUE, 
        ATTACK, 
        SLEEP, 
        RUNAWAY
    };

    public enum EVENT { 
        ENTER, 
        UPDATE, 
        EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;
    protected Light droneLight;
    protected AudioSource audioSource;

    float visDist = 20.0f;
    float visAngle = 60.0f; // NPC n‰eb tegelikult kaks korda sama palju
    float shootDist = 3.0f;


    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        droneLight = _droneLight;
        audioSource = _audioSource;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update () { stage = EVENT.UPDATE; }
    public virtual void Exit () { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;   
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        
        if(direction.magnitude < visDist && angle < visAngle) // magnitude annab meile Vector3'e pikkuse
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }

    public bool IsScared()
    {
        Vector3 direction = npc.transform.position - player.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude < 2.0f  && angle < visAngle)
        {
            return true;
        }
        return false;
    }
}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource) 
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        droneLight.color = Color.cyan;
        audioSource.loop = true;
        audioSource.clip = SoundController.SoundInstance.Scan1;
        audioSource.Play();
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player, droneLight, audioSource);
            stage = EVENT.EXIT;
        }

        else if (UnityEngine.Random.Range(0, 100) < 10) // See, et t¸¸p hakkab patrullima, on juhuslik, 10% tıen‰osus
        {
            nextState = new Patrol(npc, agent, anim, player, droneLight, audioSource);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Patrol : State
{
    private AI droneAI;
    private Checkpoint checkpoint;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource)
    {
        name = STATE.PATROL;
        agent.speed = 8.0f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        droneAI = agent.GetComponent<AI>();
        checkpoint = droneAI.InitialCheckpoint;
        droneLight.color = Color.cyan;
        audioSource.loop = true;
        audioSource.clip = SoundController.SoundInstance.Scan1;
        audioSource.Play();
        base.Enter();
    }
    public override void Update()
    {
        if (checkpoint == null) {
            Debug.Log("Ja ongi null.");
            return;
        }
        if (agent.remainingDistance < 1) 
        {
            checkpoint = checkpoint.GetNextCheckpoint();
        }
        
        agent.SetDestination(checkpoint.transform.position);

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player, droneLight, audioSource);
            stage = EVENT.EXIT;
        }
        else if (IsScared())
        {
            nextState = new RunAway(npc, agent, anim, player, droneLight, audioSource);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        // anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource)
    {
        name = STATE.PURSUE;
        agent.speed = 15.0f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        droneLight.color = Color.red;
        audioSource.clip = SoundController.SoundInstance.Alarm1;
        audioSource.Play();
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if(agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player, droneLight, audioSource);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, anim, player, droneLight, audioSource);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    // AudioSource shoot;

    public static event Action catchPlayer;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource)

    {
        name = STATE.ATTACK;
        // shoot = npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        // anim.SetTrigger("isShooting");
        agent.isStopped = true;
        catchPlayer?.Invoke(); // Action is invoked, when we enter the Attack state.
        audioSource.loop = false;
        audioSource.clip = SoundController.SoundInstance.Lasers.Clips[UnityEngine.Random.Range(0, SoundController.SoundInstance.Lasers.Clips.Count)];
        audioSource.Play();
        // shoot.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction,npc.transform.forward);
        direction.y = 0.0f;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player, droneLight, audioSource); // Seda saab siin muuta, et anda talle teine k‰itumine. Idle on turvaline, kuna viib kıigi teisteni.
            // shoot.Stop();
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isShooting");
        base.Exit();
    }
}

public class RunAway : State

{
    GameObject safeBox;

    public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource)

    {
        name = STATE.RUNAWAY;
        safeBox = GameObject.FindGameObjectWithTag("Safe");
      
    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        agent.isStopped = false;
        droneLight.color = Color.yellow;
        agent.speed = 15;
        audioSource.loop = true;
        audioSource.clip = SoundController.SoundInstance.Alarm1;
        audioSource.Play();
        agent.SetDestination(safeBox.transform.position); // Asukoha m‰‰rab juba siin - mitte Update meetodis
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1.0f)
        {
            nextState = new Idle(npc, agent, anim, player, droneLight, audioSource);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isRunning");
        base.Exit();
    }

}

