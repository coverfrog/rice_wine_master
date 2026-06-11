using UnityEngine;

[CreateAssetMenu(fileName = "AttackInputDecision", menuName = "RWM/FSM/Decisions/AttackInput")]
public class AttackInputDecisionSO : FSMDecisionSO
{
    public override bool Decide(FSMCtrl ctrl)
    {
        return Input.GetButtonDown("Fire1");
    }
}
