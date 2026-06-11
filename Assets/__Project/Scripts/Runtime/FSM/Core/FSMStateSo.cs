using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewState", menuName = "RWM/FSM/State")]
public class FSMStateSO : ScriptableObject
{
    [Header("Actions")]
    [SerializeField] private List<FSMActionSO> m_entryActions = new();
    [SerializeField] private List<FSMActionSO> m_updateActions = new();
    [SerializeField] private List<FSMActionSO> m_exitActions = new();

    #region Get

    public IReadOnlyList<FSMActionSO> EntryActions => m_entryActions;
    public IReadOnlyList<FSMActionSO> UpdateActions => m_updateActions;
    public IReadOnlyList<FSMActionSO> ExitActions => m_exitActions;

    #endregion

    [Header("Condition")]
    [SerializeField] private List<FSMTransition> m_transtions = new();

    #region Get

    public IReadOnlyList<FSMTransition> Transitions => m_transtions;

    #endregion

    #region EnterState

    public void EnterState(FSMCtrl ctrl)
    {
        ExecuteActions(ctrl, m_entryActions);
    }

    #endregion

    #region UpdateState

    public void UpdateState(FSMCtrl ctrl)
    {
        ExecuteActions(ctrl, m_updateActions);
    }

    #endregion

    #region ExitState

    public void ExitState(FSMCtrl ctrl)
    {
        ExecuteActions(ctrl, m_exitActions);
    }

    #endregion

    #region ExecuteActions

    private void ExecuteActions(FSMCtrl ctrl, List<FSMActionSO> actions)
    {
        if (actions == null) return;
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i] != null)
            {
                actions[i].Execute(ctrl);
            }
        }
    }

    #endregion

    #region CheckTransitions

    public void CheckTransitions(FSM fsm, FSMCtrl ctrl)
    {
        if (m_transtions == null) return;
        for (int i = 0; i < m_transtions.Count; i++)
        {
            bool decisionSucceeded = m_transtions[i].decision.Decide(ctrl);
            if (decisionSucceeded)
            {
                fsm.TransitionToState(m_transtions[i].trueState);
                return;
            }
            else if (m_transtions[i].falseState != null)
            {
                fsm.TransitionToState(m_transtions[i].falseState);
                return;
            }
        }
    }

    #endregion
}
