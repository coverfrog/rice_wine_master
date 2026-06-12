using UnityEngine;
using Mirror;

[RequireComponent(typeof(Status))]
public class FSMCtrl : NetworkBehaviour
{
    public FSMGroup FSMGroup { get; private set; }

    [SyncVar] public InputContext BufferInputContext;

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
        SetupInputs();
    }

    protected virtual void Update()
    {
        UpdateFSM();
        UpdateInputs();
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
        FSMGroup = new FSMGroup(this, layerLength: 1);

        // [0]
        FSMGroup.AddState(0, FSMStateType.Idle, new FSMIdleState());
        FSMGroup.AddState(0, FSMStateType.Move, new FSMMoveState());
        FSMGroup.AddState(0, FSMStateType.Interact, new FSMInteractState());

        // [idle]
        FSMGroup.AddTransition(0, FSMStateType.Idle, FSMStateType.Move, () => 
            GetInputMoveDirection().sqrMagnitude > 0.001f);
        FSMGroup.AddTransition(0, FSMStateType.Idle, FSMStateType.Interact, () => 
            GetInputInteract() == true);

        // [move]
        FSMGroup.AddTransition(0, FSMStateType.Move, FSMStateType.Idle, () => 
            GetInputMoveDirection().sqrMagnitude == 0);
        FSMGroup.AddTransition(0, FSMStateType.Move, FSMStateType.Interact, () => 
            GetInputInteract() == true);

        // [interact]
        FSMGroup.AddTransition(0, FSMStateType.Interact, FSMStateType.Idle, () => 
            GetInputInteract() == false);

        // [run]
        FSMGroup.Run();
    }

    protected virtual void SetupInputs()
    {
        // [return]
        if (isLocalPlayer == false)
        {
            return;
        }

        // [base]
        
    }

    protected virtual void SetupStatus()
    {
        // [base]
        Status.Setup(this);
    }

    protected virtual void UpdateFSM()
    {
        if (FSMGroup == null)
        {
            return;
        }

        FSMGroup.UpdateState();
    }

    protected virtual void UpdateInputs()
    {
        if (InputManager.Instance == null)
        {
            return;
        }

        BufferInputContext = InputManager.Instance.Context;
    }

    protected virtual void FixedUpdateFSM()
    {
        if (FSMGroup == null)
        {
            return;
        }

        FSMGroup.FixedUpdateState();
    }

    protected virtual void LateUpdateFSM()
    {
        if (FSMGroup == null)
        {
            return;
        }

        FSMGroup.LateUpdateState();
    }

    #region : GetInput

    protected virtual Vector3 GetInputMoveDirection()
    {
        Vector3 pointXZ = new(transform.position.x, 0, transform.position.z);
        Vector3 moveGroundPointXZ = BufferInputContext.MoveGroundPoint;

        if (Vector3.Distance(pointXZ, moveGroundPointXZ) < 0.5f)
            return Vector3.zero;

        Vector3 direction = (moveGroundPointXZ - pointXZ).normalized;

        if (direction.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return direction;
    }

    protected virtual bool GetInputInteract()
    {
        return BufferInputContext.InteractID != 0;
    }

    #endregion

    #region : HelperMethod

    public virtual void MoveUpdate(float deltaTime)
    {
        transform.position += deltaTime * Status[StatType.MoveSpeed] * GetInputMoveDirection();
    }

    #endregion
}
