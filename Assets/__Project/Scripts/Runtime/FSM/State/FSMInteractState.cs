using Mirror;
using UnityEngine;

public class FSMInteractState : FSMState
{
    public FSMInteractState(FSMCtrl ctrl) : base(ctrl)
    {

    }

    public override void OnEnter()
    {
        uint interactID = Ctrl.InputContext.InteractID;

        if (NetworkClient.spawned.TryGetValue(interactID, out NetworkIdentity identity) == false)
        {
            Cancel();
            return;
        }

        if (identity.TryGetComponent(out InteractObject io) == false)
        {
            Cancel();
            return;
        }

        io.Interact();
    }

    private void Cancel()
    {
        FSMInputContext context = Ctrl.InputContext;
        context.InteractID = 0;

        Ctrl.InputContext = context;
    }
}
