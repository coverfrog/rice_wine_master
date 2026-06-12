using UnityEngine;

public class FSMMoveState : FSMState
{
    public override void OnUpdate(FSMCtrl ctrl)
    {
        ctrl.transform.position +=
            Time.deltaTime *
            ctrl.Status[StatType.MoveSpeed] *
            ctrl.InputContext.MoveDirection;
    }
}
