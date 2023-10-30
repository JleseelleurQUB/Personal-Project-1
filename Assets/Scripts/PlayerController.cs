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
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //update inputs
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
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
