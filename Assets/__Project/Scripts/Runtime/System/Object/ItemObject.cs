using Mirror;
using UnityEngine;

public class ItemObject : InteractObject
{
    [Header("Item")]
    [SerializeField] private ulong m_id = 1;

    public override void Interact()
    {
        base.Interact();
    }
}
