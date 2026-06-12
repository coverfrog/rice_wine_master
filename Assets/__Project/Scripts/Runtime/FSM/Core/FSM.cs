using System;
using UnityEngine;

[Serializable]
public class FSM
{
    [SerializeField] private FSMStateSO m_startState = null;

    private FSMCtrl m_ctrl;
    private FSMStateSO m_currentState = null;

    public void Setup(FSMCtrl ctrl)
    {
        m_ctrl = ctrl;

        if (m_startState != null)
        {
            m_currentState = m_startState;
            m_currentState.EnterState(m_ctrl);
        }
    }

    public void UpdateState()
    {
        if (m_ctrl == null) return;

        m_currentState.UpdateState(m_ctrl);
        m_currentState.CheckTransitions(this, m_ctrl);
    }
    
    public void TransitionToState(FSMStateSO nextState)
    {
        if (m_ctrl == null ||  
            m_currentState == nextState ||
            nextState == null)
        {
            return;
        }

        Debug.Log(nextState.name);

        m_currentState.ExitState(m_ctrl);
        m_currentState = nextState;
        m_currentState.EnterState(m_ctrl);
    }
}
