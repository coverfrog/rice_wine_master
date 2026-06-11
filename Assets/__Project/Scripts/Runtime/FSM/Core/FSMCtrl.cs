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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, 0f, vertical).normalized;
    }
    public void Move(Vector3 direction)
    {
        m_charCtrl.Move(direction * m_moveSpeed * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    #endregion
}
