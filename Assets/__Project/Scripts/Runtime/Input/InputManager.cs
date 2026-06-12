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
        context.IsInteract = Input.GetKeyDown(KeyCode.E);

        Context = context;
    }

    #endregion
}
