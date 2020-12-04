using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPhysics : MonoBehaviour
{
    public bool isColliding;
    public EnemyController targetHit;
    GameObject activePortal;
    Transform playerObj;
    float directionCheck;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = transform.root;
        targetHit = null;
        directionCheck = 0;
        /*
        to-do: 
        use raycasting from 3DSurvival for reticle
        use raycasting/normals to "spawn" teleporters from a list of 2 (teleport them
        based on right/left click)
        make the axe colliding with a wall summon the teleporter
        get the character to turn left/right and have the camera change based on it

        change the direction of the player when they go through a teleporter in OnTriggerEnter

        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        

        if(col.gameObject.GetComponent<StopMovement>() != null && transform.parent==null)
        {
            isColliding = true;
            Debug.Log("Collision Axe Test");
            targetHit = null;
            /*
            activePortal.transform.position = transform.position + (playerObj.transform.forward*-1) + new Vector3(0, -1f, 0);
            activePortal.transform.LookAt(playerObj);
            activePortal.transform.Rotate(0, activePortal.transform.rotation.y + 180, 0);
            */
        }

        if (col.gameObject.GetComponent<PortalSurface>() != null && transform.parent == null)
        {
            //float snapAngle = 0;
            //float iterator = 0;

            activePortal.transform.position = transform.position;
            activePortal.transform.LookAt(playerObj);
            activePortal.transform.rotation = Quaternion.Euler(0,SnapToValue(activePortal.transform.eulerAngles.y),0);
            activePortal.transform.Rotate(0, 180 + col.transform.eulerAngles.y, 0);
            activePortal.transform.position += (activePortal.transform.forward*-1);
        }

        
        if (col.gameObject.GetComponent<EnemyController>() != null)
        {
            isColliding = true;
            Debug.Log("Collision Axe Test");
            targetHit = col.gameObject.GetComponent<EnemyController>();
            targetHit.StopMovement();
        }

    }

    public void SetColliding(bool value)
    {
        isColliding = value;
    }

    public void ResetTargetMove()
    {
        if (targetHit!=null && targetHit.GetComponent<EnemyController>() != null)
        {
            targetHit.ResetMovement();
        }
    }

    public void SetPortalSpawn(GameObject portal)
    {
        activePortal = portal;
    }

    public float SnapToValue(float checkValue)
    {
        float result = 0;

        
        while (checkValue < 0)
            checkValue += 360;

        while (checkValue > 359)
            checkValue -= 360;
        


        if (checkValue > 45 && checkValue < 135)
            result = 90;
        else
            if (checkValue > 135 && checkValue < 225)
            result = 180;
        else
            if (checkValue > 225 && checkValue < 315)
            result = 270;
        else
            result = 0;


        return result;
    }
}
