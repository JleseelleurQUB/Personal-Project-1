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
        harpoonRB.AddForce(transform.forward * shootVelocity * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDistance = player.transform.position - transform.position;
        if(Vector3.Magnitude(playerDistance) > vanishDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.WoundBoss();
            Destroy(gameObject);
        }
    }
}
