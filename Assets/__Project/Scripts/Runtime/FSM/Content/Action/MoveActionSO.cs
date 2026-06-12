using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "RWM/FSM/Actions/Move")]
public class MoveActionSO : FSMActionSO
{
    public override void Execute(FSMCtrl ctrl)
    {
        Vector3 inputDir = ctrl.GetInputDirection();
        if (inputDir.sqrMagnitude < 0.001f)
            return;

        ctrl.Move(inputDir);
    }
}
