using UnityEngine;

public class MoveTexture : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;
    private Renderer rend;
    
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float moveThis = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, moveThis));
    }
}
