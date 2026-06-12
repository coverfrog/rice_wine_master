using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FSM
{
    public Dictionary<FSMStateType, FSMState> StateDict { get; } = new();

    public Dictionary<FSMStateType, List<FSMCondition>> ConditionDict { get; } = new();

    public FSMCtrl Ctrl { get; }
    
    public bool IsRun { get; private set; }

    public FSMStateType CurrentStateType { get; private set; }

    public FSM(FSMCtrl ctrl)
    {
        Ctrl = ctrl;
    }

    public void AddState(FSMStateType type, FSMState state)
    {
        StateDict.Add(type, state);

        if (ConditionDict.ContainsKey(type) == false)
        {
            ConditionDict.Add(type, new List<FSMCondition>());
        }
    }

    public void AddTransition(FSMStateType fromType, FSMStateType toType, Func<bool> condition)
    {
        ConditionDict[fromType].Add(new FSMCondition(fromType, toType, condition));
    }

    public void UpdateState()
    {
        if (IsRun == false)
        {
            return;
        }

        StateDict[CurrentStateType].OnUpdate();

        foreach (FSMCondition condition in ConditionDict[CurrentStateType])
        {
            if (condition.Condition())
            {
                ChangeState(condition.ToType);
            }
        }
    }

    public void FixedUpdateState()
    {
        StateDict[CurrentStateType].OnFixedUpdate();

    }

    public void LateUpdateState()
    {
        StateDict[CurrentStateType].OnLateUpdate();
    }

    public void Run()
    {
        if (StateDict.Count == 0)
        {
            return;
        }

        IsRun = true;
        CurrentStateType = StateDict.Keys.First();
        StateDict[CurrentStateType].OnEnter();
    }

    public void ChangeState(FSMStateType to)
    {
        StateDict[CurrentStateType].OnExit();
        CurrentStateType = to;
        StateDict[CurrentStateType].OnEnter();
    }
}
