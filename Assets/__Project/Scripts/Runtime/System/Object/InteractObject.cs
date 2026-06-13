using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class InteractObject : NetworkBehaviour
{
    [Header("Interact")]
    [SyncVar] 
    public bool IsClickable = true;

    public virtual void Interact(Action actExit)
    {

    }
}
