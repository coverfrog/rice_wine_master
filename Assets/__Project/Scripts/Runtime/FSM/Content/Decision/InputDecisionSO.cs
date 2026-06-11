using UnityEngine;

[CreateAssetMenu(fileName = "InputDecision", menuName = "RWM/FSM/Decisions/InputCheck")]
public class InputDecisionSO : FSMDecisionSO
{
    [SerializeField] private bool isMovementInput; // true: 입력이 있을 때 참, false: 입력이 없을 때 참
    
    public override bool Decide(FSMCtrl ctrl)
    {
        bool hasInput = ctrl.GetInputDirection().magnitude > 0.1f;
        return hasInput == isMovementInput;
    }
}
