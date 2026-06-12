using UnityEngine;
using Mirror;

public class FSMCtrl : NetworkBehaviour
{
    public FSMGroup FsmGroup { get; private set; }

    public override void OnStartServer()
    {
        base.OnStartServer();

        Setup();
    }

    protected virtual void Update()
    {
        UpdateFSM();
    }

    protected virtual void FixedUpdate()
    {
        FixedUpdateFSM();
    }

    protected virtual void LateUpdate()
    {
        LateUpdateFSM();
    }

    protected virtual void Setup()
    {
        SetupFSM();
    }

    protected virtual void SetupFSM()
    {
        // [base]
        FsmGroup = new FSMGroup();
        FsmGroup.Setup(this, layerLength: 1);

        // [0]
        FsmGroup.AddState(0, FSMStateType.Idle, new FSMIdleState());
        FsmGroup.AddState(0, FSMStateType.Move, new FSMMoveState());
        FsmGroup.AddState(0, FSMStateType.Interact, new FSMInteractState());

        // [idle]
        FsmGroup.AddTransition(0, FSMStateType.Idle, FSMStateType.Move, () => 
            GetInputMove().sqrMagnitude > 0.001f);
        FsmGroup.AddTransition(0, FSMStateType.Idle, FSMStateType.Interact, () => 
            GetInputInteract() == true);

        // [move]
        FsmGroup.AddTransition(0, FSMStateType.Move, FSMStateType.Idle, () => 
            GetInputMove().sqrMagnitude == 0);
        FsmGroup.AddTransition(0, FSMStateType.Move, FSMStateType.Interact, () => 
            GetInputInteract() == true);

        // [interact]
        FsmGroup.AddTransition(0, FSMStateType.Interact, FSMStateType.Idle, () => 
            GetInputInteract() == true);

        // [run]
        FsmGroup.Run();
    }

    protected virtual void UpdateFSM()
    {
        if (FsmGroup == null)
        {
            return;
        }

        FsmGroup.UpdateState();
    }

    protected virtual void FixedUpdateFSM()
    {
        if (FsmGroup == null)
        {
            return;
        }

        FsmGroup.FixedUpdateState();
    }

    protected virtual void LateUpdateFSM()
    {
        if (FsmGroup == null)
        {
            return;
        }

        FsmGroup.LateUpdateState();
    }

    #region : GetInput

    private Vector3 GetInputMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0, v).normalized;
    }

    private bool GetInputInteract()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    #endregion

    #region : HelperMethod

    public void MoveUpdate()
    {
        Debug.Log("Move");
    }

    #endregion
}
