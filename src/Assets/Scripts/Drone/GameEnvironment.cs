using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

/*
 * Loome Singleton'i. Klass hõlmab checkpoint'e - kui neid pole, luuakse need stseenist vastavat silti otsides ("Checkpoint")
 */
public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }

    public static GameEnvironment Singleton
    {
        get {
           //if (instance == null)
           //{
                //Debug.Log("created new Gameenvironment");
                instance = new GameEnvironment();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList(); // Seab tähestikulisse kasvavasse järjekorda
            //}
            //Debug.Log("still here bro");
            Debug.Log(instance.checkpoints);
            return instance;
        }
    }
}
