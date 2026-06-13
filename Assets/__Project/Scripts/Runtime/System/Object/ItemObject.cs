using Mirror;
using System;
using UnityEngine;

public class ItemObject : InteractObject
{
    [Header("Item")]
    [SyncVar, SerializeField] 
    private ulong m_id = 1;

    [SyncVar, SerializeField]
    private int m_count = 1;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        LoadModel();
    }

    private void LoadModel()
    {
        if (DataManager.Instance.ItemDataDict.TryGetValue(m_id, out ItemData data) == false)
        {
            return;
        }
    }


    public override void Interact(Action actExit)
    {
        Debug.Log($"{m_id}: {m_count}");

        actExit?.Invoke();
    }
}
