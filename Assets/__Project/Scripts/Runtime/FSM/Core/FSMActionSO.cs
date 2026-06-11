using UnityEngine;

public abstract class FSMActionSO : ScriptableObject
{
    public abstract void Execute(FSMCtrl ctrl);

}
