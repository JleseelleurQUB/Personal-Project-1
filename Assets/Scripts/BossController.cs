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
    public Animator bossAnim;
    private GameManager gameManager;
    public AudioMixerGroup emphasise;
    public AudioMixerGroup master;
    private AudioSource bossAudio;
    public AudioClip creatureNoise1;
    public AudioClip creatureNoise2;
    public AudioClip hit;


    // Start is called before the first frame update
    void Start()
    {
        bossAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        bossAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // finds player direction and moves towards them
        transform.position = new Vector3(transform.position.x, -0.9f, transform.position.z);
        playerLocationPlane = player.transform.position;
        playerLocationPlane.y = 0;

        playerDirection = transform.position - playerLocationPlane;
      

        if (gameManager.gameActive)
        {
            bossAnim.speed = 1;
            if (playerDirection.magnitude > 10)
            {
                bossAnim.SetBool("Tracking", true);
                transform.LookAt(playerLocationPlane);
                transform.Translate(playerDirection.normalized * Time.deltaTime * 4);
            }
            else
            {
                bossAnim.SetBool("Tracking", false);
                bossAnim.SetTrigger("Attack");
                transform.LookAt(playerLocationPlane);
            }
        }
        else 
        {
            bossAnim.speed = 0;
        }

    }

    public void AttackLandEvent()
    {
        bossAudio.outputAudioMixerGroup = emphasise;
        bossAudio.clip = hit;
        bossAudio.time = 1.0f;
        bossAudio.Play();

        if (bossAudio.outputAudioMixerGroup.name == "Emphasise")
        {
            Debug.Log("receiving");
        }
        else
        { 
            Debug.Log("not receiving");
        }
    }

}
