using UnityEngine;

public class FSMInteractState : FSMState
{
    public override void OnEnter(FSMCtrl ctrl)
    {
        Debug.Log("Interact");
    }
}
