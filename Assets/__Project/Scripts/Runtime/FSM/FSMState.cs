using UnityEngine;

public abstract class FSMState
{
    public FSMCtrl Ctrl { get; private set; }

    public FSMState(FSMCtrl ctrl)
    {
        Ctrl = ctrl;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }

    public virtual void OnLateUpdate()
    {

    }
}
