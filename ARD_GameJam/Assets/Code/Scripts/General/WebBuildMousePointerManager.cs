using UnityEngine;
using UnityEngine.InputSystem;

public class WebBuildMousePointerManager : MonoBehaviour
{
    [SerializeField] private InputActionReference m_clickInActRef;
    public static WebBuildMousePointerManager Instance {  get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        m_clickInActRef.action.performed += OnScreenClicked;
    }
    private void OnDisable()
    {
        m_clickInActRef.action.performed -= OnScreenClicked;
    }
    private void OnScreenClicked(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentGameState == GameStates.RUNNING)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState= CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) 
        { 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        //else Cursor.lockState = CursorLockMode.Confined;
    }
}
