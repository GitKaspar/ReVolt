using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimatedButton))]
public class ClickedButtonAnimation : MonoBehaviour
{
    public AnimationCurve AnimCurve;
    public float AnimSpeed;
    public Vector3 TargetScale = Vector3.zero;
    public Vector3 MidwayScale;
    public Vector3 StartScale = Vector3.one;

    private Vector3 m_Start;
    private Vector3 m_End;

    private float timePassed;

    public event Action OnFinished;

    private void OnEnable()
    {
        timePassed = 0;
        transform.localScale = StartScale;

        m_Start = StartScale;
        m_End = MidwayScale;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime * AnimSpeed;

        if (timePassed >= 0.5f) 
        {
            m_Start = MidwayScale;
            m_End = TargetScale;
        }

        if (timePassed >= 1f)
        {
            transform.localScale = m_End;
            this.enabled = false;
            OnFinished?.Invoke();
            return;
        }


        float modifier = AnimCurve.Evaluate(timePassed);
        transform.localScale = Vector3.LerpUnclamped(m_Start, m_End, modifier);
    }
}
