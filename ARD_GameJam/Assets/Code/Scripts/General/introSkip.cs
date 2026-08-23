using UnityEngine;
using UnityEngine.InputSystem;

public class introSkip : MonoBehaviour
{
    public InputActionReference introSkipInActRef;

    private void Start()
    {
        introSkipInActRef.action.Enable();
    }
    private void OnDestroy()
    {
        introSkipInActRef.action.Disable();
    }
    private void Update()
    {
        if (introSkipInActRef.action.WasPerformedThisFrame())
            GameManager.Instance.LoadLevel((int)GameScenes.Level1);
    }
}
