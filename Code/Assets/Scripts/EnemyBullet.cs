using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    float bulletSpeed;

    GameObject target;

    Transform targetTransform;

    [SerializeField]
    float bulletLifetime;

    float elapsed = 0;

    Vector3 bulletPath;

    float seekingValue = 1;


    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
        bulletPath = new Vector3(targetTransform.position.x, targetTransform.position.y+0.5f,
        targetTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        seekingValue -= 1 * Time.deltaTime;

        if(seekingValue<0)
        {
            ResetTarget();
            seekingValue = 1;
        }


        transform.position = Vector3.MoveTowards(
        transform.position, bulletPath, bulletSpeed * Time.deltaTime);


        elapsed += Time.deltaTime;

        if (elapsed >= bulletLifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletSpeed(float val)
    {
        bulletSpeed = val;
    }

    public void SetTarget(GameObject val)
    {
        target = val;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Portal>() != null)
            ResetTarget();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SwordPhysics>() != null)
            Destroy(gameObject);
    }


    public void ResetTarget()
    {
        targetTransform = target.GetComponent<Transform>();
        bulletPath = new Vector3(targetTransform.position.x, targetTransform.position.y + 0.5f,
        targetTransform.position.z);
    }

}
