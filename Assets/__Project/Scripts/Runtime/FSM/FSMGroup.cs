
using System;
using System.Collections.Generic;
using UnityEngine;

public class FSMGroup
{
    public FSMCtrl Ctrl { get; private set; }

    public Dictionary<int, FSM> FSMDict { get; private set; }

    public void Setup(FSMCtrl ctrl, int layerLength)
    {
        Ctrl = ctrl;
        FSMDict = new Dictionary<int, FSM>(capacity: layerLength);

        for (int layer = 0; layer < layerLength; layer++)
        {
            FSMDict.Add(layer, new FSM(ctrl));
        }
    }

    public void AddState(int layer, FSMStateType type, FSMState state)
    {
        FSMDict[layer].AddState(type, state);
    }

    public void AddTransition(int layer, FSMStateType fromType, FSMStateType toType, Func<bool> condition)
    {
        FSMDict[layer].AddTransition(fromType, toType, condition);
    }

    public void UpdateState()
    {
        foreach (FSM fsm in FSMDict.Values)
        {
            fsm.UpdateState();
        }
    }

    public void Run()
    {
        foreach (FSM fsm in FSMDict.Values)
        {
            fsm.Run();
        }
    }
}
