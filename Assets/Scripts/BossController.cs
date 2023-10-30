using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossController : MonoBehaviour
{
    private Vector3 playerDirection;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // finds player direction and moves towards them, needs adjustment to not include vertical movement or it sinks into the floor
        playerDirection = player.transform.position - transform.position;
        transform.Translate(playerDirection.normalized * Time.deltaTime * 4);
    }

    // console tells player if they have been hit
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player hit");
    }
}
