using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class InteractObject : NetworkBehaviour, IInteract
{
    public uint GetNetID()
    {
        return netId;
    }

    public void Interact()
    {
        
    }
}
