using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public List<Checkpoint> Next;

    public Checkpoint GetNextCheckpoint()
    {
        if (Next.Count == 0) return null;
        return Next[Random.Range(0, Next.Count)];
    }
}
