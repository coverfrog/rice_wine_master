using UnityEngine;

public class FSMMoveState : FSMState
{
    public override void OnEnter(FSMCtrl ctrl)
    {
        base.OnEnter(ctrl);

        Debug.Log("Move");
    }
}
