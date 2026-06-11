using System;

[Serializable]
public struct FSMTransition
{
    public FSMDecisionSO decision;
    public FSMStateSO trueState;
    public FSMStateSO falseState;
}