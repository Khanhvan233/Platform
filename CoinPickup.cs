using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{
    public int pointToAdd = 100;
    //public float respawnTime = 5f;

    //private Collider2D coinCollider;
    //private SpriteRenderer coinRenderer;
    //private Animator coinAnimator;

    //private void Awake()
    //{
    //    coinCollider = GetComponent<Collider2D>();
    //    coinRenderer = GetComponent<SpriteRenderer>();
    //    coinAnimator = GetComponent<Animator>();
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tăng điểm
            ScoreManager.AddPoints(pointToAdd);
            Destroy(gameObject);

            // Ẩn coin và tạm dừng animation
            //coinRenderer.enabled = false;
            //coinCollider.enabled = false;
            //coinAnimator.enabled = false;

            //// Respawn sau thời gian
            //StartCoroutine(RespawnCoin());
        }
    }

    //private IEnumerator RespawnCoin()
    //{
    //    yield return new WaitForSeconds(respawnTime);

    //    // Hiện lại coin
    //    coinRenderer.enabled = true;
    //    coinCollider.enabled = true;
    //    coinAnimator.enabled = true;

    //    // Reset animation từ đầu (layer 0)
    //    coinAnimator.Play("Spin", 0, 0f);
    //}
}