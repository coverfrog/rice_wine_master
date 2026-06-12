using System;
using UnityEngine;

[Serializable]
public struct FSMInputContext
{
    public bool InteractAble;
    public uint InteractID;
    public Vector3 MoveDirection;
}
