using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool alive = true;
    private bool falling = false;

    public GameObject firePoint;
    public GameObject harpoonPrefab;
    public ParticleSystem bloodSpatter;
    public ParticleSystem airBubbles;
    private Animator playerAnim;
    private Rigidbody playerRB;
 
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
        if (alive)
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
       
        if (falling)
        {
            transform.Translate(0, -0.1f, 0);
        }

        if (transform.position.y < -30 && GameObject.Find("Player Health Bar").GetComponent<Slider>().value > 0)
        {
            gameManager.WoundPlayer();
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
        shootAudio.PlayOneShot(shoot);
        Instantiate(harpoonPrefab, firePoint.transform.position, transform.rotation);
        Instantiate(airBubbles, firePoint.transform.position, firePoint.transform.rotation);
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

    public void FootstepAudio()
    {
        bodyAudio.volume = 0.05f;
        bodyAudio.PlayOneShot(footstep);
    }


    // PLAYER HIT FUNCTIONS

    // Calls game manager to reduce player healthbar when colliding with a hitbox
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && hitCooldown)
        {
          
            gameManager.WoundPlayer();
            bodyAudio.volume = 0.9f;
            bodyAudio.PlayOneShot(impact);
            Instantiate(bloodSpatter, transform.position, bloodSpatter.transform.rotation);


            hitCooldown = false;
            StartCoroutine(PlayerHitCooldown());
        }
    }

    IEnumerator PlayerHitCooldown()
    {
        yield return new WaitForSeconds(2);
        hitCooldown = true;
    }

    public void PlayerDeath()
    {
        alive = false;
        falling = true;
    }
}
