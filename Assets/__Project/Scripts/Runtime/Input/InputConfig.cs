using UnityEngine;

[CreateAssetMenu(fileName ="InputConfig", menuName = "RWM/Input/Config")]
public class InputConfig : ScriptableObject
{
    [SerializeField] private LayerMask m_groundLayer;

    public LayerMask GroundLayer => m_groundLayer;
}
