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

    public GameObject harpoonPrefab;
    public Animator playerAnim;
    public Rigidbody playerRB;
    // Start is called before the first frame update
    void Start()
    {
      playerAnim = GetComponent<Animator>();
      playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //update inputs
        inputHorizontal = -Input.GetAxis("Horizontal");
        inputVertical = -Input.GetAxis("Vertical");
        inputMouseHorizontal = Input.GetAxis("Mouse X");

        

        // controls the players movement and horizontal turning through key and mouse inputs
        transform.Translate(Vector3.right * inputHorizontal * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * inputVertical * Time.deltaTime * speed);
        transform.Rotate(Vector3.up, inputMouseHorizontal * turnSpeedHorizontal);

        // spawns projectile and starts cooldown when pressing space
        if (Input.GetKeyDown(KeyCode.Space) && harpoonAvailable)
        {
            SpawnHarpoon();
            StartCoroutine(HarpoonRecharge());
        }


        // animation
        playerAnim.SetFloat("Horizontal Speed", inputHorizontal);
        playerAnim.SetFloat("Forward Speed", -inputVertical);
        
        if(playerRB.velocity.magnitude > 0)
        {
            playerAnim.SetBool("Idle", false);
        }
        else
        {
            playerAnim.SetBool("Idle", true);
        }
    }

    IEnumerator HarpoonRecharge()
    {
        yield return new WaitForSeconds(harpoonRecharge);
        harpoonAvailable = true;
    }

    void SpawnHarpoon()
    {
        Instantiate(harpoonPrefab, transform.position, transform.rotation);
        harpoonAvailable = false;
    }

}
