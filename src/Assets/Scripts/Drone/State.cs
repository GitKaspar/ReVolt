using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class State
{
    public enum STATE
    {
        IDLE,
        PATROL,
        PURSUE,
        ATTACK,
        SLEEP,
        RUNAWAY,
        DISABLED
    };

    public enum EVENT
    {
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
    protected float speedModifier;

    float visDist = 25.0f;
    float visAngle = 60.0f; // NPC n‰eb tegelikult kaks korda sama palju
    float shootDist = 1.0f;


    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        droneLight = _droneLight;
        audioSource = _audioSource;
        speedModifier = _speedModifier; 
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

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

        if (direction.magnitude < visDist && angle < visAngle) // magnitude annab meile Vector3'e pikkuse
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
        if (direction.magnitude < 1.5f && angle < visAngle)
        {
            return true;
        }
        return false;
    } 
    
    public bool CanDisable()
    {
        Vector3 direction = npc.transform.position - player.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude < 5.0f && angle < visAngle)
        {
            return true;
        }
        return false;
    }
}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource, _speedModifier)
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
            nextState = new Pursue(npc, agent, anim, player, droneLight, audioSource, speedModifier);
            stage = EVENT.EXIT;
        }

        else if (UnityEngine.Random.Range(0, 100) < 10) // See, et t¸¸p hakkab patrullima, on juhuslik, 10% tıen‰osus
        {
            nextState = new Patrol(npc, agent, anim, player, droneLight, audioSource, speedModifier);
            stage = EVENT.EXIT;
        }
        else if (CanDisable())
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                nextState = new Disabled(npc, agent, anim, player, droneLight, audioSource, speedModifier);
                stage = EVENT.EXIT;
            }
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

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource, _speedModifier)
    {
        name = STATE.PATROL;
        agent.speed = 6.0f * speedModifier;
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
        if (checkpoint == null)
        {
            Debug.Log("Ja ongi null");
            return;
        }
        else
        {
            if (agent.remainingDistance < 1)
            {
                checkpoint = checkpoint.GetNextCheckpoint();
            }
            agent.SetDestination(checkpoint.transform.position);
        }


        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player, droneLight, audioSource, speedModifier);
            stage = EVENT.EXIT;
        }
        else if (IsScared())
        {
            nextState = new RunAway(npc, agent, anim, player, droneLight, audioSource, speedModifier);
            stage = EVENT.EXIT;
        }
        else if (CanDisable()) {
            if (Input.GetKeyUp(KeyCode.E)) {
                nextState = new Disabled(npc, agent, anim, player, droneLight, audioSource, speedModifier);
                stage = EVENT.EXIT;
            }
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
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource, _speedModifier)
    {
        name = STATE.PURSUE;
        agent.speed = 12.0f * speedModifier;
        agent.angularSpeed = agent.angularSpeed * 180;
        agent.acceleration = agent.acceleration * 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        droneLight.color = Color.red;
        audioSource.clip = SoundController.SoundInstance.Alarm1;
        audioSource.Play();
        base.Enter();
        Debug.Log(agent.angularSpeed);
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player, droneLight, audioSource, speedModifier);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, anim, player, droneLight, audioSource, speedModifier);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        agent.acceleration = agent.acceleration * 0.2f;
        agent.angularSpeed = agent.angularSpeed / 180;
        // anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 4.0f;
    // AudioSource shoot;

    public static event Action catchPlayer;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource, _speedModifier)

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
        audioSource.ignoreListenerPause = true;
        audioSource.clip = SoundController.SoundInstance.Lasers.Clips[UnityEngine.Random.Range(0, SoundController.SoundInstance.Lasers.Clips.Count)];
        audioSource.Play();
        // shoot.Play();
        base.Enter();

        Events.EndGame(false);
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0.0f;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player, droneLight, audioSource, speedModifier); // Seda saab siin muuta, et anda talle teine k‰itumine. Idle on turvaline, kuna viib kıigi teisteni.
            // shoot.Stop();
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isShooting");
        audioSource.ignoreListenerPause = false;
        base.Exit();
    }
}

public class RunAway : State

{
    GameObject safeBox;

    public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource, _speedModifier)

    {
        name = STATE.RUNAWAY;
        safeBox = GameObject.FindGameObjectWithTag("Safe");

    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        agent.isStopped = false;
        droneLight.color = Color.yellow;
        agent.speed = 15 * speedModifier;
        audioSource.loop = true;
        audioSource.clip = SoundController.SoundInstance.Alarm1;
        audioSource.Play();
        agent.SetDestination(safeBox.transform.position); // Asukoha m‰‰rab juba siin - mitte Update meetodis
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1.0f)
        {
            nextState = new Idle(npc, agent, anim, player, droneLight, audioSource, speedModifier);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Disabled : State

{

    public Disabled(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Light _droneLight, AudioSource _audioSource, float _speedModifier)
        : base(_npc, _agent, _anim, _player, _droneLight, _audioSource, _speedModifier)

    {
        name = STATE.DISABLED;
    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        anim.SetTrigger("isDestroyed");
        agent.isStopped = true;
        droneLight.enabled = false; ;
        agent.speed = 0;
        audioSource.clip = SoundController.SoundInstance.DroneShutDown;
        audioSource.loop = false;
        audioSource.Play();
 // Asukoha m‰‰rab juba siin - mitte Update meetodis
        base.Enter();
    }

    public override void Update()
    {
        /*if (agent.remainingDistance < 1.0f)
        {
            nextState = new Idle(npc, agent, anim, player, droneLight, audioSource, speedModifier);
            
        }*/
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isRunning");
        base.Exit();
    }
}