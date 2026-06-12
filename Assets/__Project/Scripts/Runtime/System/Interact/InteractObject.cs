using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class InteractObject : NetworkBehaviour
{
    [Header("Option")]
    [SyncVar] 
    public bool IsClickable = true;

    [Header("Event")]
    public UnityEvent<InteractObject> OnEnter;

    public void Interact()
    {
        Debug.Log("Interact");

        OnEnter?.Invoke(this);
    }
}
