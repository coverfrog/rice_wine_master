using UnityEngine;
using Mirror;
[RequireComponent(typeof(Status))]

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Inventory))]
public class FSMCtrl : NetworkBehaviour
{
    public FSMGroup FSMGroup { get; protected set; }

    // [sync]
    [SyncVar(hook = nameof(HookInputContext))]
    [HideInInspector] public FSMInputContext InputContext;


    // [value]
    private RaycastHit[] m_interactHitResult = new RaycastHit[10];

    #region : Component

    public Status Status
    {
        get
        {
            if (m_status == null) m_status = GetComponent<Status>();
            return m_status;
        }
    }

    private Status m_status;


    public Rigidbody Rb3d
    {
        get
        {
            if (m_rb3d == null) m_rb3d = GetComponent<Rigidbody>();
            return m_rb3d;
        }
    }

    private Rigidbody m_rb3d;


    public Inventory Inventory
    {
        get
        {
            if (m_inventory == null) m_inventory = GetComponent<Inventory>();
            return m_inventory;
        }
    }

    private Inventory m_inventory;

    #endregion

    #region : Mirror

    public override void OnStartServer()
    {
        base.OnStartServer();

        SetupStatus();
        SetupInventory();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        SetupFSM();
        SetupInputs();
        SetupCam();
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
        FSMGroup.AddState(0, FSMStateType.Idle, new FSMIdleState(this));
        FSMGroup.AddState(0, FSMStateType.Move, new FSMMoveState(this));
        FSMGroup.AddState(0, FSMStateType.Interact, new FSMInteractState(this));

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

    protected virtual void SetupInventory()
    {
        // [base]
        Inventory.Setup(this);
    }


    protected virtual void SetupCam()
    {
        // [return]
        if (isLocalPlayer == false)
        {
            return;
        }

        // [follow]
        CamManager.Instance.Follow(transform);
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
        context.MoveDirection = InputManager.Instance.Context.MoveDirection;

        InputContext = context;
    }

    protected virtual void UpdateInputsInteract()
    {
        const float rayDistance = 2.0f;

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red, 0.1f);
#endif

        FSMInputContext context = InputContext;
        bool isInput = InputManager.Instance.Context.IsInteract;

        if (InputContext.InteractID == 0)
        {
            int hitCount = Physics.RaycastNonAlloc(transform.position, transform.forward, m_interactHitResult, maxDistance: rayDistance);

            if (hitCount == 0)
            {
                context.InteractAble = false;
                InputContext = context;

                return;
            }

            InteractObject interactObject = null;

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit result = m_interactHitResult[i];

                if (result.collider != null &&
                    result.collider.transform.IsChildOf(transform) == true)
                {
                    continue;
                }

                if (result.rigidbody != null &&
                    result.rigidbody.TryGetComponent(out interactObject) == true)
                {
                    break;
                }

                if (result.collider != null &&
                    result.collider.TryGetComponent(out interactObject) == false)
                {
                    break;
                }
            }

            if (interactObject != null)
            {
                context.InteractAble = true;

                if (isInput)
                {
                    context.InteractID = interactObject.netId;
                }
            }
            else
            {
                context.InteractAble = false;
            }

            InputContext = context;
        }
        else
        {
            if (isInput == false)
            {
                return;
            }

            context.InteractID = 0;

            InputContext = context;
        }
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
            if (oldValue.InteractAble != newValue.InteractAble)
            {

            }
        }
        else
        {

        }
    }

    #endregion
}
