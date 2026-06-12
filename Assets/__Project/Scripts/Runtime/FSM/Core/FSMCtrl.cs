using System.Collections.Generic;
using UnityEngine;

public abstract class FSMCtrl : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private List<FSM> m_fsms = new();

    [Header("Ref")]
    [SerializeField] private CharacterController m_charCtrl = null;
    [SerializeField] private Animator animator = null;

    [Header("Value")]
    [SerializeField] private float m_moveSpeed = 3.0f;
    [SerializeField] private float m_rotationSpeed = 20.0f;

    #region Start

    public virtual void Start()
    {
        Setup();
    }

    #endregion

    #region Update

    private void Update()
    {
        if (m_fsms is { Count: > 0 })
        {
            foreach (FSM fsm in m_fsms)
            {
                fsm.UpdateState();
            }
        }
    }

    #endregion

    #region Setup

    public virtual void Setup()
    {
        if (m_fsms is { Count: > 0 })
        {
            foreach (FSM fsm in m_fsms)
            {
                fsm.Setup(this);
            }
        }
    }

    #endregion

    #region Get

    public Animator Animator => animator;

    #endregion

    #region Helper Methods

    public Vector3 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0f, v).normalized;
    }
    public void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Vector3 moveVelocity = direction * m_moveSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);

        m_charCtrl.Move(moveVelocity * Time.deltaTime);
    }

    #endregion
}
