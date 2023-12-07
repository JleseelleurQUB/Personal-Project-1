using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BossController : MonoBehaviour
{
    private Vector3 playerDirection;
    public GameObject player;
    public Animator bossAnim;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        bossAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // finds player direction and moves towards them, needs adjustment to not include vertical movement or it sinks into the floor
        playerDirection = transform.position - player.transform.position;
        playerDirection.y = 0;

        if (gameManager.gameActive)
        {
            bossAnim.speed = 1;
            if (playerDirection.magnitude > 10)
            {
                bossAnim.SetBool("Tracking", true);
                transform.LookAt(player.transform.position);
                transform.Translate(playerDirection.normalized * Time.deltaTime * 4);
            }
            else
            {
                bossAnim.SetBool("Tracking", false);
                bossAnim.SetTrigger("Attack");
                transform.LookAt(player.transform.position);
            }
        }
        else 
        {
            bossAnim.speed = 0;
        }
    }

}
