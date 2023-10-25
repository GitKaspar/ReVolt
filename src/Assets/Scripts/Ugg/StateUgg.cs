using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateUgg
{
    public enum STATE { 
        IDLE_U, PATROL_U, PURSUE_U, ATTACK_U, SLEEP_U, RUNAWAY_U
    };

    public enum EVENT { 
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage; // Etapp
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected StateUgg nextState; // Võib lisada ka järgmise seisundi 
    protected NavMeshAgent agent;

    // Kui mängija on 10 ühiku kaugusel ja 30 kraadi ulatuses, siis teda nähakse. Kui mängija on 7 ühiku kaugusel, siis npc tulistab.

    float visDist = 10.0f;
    float visAngle = 30.0f; // NPC näeb tegelikult kaks korda sama palju
    float shootDist = 7.0f;
    float fleeDist = 2.0f;

    public StateUgg(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update () { stage = EVENT.UPDATE; }
    public virtual void Exit () { stage = EVENT.EXIT; }

    public StateUgg Process()
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
        Vector3 direction = npc.transform.position - player.position; // Siin muutus.
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude < fleeDist  && angle < visAngle)
        {
            return true;
        }
        return false;
    }
}

public class Idle_U : StateUgg
{ 
    public Idle_U(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) 
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE_U;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        if(CanSeePlayer())
        {
            nextState = new Pursue_U(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
      else if (Random.Range(0, 100) < 10) // See, et tüüp hakkab patrullima, on juhuslik, 10% tõenäosus
        {
            nextState = new Patrol_U(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Patrol_U : StateUgg
{
    int currentIndex = -1;

    public Patrol_U(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL_U;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);

            if(distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }
        anim.SetTrigger("isWalking");
        base.Enter();
    }   
    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex = currentIndex++;

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue_U(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }  
    public override void Exit()
    {

        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue_U : StateUgg
{
    public Pursue_U(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE_U;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if(agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack_U(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol_U(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (IsScared())
            {
                nextState = new RunAway_U(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack_U : StateUgg
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack_U(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)

    {
        name = STATE.ATTACK_U;
        shoot = npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        shoot.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction,npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Idle_U(npc, agent, anim, player); // Seda saab siin muuta, et anda talle teine käitumine. Idle on turvaline, kuna viib kõigi teisteni.
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }
}

public class RunAway_U : StateUgg

{
    GameObject safeBox; // Oleks säästlikum panna see GameEnvironment klassi ja kutsuda sealt.
    float rotationSpeed = 2.0f;

    public RunAway_U(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
       : base(_npc, _agent, _anim, _player)

    {
        name = STATE.RUNAWAY_U;
        safeBox = GameObject.FindGameObjectWithTag("Safe");
      
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        agent.isStopped = false;
        agent.speed = 6;
        agent.SetDestination(safeBox.transform.position); // Asukoha määrab juba siin - mitte Update meetodis
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            nextState = new Idle_U(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }

}