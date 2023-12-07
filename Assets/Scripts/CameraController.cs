using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float inputMouseVertical;
    public float turnSpeedVertical;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotates the camera up and down based on mouse input, possible avenue for refactoring by incorporating into main player controller script
        if (gameManager.gameActive)
        {
            inputMouseVertical = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.left, inputMouseVertical * turnSpeedVertical);
        }
    }
}
