using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public Vector2 offset;

    void LateUpdate()
    {
        Vector2 pos = (Vector2)player.position + offset;
        transform.position = new Vector3(pos.x, pos.y, -10f);
    }
}
