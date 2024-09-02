using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Vector3 startingPoint;

    private Vector3 target;

    [SerializeField]
    private LayerMask zombieMask;

    private bool isPiercing;

    private float bulletDamage;

    private float bulletLife;

    public void SetAttributes(bool piercing, float damage, float lifeSpan)
    {
        isPiercing = piercing;
        bulletDamage = damage;
        bulletLife = lifeSpan;
    }

    public void Start()
    {
        startingPoint = transform.position;
        Destroy(gameObject, bulletLife);
    }

    public void SetDestination(Vector3 end)
    {
        target = end;
    }

    public void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<Enemy_Behaviour>() != null)
        {
            collision.gameObject.GetComponent<Enemy_Behaviour>().ReduceHealth((int)bulletDamage);

            if(!isPiercing)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
