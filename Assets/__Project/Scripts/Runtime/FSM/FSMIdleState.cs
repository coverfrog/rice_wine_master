using UnityEngine;

public class FSMIdleState : FSMState
{
    public override void OnEnter(FSMCtrl ctrl)
    {
        base.OnEnter(ctrl);

        Debug.Log("Idle");
    }
}
