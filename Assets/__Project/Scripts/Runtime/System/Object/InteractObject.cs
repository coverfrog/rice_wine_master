using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class InteractObject : NetworkBehaviour
{
    [Header("Interact")]
    [SyncVar] 
    public bool IsClickable = true;

    [Space]
    public UnityEvent<InteractObject> OnEnter;

    public virtual void Interact()
    {
        OnEnter?.Invoke(this);
    }
}
