using Mirror;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    public FSMCtrl Ctrl { get; private set; }

    public void Setup(FSMCtrl ctrl)
    {
        Ctrl = ctrl;
    }
}
