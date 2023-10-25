using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateDrone
{

    /*
     * Millised seisundid peavad sellel droonil olema?
     * Idle? Jah. Passib. See on vaikimisi.
     * Patrull? Jah. Mida muuta vaja? Animatsioon vist vaja maha v�tta - lihtsalt liigub.
     * J�litamine? Jah. Mida muudab? Samuti animatsioon maha. Liigub lihtsalt kiiremini. K�ll aga oleks vaja drooni tulukese v�rvi muuta, kui j�litab.
     * R�ndamine? V�ib-olla. Ehk piisab, kui droon k�llalt l�hedale j�uab? Mida muudab? Animatsioon maha. Peab tulistama? Tulistab l�hemalt? Heli ka veel ei vaja.
     * Magamine? Saab panna magama, aga pole implementeeritud.
     * P�genemine? V�ib-olla
     */
    public enum STATE { 
        IDLE, PATROL, PURSUE, ATTACK, SLEEP, RUNAWAY
    };

    public enum EVENT { 
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage; // Etapp
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected StateDrone nextState; // V�ib lisada ka j�rgmise seisundi 
    protected NavMeshAgent agent;

    // Kui m�ngija on 10 �hiku kaugusel ja 30 kraadi ulatuses, siis teda n�hakse. Kui m�ngija on 7 �hiku kaugusel, siis npc tulistab.

    float visDist = 10.0f;
    float visAngle = 45.0f; // NPC n�eb tegelikult kaks korda sama palju
    float shootDist = 3.0f;
    float fleeDist = 2.0f;
  

        public StateDrone(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
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

    public StateDrone Process()
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

public class Idle : StateDrone
{


    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) 
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        else if (Random.Range(0, 100) < 10) // See, et t��p hakkab patrullima, on juhuslik, 10% t�en�osus
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Patrol : StateDrone
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
        
    }

    public override void Enter()
    {
        // Esimese versiooni tehisaru ei patrulli. L�litan ajautiselt patrullimiskoodi v�lja - ei t��ta ka hetkel.
        /*
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
        // anim.SetTrigger("isWalking");
        */
        base.Enter();
    }   
    public override void Update()
    {
        /* Ka v�lja l�litatud, kuna esialgu droon vaid seisab.
        if(agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex = currentIndex++;

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }
        */
        if (agent.transform.position != AIDrone.Checkpoint.position)
        {
            agent.SetDestination(AIDrone.Checkpoint.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }  
    public override void Exit()
    {
        // anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue : StateDrone
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if(agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (IsScared())
            {
                nextState = new RunAway(npc, agent, anim, player);
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

public class Attack : StateDrone
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)

    {
        name = STATE.ATTACK;
        shoot = npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        // anim.SetTrigger("isShooting");
        agent.isStopped = true;
        // shoot.Play();
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
            nextState = new Idle(npc, agent, anim, player); // Seda saab siin muuta, et anda talle teine k�itumine. Idle on turvaline, kuna viib k�igi teisteni.
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isShooting");
        // shoot.Stop();
        base.Exit();
    }
}

public class RunAway : StateDrone

{
    GameObject safeBox; // Oleks s��stlikum panna see GameEnvironment klassi ja kutsuda sealt.
    float rotationSpeed = 2.0f;

    public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
       : base(_npc, _agent, _anim, _player)

    {
        name = STATE.RUNAWAY;
        safeBox = GameObject.FindGameObjectWithTag("Safe");
      
    }

    public override void Enter()
    {
        // anim.SetTrigger("isRunning");
        agent.isStopped = false;
        agent.speed = 6;
        agent.SetDestination(safeBox.transform.position); // Asukoha m��rab juba siin - mitte Update meetodis
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        // anim.ResetTrigger("isRunning");
        base.Exit();
    }

}