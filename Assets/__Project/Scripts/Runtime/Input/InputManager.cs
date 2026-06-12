using Mirror;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }

    public InputConfig Config { get; private set; }

    public InputContext Context { get; private set; }

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

    private void Start()
    {
        Addressables.LoadAssetAsync<InputConfig>("config/input").Completed += handle =>
        {
            if (handle.Status != AsyncOperationStatus.Succeeded) return;

            Config = handle.Result;
            Context = new();
        };
    }

    private void OnDestroy()
    {
        if (Config != null) Addressables.Release(Config);
    }

    private void Update()
    {
        if (Config == null) return;

        UpdateInputMove();
        UpdateInputInteract();
    }

    public void Clear()
    {
        Context = new InputContext();
    }

    #region : Update Input

    private void UpdateInputMove()
    {
        if (Input.GetMouseButtonDown(1) == false) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: Mathf.Infinity, Config.GroundLayer) == false) return;

        InputContext context = Context;
        context.MoveGroundPoint.x = hit.point.x;
        context.MoveGroundPoint.z = hit.point.z;

        Context = context;
    }

    private void UpdateInputInteract()
    {
        InputContext context = Context;

        if (Input.GetMouseButtonDown(0) == false) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: Mathf.Infinity) == false) return;

        IInteract interact = null;

        if (hit.rigidbody != null &&
            hit.rigidbody.TryGetComponent(out interact) == true)
        {
        }

        if (interact == null)
        {
            if (hit.collider != null &&
                hit.collider.TryGetComponent(out interact))
            {
            
            }
        }

        if (interact == null)
        {
            return;
        }

        context.InteractID = interact.GetNetID();

        Context = context;
    }

    #endregion
}
