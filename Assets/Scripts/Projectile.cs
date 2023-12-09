using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float shootVelocity;
    private Rigidbody harpoonRB;
    private GameObject player;
    public float vanishDistance;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        harpoonRB = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // shoots the projectile forwards upon instantiation
        harpoonRB.AddForce(transform.forward * shootVelocity, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // finds the distance to the player, destroys the projectile if it travels too far away
        Vector3 playerDistance = player.transform.position - transform.position;
        if(Vector3.Magnitude(playerDistance) > vanishDistance)
        {
            Destroy(gameObject);
        }

        if (gameManager.gameActive == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // calls game manager's wound boss function upon impacting the boss's collider
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossController bossScript = collision.gameObject.GetComponent<BossController>();
            bossScript.HurtBoss();
            gameManager.WoundBoss();
            Debug.Log("hit");
            Destroy(gameObject);
        }
    }
}
