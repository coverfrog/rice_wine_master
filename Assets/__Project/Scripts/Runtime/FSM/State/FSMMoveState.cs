using UnityEngine;

public class FSMMoveState : FSMState
{
    public FSMMoveState(FSMCtrl ctrl) : base(ctrl)
    {
    }

    public override void OnUpdate()
    {
        Ctrl.transform.position +=
            Time.deltaTime *
            Ctrl.Status[StatType.MoveSpeed] *
            Ctrl.InputContext.MoveDirection;
    }
}
