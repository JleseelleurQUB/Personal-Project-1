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
    
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        harpoonRB = GetComponent<Rigidbody>();

        // shoots the projectile forwards upon instantiation
        harpoonRB.AddForce(transform.forward * shootVelocity * Time.deltaTime, ForceMode.Impulse);
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        // calls game manager's wound boss function upon impacting the boss's collider
        if (collision.gameObject.CompareTag("Boss"))
        {
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.WoundBoss();
            Debug.Log("hit");
            Destroy(gameObject);
        }
    }
}
