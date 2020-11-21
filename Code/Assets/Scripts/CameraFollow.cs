using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.


    Vector3 offset;                     // The initial offset from the target.


    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;
    }


    void FixedUpdate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        

        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position + offset;

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(target.position, transform.up, x * 5);
            targetCamPos = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z + 1);
        }
        else
            transform.rotation = transform.parent.rotation;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}