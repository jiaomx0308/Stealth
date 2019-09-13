using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>
{
    public virtual void Enter(T e)
    {

    }
    public virtual void Update(T e)
    {

    }

    public virtual void Exit(T e)
    {

    }
}


public class StateMachine<TOwner> : MonoBehaviour
{
    private State<TOwner> curstate = null;
    private TOwner owner;

    public void Init(TOwner owner, State<TOwner> state)
    {
        this.owner = owner;
        ChangeState(state);
    }

    public void ChangeState(State<TOwner> state)
    {
        if (curstate != null)
        {
            curstate.Exit(this.owner);
        }
        curstate = state;
        curstate.Enter(this.owner);
    }

    // Update is called once per frame
    void Update()
    {
        curstate.Update(owner);
    }
}
