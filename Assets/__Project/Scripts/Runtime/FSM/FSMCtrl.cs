using UnityEngine;

public class FSMCtrl : MonoBehaviour
{
    private FSMGroup m_fsmGroup;

    protected virtual void Awake()
    {
        Setup();
    }

    protected virtual void Update()
    {
        UpdateFSM();
    }

    protected virtual void Setup()
    {
        SetupFSM(ref m_fsmGroup);
    }

    protected virtual void SetupFSM(ref FSMGroup fsmGroup)
    {
        fsmGroup = new FSMGroup();
        fsmGroup.Setup(this, layerLength: 1);

        fsmGroup.AddState(0, FSMStateType.Idle, new FSMIdleState());
        fsmGroup.AddState(0, FSMStateType.Move, new FSMMoveState());

        fsmGroup.AddTransition(0, FSMStateType.Idle, FSMStateType.Move, () => GetInputMovement().sqrMagnitude > 0.001f);
        fsmGroup.AddTransition(0, FSMStateType.Move, FSMStateType.Idle, () => GetInputMovement().sqrMagnitude == 0);

        fsmGroup.Run();
    }

    protected virtual void UpdateFSM()
    {
        if (m_fsmGroup == null)
        {
            return;
        }

        m_fsmGroup.UpdateState();
    }

    private Vector3 GetInputMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0, v).normalized;
    }
}
