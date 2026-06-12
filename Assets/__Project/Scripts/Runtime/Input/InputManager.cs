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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(h, 0, v).normalized;

        if (movement.sqrMagnitude < 0.001f) movement = Vector3.zero;

        InputContext context = Context;
        context.MoveDirection.x = movement.x;
        context.MoveDirection.z = movement.z;

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
