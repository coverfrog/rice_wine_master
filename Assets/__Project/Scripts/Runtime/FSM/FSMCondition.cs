using System;
using UnityEngine;

public class FSMCondition
{
    public FSMStateType FromType { get; }

    public FSMStateType ToType { get; }

    public Func<bool> Condition { get; }

    public FSMCondition(FSMStateType fromType, FSMStateType toType, Func<bool> condition)
    {
        FromType = fromType;
        ToType = toType;
        Condition = condition;
    }
}
