using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPhysics : MonoBehaviour
{
    public bool isColliding;
    public EnemyController targetHit;
    
    // Start is called before the first frame update
    void Start()
    {
        targetHit = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        

        if(col.gameObject.GetComponent<StopMovement>() != null)
        {
            isColliding = true;
            Debug.Log("Collision Axe Test");
            targetHit = null;
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
}
