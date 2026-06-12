using Mirror;
using System;
using UnityEngine;

public class Status : NetworkBehaviour
{
    public FSMCtrl Ctrl { get; private set; }

    public SyncDictionary<StatType, float> Values { get; } = new();

    public float this[StatType type]
    {
        get
        {
            if (Values.ContainsKey(type) == false)
            {
                return 0.0f;
            }

            return Values[type];
        }
    }

    public void Setup(FSMCtrl ctrl)
    {
        Ctrl = ctrl;

        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            Values.Add(type, -1);
        }

        Values[StatType.Hp] = 10;
        Values[StatType.MoveSpeed] = 3;
    }
}
