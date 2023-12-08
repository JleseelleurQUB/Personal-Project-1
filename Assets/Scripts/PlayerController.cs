using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;
    private float inputMouseHorizontal;
    public float speed;
    public float turnSpeedHorizontal;
    public float harpoonRecharge;
    private bool harpoonAvailable = true;
    private bool hitCooldown = true;

    public GameObject harpoonPrefab;
    private Animator playerAnim;
    private Rigidbody playerRB;
    public GameObject firePoint;
    private GameManager gameManager;

    private AudioSource bodyAudio;
    private AudioSource shootAudio;

    public AudioClip footstep;
    public AudioClip shoot;
    public AudioClip impact;

    // Start is called before the first frame update
    void Start()
    {
      playerAnim = GetComponent<Animator>();
      playerRB = GetComponent<Rigidbody>();
      gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
      bodyAudio = GetComponent<AudioSource>();
      shootAudio = firePoint.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks for player control inputs

        GetInputs();


        // controls the players movement and horizontal turning based on the key and mouse inputs

        UpdateMovement();


        // spawns projectile and starts cooldown when pressing space
        if (Input.GetKeyDown(KeyCode.Space) && harpoonAvailable && gameManager.gameActive)
        {
            SpawnHarpoon();
            StartCoroutine(HarpoonRecharge());
        }


    }

    // HARPOON FUNCTIONS

    IEnumerator HarpoonRecharge()
    {
        yield return new WaitForSeconds(harpoonRecharge);
        harpoonAvailable = true;
    }

    void SpawnHarpoon()
    {
        Instantiate(harpoonPrefab, firePoint.transform.position, transform.rotation);
        harpoonAvailable = false;
    }

    // MOVEMENT FUNCTIONS

    void GetInputs()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputMouseHorizontal = Input.GetAxis("Mouse X");
    }

    void UpdateMovement()
    {
        if (gameManager.gameActive)
        {
            // player location and rotation update

            transform.Translate(Vector3.right * inputHorizontal * Time.deltaTime * speed);
            transform.Translate(Vector3.forward * inputVertical * Time.deltaTime * speed);
            transform.Rotate(Vector3.up, inputMouseHorizontal * turnSpeedHorizontal);

            // animation parameters update

            if (playerRB.velocity.magnitude > 0)
            {
                playerAnim.SetBool("Idle", false);
            }
            else
            {
                playerAnim.SetBool("Idle", true);
            }

            playerAnim.speed = 1;
            playerAnim.SetFloat("Horizontal Speed", -inputHorizontal);
            playerAnim.SetFloat("Forward Speed", inputVertical);
        }
        else
        {
            playerAnim.speed = 0;
        }
    }

    // PLAYER HIT FUNCTIONS

    // Calls game manager to reduce player healthbar when colliding with a hitbox
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && hitCooldown)
        {
          
            gameManager.WoundPlayer();
            bodyAudio.clip = impact;
            bodyAudio.time = 0.6f;
            bodyAudio.Play();


            hitCooldown = false;
            StartCoroutine(PlayerHitCooldown());
        }
    }

    IEnumerator PlayerHitCooldown()
    {
        yield return new WaitForSeconds(2);
        hitCooldown = true;
    }
}
