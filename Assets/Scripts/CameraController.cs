using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float inputMouseVertical;
    public float turnSpeedVertical;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        inputMouseVertical = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.left, inputMouseVertical * turnSpeedVertical);
    }
}
