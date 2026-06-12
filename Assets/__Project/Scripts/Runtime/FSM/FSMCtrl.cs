using UnityEngine;
using Mirror;

[RequireComponent(typeof(Status))]
public class FSMCtrl : NetworkBehaviour
{
    public FSMGroup FSMGroup { get; private set; }

    [SyncVar(hook = nameof(HookInputContext))]
    [HideInInspector]
    public FSMInputContext InputContext;

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

    #region : Mirror

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

    #endregion

    #region : Unity

    protected virtual void Update()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        UpdateFSM();
        UpdateInputs();
    }

    protected virtual void FixedUpdate()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        FixedUpdateFSM();
    }

    protected virtual void LateUpdate()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        LateUpdateFSM();
    }

    #endregion

    #region : Setup

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

    protected virtual void SetupStatus()
    {
        // [base]
        Status.Setup(this);
    }

    protected virtual void SetupInputs()
    {
        // [base]
        InputContext = new FSMInputContext();

        // [clear]
        InputManager.Instance.Clear();
    }

    #endregion

    #region : Update

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

        UpdateInputsMoveDirection();
        UpdateInputsInteract();
    }

    protected virtual void UpdateInputsMoveDirection()
    {
        FSMInputContext context = InputContext;

        Vector3 pointXZ = new(transform.position.x, 0, transform.position.z);
        Vector3 moveGroundPointXZ = InputManager.Instance.Context.MoveGroundPoint;

        if (Vector3.Distance(pointXZ, moveGroundPointXZ) < 0.2f)
        {
            context.MoveDirection = Vector3.zero;
            InputContext = context;
            return;
        }

        Vector3 direction = (moveGroundPointXZ - pointXZ).normalized;

        if (direction.sqrMagnitude < 0.001f)
        {
            context.MoveDirection = Vector3.zero;
            InputContext = context;
            return;
        }

        context.MoveDirection = direction;
        InputContext = context;
    }

    protected virtual void UpdateInputsInteract()
    {
        FSMInputContext context = InputContext;
        context.InteractID = InputManager.Instance.Context.InteractID;

        InputContext = context;
    }

    #endregion

    #region : FixedUpdate

    protected virtual void FixedUpdateFSM()
    {
        if (FSMGroup == null)
        {
            return;
        }

        FSMGroup.FixedUpdateState();
    }

    #endregion

    #region : LateUpdate

    protected virtual void LateUpdateFSM()
    {
        if (FSMGroup == null)
        {
            return;
        }

        FSMGroup.LateUpdateState();
    }

#endregion

    #region : GetInput

    protected virtual Vector3 GetInputMoveDirection()
    {
        return InputContext.MoveDirection;
    }

    protected virtual bool GetInputInteract()
    {
        return InputContext.InteractID != 0;
    }

    #endregion

    #region : Hook

    public virtual void HookInputContext(FSMInputContext oldValue, FSMInputContext newValue)
    {
        if (isLocalPlayer == true)
        {
            if (oldValue.InteractID != newValue.InteractID)
            {
                Debug.Log(newValue.InteractID);
            }
        }
        else
        {

        }
    }

    #endregion
}
