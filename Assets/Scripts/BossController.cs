using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;

public class BossController : MonoBehaviour
{
    private Vector3 playerDirection;
    private Vector3 playerLocationPlane;
    public GameObject player;
    public GameObject arm;
    private Animator bossAnim;
    private GameManager gameManager;
    private AudioSource bossAudio;
    public AudioClip[] creatureNoises;
    private bool alive = true;
    private bool falling = false;
    private bool preppingCharge = false;
    private bool chargeReady = false;
    private bool charging = false;
    private bool chargeAvailable = true;
    [SerializeField] float chargeTime;
    [SerializeField] float chargeSpeed;
    private Collider armHitbox;



    // Start is called before the first frame update
    void Start()
    {
        bossAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        bossAudio = GetComponent<AudioSource>();
        alive = true;
        armHitbox = arm.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
 
        
        // Controls boss position, movement, animation
        if (alive)
        {
            BossAI();

        }


        // Boss continually falls when defeated
        if (falling)
        {
            transform.Translate(0, -0.1f, 0);
        }

    }


    private void BossAI()
    {
        // Prevents boss falling through floor when pushed by player
        transform.position = new Vector3(transform.position.x, -0.9f, transform.position.z);

        // Finds player direction
        playerLocationPlane = player.transform.position;
        playerLocationPlane.y = 0;

        playerDirection = transform.position - playerLocationPlane;


        if (gameManager.gameActive) // Game unpaused
        {
            
            if (!chargeAvailable || Random.Range(0f, 100f) > (20 * Time.deltaTime) && !preppingCharge) // chance of beginning charge routine if available, during which ordinary AI behaviour will not activate
            {

                bossAnim.speed = 1;
                if (playerDirection.magnitude > 10)
                {
                    bossAnim.SetBool("Tracking", true);
                    transform.LookAt(playerLocationPlane);
                    transform.Translate(-playerDirection.normalized * Time.deltaTime * 4, Space.World);
                }
                else
                {
                    if (bossAnim.GetBool("Attack"))
                    {
                        transform.LookAt(playerLocationPlane);
                    }
                    else
                    {
                        bossAnim.SetBool("Tracking", false);
                        bossAnim.SetTrigger("Attack");
                        bossAudio.pitch = 0.4f;
                        bossAudio.volume = 1;
                        bossAudio.PlayOneShot(creatureNoises[Random.Range(1, 3)]);
                        armHitbox.enabled = true;
                    }
                }
            }

            else // Charge routine
            {
                if (!preppingCharge && !charging)
                {
                    StartCoroutine(ChargeTime());
                    preppingCharge = true;
                    bossAnim.SetBool("Tracking", false);
                    armHitbox.enabled = false;
                }
                else if (preppingCharge && !charging && !chargeReady)
                {
                    transform.LookAt(playerLocationPlane);
                }
                else if (chargeReady && !charging)
                {
                    charging = true;
                    bossAudio.pitch = 0.4f;
                    bossAudio.volume = 1;
                    bossAudio.PlayOneShot(creatureNoises[Random.Range(1, 3)]);
                    StartCoroutine(ChargeDuration());
                }
                else if (charging)
                {
                    transform.Translate(Vector3.forward * chargeSpeed * Time.deltaTime);
                    bossAnim.speed = 4;
                }
                else
                {
                    Debug.Log("charge logic error");
                }
            }
        }
        // Game paused
        else
        {
            bossAnim.speed = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" && charging)
        {
            gameManager.WoundPlayer();
        }
    }


    // CHARGE COROUTINES

    // Time to prepare charge
    IEnumerator ChargeTime()
    {
        yield return new WaitForSeconds(chargeTime);
        chargeReady = true;
    }

    // Duration of charge
    IEnumerator ChargeDuration()
    {
        yield return new WaitForSeconds(1.5f);
        charging = false;
        preppingCharge = false;
        chargeReady = false;
        chargeAvailable = false;
        bossAnim.speed = 1;
        StartCoroutine(ChargeCooldown());
    }

    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(Random.Range(6, 12));
        chargeAvailable = true;
    }


    // AUDIO EVENTS

    // Plays sound when boss attack hits the ground
    public void AttackLandEvent()
    {
        bossAudio.PlayOneShot(creatureNoises[0]);
    }

    public void HurtBoss()
    {
        // 40% chance boss lets out cry on being hit by player
        if (Random.Range(0, 100) > 60)
        {
            bossAudio.pitch = 0.6f;
            bossAudio.PlayOneShot(creatureNoises[Random.Range(1, 3)]);
        }
    }


    // Boss death routine
    public void BossDeath()
    {
        alive = false;
        bossAnim.SetTrigger("Attack");
        bossAnim.SetBool("Tracking", false);
        bossAudio.pitch = 0.6f;
        bossAudio.PlayOneShot(creatureNoises[Random.Range(1, 3)]);
        falling = true;
    }
}
