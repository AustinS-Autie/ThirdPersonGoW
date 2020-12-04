using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 0;
    [SerializeField]
    Transform tiltUpDown;
    
    [SerializeField]
    int tiltMaxX = 0;
    [SerializeField]
    int tiltMaxY = 0;

    [SerializeField]
    GameObject currentPlayer;

    Transform playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = currentPlayer.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.lockState = CursorLockMode.Confined;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");


        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y+2, playerTransform.position.z-4);

        transform.RotateAround(currentPlayer.transform.position, transform.up, x*rotSpeed);
        transform.Rotate(new Vector3(0, x * rotSpeed, 0));
        tiltUpDown.Rotate(new Vector3(y * rotSpeed, 0, 0));

    }
}
