using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField] private ulong m_id;
    [SerializeField] private string m_modelPath;

    public ulong ID => m_id;

    public string ModelPath => m_modelPath;
}
