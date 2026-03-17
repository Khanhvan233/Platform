using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public class RespawnCoin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float respawnTime;
    private SpriteRenderer sprite;
    private Collider2D col;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    IEnumerator RespawnCoroutine()
    {
        // Ẩn coin
        gameObject.SetActive(false);
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // Hiện lại coin
        gameObject.SetActive(true);
        col.enabled = true;
    }
}
