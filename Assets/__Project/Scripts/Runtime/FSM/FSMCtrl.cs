using UnityEngine;
using Mirror;

[RequireComponent(typeof(Status))]
public class FSMCtrl : NetworkBehaviour
{
    public FSMGroup FsmGroup { get; private set; }

    #region : Status

    public Status Status
    {
        get
        {
            if (m_status == null) m_status = GetComponent<Status>();
            return m_status;
        }
    }

    private Status m_status;

    #endregion

    public override void OnStartServer()
    {
        base.OnStartServer();

        SetupStatus();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        SetupFSM();
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

    protected virtual void SetupFSM()
    {
        // [return]
        if (isLocalPlayer == false)
        {
            return;
        }

        // [base]
        FsmGroup = new FSMGroup(this, layerLength: 1);

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

    protected virtual void SetupStatus()
    {
        // [base]
        Status.Setup(this);

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

    protected virtual Vector3 GetInputMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0, v).normalized;
    }

    protected virtual bool GetInputInteract()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    #endregion

    #region : HelperMethod

    public virtual void MoveUpdate(float deltaTime)
    {
        transform.position += GetInputMove() * Status[StatType.MoveSpeed] * deltaTime; 
    }

    #endregion
}
