using UnityEngine;

public abstract class FSMDecisionSO : ScriptableObject
{
    public abstract bool Decide(FSMCtrl ctrl);
}
