using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class InteractObject : NetworkBehaviour
{
    [SyncVar] public bool IsClickable = true;

    public void Interact()
    {
        
    }
}
