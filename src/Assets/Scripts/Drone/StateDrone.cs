using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateDrone
{

    /*
     * Millised seisundid peavad sellel droonil olema?
     * Idle? Jah. Passib. See on vaikimisi.
     * Patrull? Jah. Mida muuta vaja? Animatsioon vist vaja maha võtta - lihtsalt liigub.
     * Jälitamine? Jah. Mida muudab? Samuti animatsioon maha. Liigub lihtsalt kiiremini. Küll aga oleks vaja drooni tulukese värvi muuta, kui jälitab.
     * Ründamine? Võib-olla. Ehk piisab, kui droon küllalt lähedale jõuab? Mida muudab? Animatsioon maha. Peab tulistama? Tulistab lähemalt? Heli ka veel ei vaja.
     * Magamine? Saab panna magama, aga pole implementeeritud.
     * Põgenemine? Võib-olla
     */
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
    protected EVENT stage; // Etapp
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected StateDrone nextState; // Võib lisada ka järgmise seisundi 
    protected NavMeshAgent agent;

    // Kui mängija on 10 ühiku kaugusel ja 30 kraadi ulatuses, siis teda nähakse. Kui mängija on 7 ühiku kaugusel, siis npc tulistab.

    float visDist = 20.0f;
    float visAngle = 60.0f; // NPC näeb tegelikult kaks korda sama palju
    float shootDist = 3.0f;

        public StateDrone(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        stage = EVENT.ENTER;
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
        if (direction.magnitude < 2.0f  && angle < visAngle)
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

        else if (Random.Range(0, 100) < 10) // See, et tüüp hakkab patrullima, on juhuslik, 10% tõenäosus Tõstame 50 peale?
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
        agent.speed = 4.0f;
        agent.isStopped = false;
        
    }

    public override void Enter()
    {
       /*
        float lastDist = Mathf.Infinity;

        
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i]; // antud checkpoint'i GameObject
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position); // vahemaa antud checkpoint'i ja NPC vahel
            // Kui antud checkpoint on lähemal kui eelmine lähim (alguses lõpmatus), siis 
            if(distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }*/
        // anim.SetTrigger("isWalking");
        currentIndex = 0;
        base.Enter();
    }   
    public override void Update()
    {
        // Siin peaks vahetuma sihtkoht. Kui oleme sihile lähemal kui 1 ühik, siis kui meil on list läbi käidud, alustame uuesti, kui ei, kasvatame indeksit.
        // IF remainingDistance, is unknown, the value is infinity (greater than 1) - remainingDisntace is not unknown.
        if(agent.remainingDistance < 1)
        {
            Debug.Log("Checkpoint reached."); // This is triggered even before we reach the first checkpoint...
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
                Debug.Log("Index reset to 0."); // This is never triggered.
            }
            else
            {
                currentIndex = currentIndex++;
                Debug.Log("Raised index by 1. Current index: " + currentIndex); // Index remains 0 throughout. Problem? Global value resets index to -1? Works in FSM project, though.
            }
            // See siin peaks olema uus siht.
            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
            Debug.Log("Off to next checkpoint.");

            // Amount of checkpoints is correct(size of list = 5)

        }


        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (IsScared())
        {
            nextState = new RunAway(npc, agent, anim, player);
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
        agent.speed = 8.0f;
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
    // AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)

    {
        name = STATE.ATTACK;
        // shoot = npc.GetComponent<AudioSource>();
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
        direction.y = 0.0f;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player); // Seda saab siin muuta, et anda talle teine käitumine. Idle on turvaline, kuna viib kõigi teisteni.
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

public class RunAway : StateDrone

{
    GameObject safeBox; // Oleks säästlikum panna see GameEnvironment klassi ja kutsuda sealt.

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
        agent.SetDestination(safeBox.transform.position); // Asukoha määrab juba siin - mitte Update meetodis
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1.0f)
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