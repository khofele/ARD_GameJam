using UnityEngine;

public class SpinObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0f, 1f, 0f, Space.Self);
    }
}
