using Unity.Cinemachine;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static CamManager Instance { get; private set; }

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Follow(Transform target)
    {
        if (Camera.main.TryGetComponent(out CinemachineCamera cc) == false)
        {
            return;
        }

        cc.Follow = target;
    }
}
