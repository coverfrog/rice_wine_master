using UnityEngine;

public class FSMMoveState : FSMState
{
    public FSMMoveState(FSMCtrl ctrl) : base(ctrl)
    {
    }

    public override void OnFixedUpdate()
    {
        Vector3 position = Ctrl.Rb3d.position +
                        Time.deltaTime *
                        Ctrl.Status[StatType.MoveSpeed] *
                        Ctrl.InputContext.MoveDirection;

        Ctrl.Rb3d.MovePosition(position);
    }
}
