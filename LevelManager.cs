using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject currentCheckPoint;

    private Controller player;
    public int pointsToSubtractOnDeath;

    public GameObject respawnParticle;
    public GameObject deathParticle;

    private float gravityStore;

    void Start()
    {
        player = FindAnyObjectByType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void RespawnPlayer()
    {
            StartCoroutine(RespawnPlayerCo());
    }
 
    public IEnumerator RespawnPlayerCo()
    {
        // Tao Instan cho particle khi player chet
        GameObject death = Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        //Chạy particle
        ScoreManager.AddPoints(-pointsToSubtractOnDeath);
        death.GetComponent<ParticleSystem>().Play();

        // Xoa nhan vat khi chet
        player.enabled = false;
        player.GetComponent<Renderer>().enabled = false;

        // tat gravity khi chet
        gravityStore = player.GetComponent<Rigidbody2D>().gravityScale;
        player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;


        Debug.Log("Respawn Player");
        yield return new WaitForSeconds(1f);
        player.transform.position = currentCheckPoint.transform.position;

        // bat gravity khi player respawn
        player.GetComponent<Rigidbody2D>().gravityScale = gravityStore;

        //Tao Instan cho particle khi player respawn 
        GameObject respawn = Instantiate(respawnParticle, currentCheckPoint.transform.position, currentCheckPoint.transform.rotation);


        // hien nhan vat khi chet
        player.enabled = true;
        player.GetComponent<Renderer>().enabled = true;

        //Xoa particle sau 2s
        Destroy(respawn, 2f);
        Destroy(death, 2f);

    }
}
