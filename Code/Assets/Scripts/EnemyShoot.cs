using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    float cooldownTime;
    float cooldown;

    [SerializeField]
    float bulletSpeed;

    [SerializeField]
    GameObject enemyBullet;

    [SerializeField]
    GameObject playerTarget;

    float skewX;
    float skewY;
    float skewZ;

    int destroyed;


    // Start is called before the first frame update
    void Start()
    {
        cooldown = cooldownTime + Random.Range(-0.5f, 0.5f);
        destroyed = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (cooldown >= cooldownTime)
        {
            skewX = Random.Range(-0.1f, 0.1f);
            skewY = Random.Range(-0.1f, 0.1f);
            skewZ = Random.Range(-0.1f, 0.1f);

            GameObject b = Instantiate(enemyBullet, transform.position, transform.rotation);

            b.GetComponent<EnemyBullet>().SetBulletSpeed(bulletSpeed);
            if (playerTarget != null)
                b.GetComponent<EnemyBullet>().SetTarget(playerTarget);
            else
                b.transform.LookAt(transform.forward);
            
            cooldown = 0;
        }
        else
            cooldown += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

    }
}
