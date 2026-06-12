using UnityEngine;

public class FSMMoveState : FSMState
{
    public override void OnUpdate(FSMCtrl ctrl)
    {
        ctrl.MoveUpdate();
    }
}
