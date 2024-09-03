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

    //Function to act as psuedo constructor
    //Lets the object making a bullet set all the attributes of the bullet without having to do multiple setters
    //Descriptions should be self explanatory
    public void SetAttributes(bool piercing, float damage, float lifeSpan)
    {
        isPiercing = piercing;
        bulletDamage = damage;
        bulletLife = lifeSpan;
    }

    //When the bullet is created, set the lifespan to bulletLife so that they auto destroy if they somehow don't hit a zombie
    //Important for the sniper bullet since it won't actually destroy otherwise
    public void Start()
    {
        startingPoint = transform.position;
        Destroy(gameObject, bulletLife);
    }

    //Deprecated function
    //Back when bullet logic was purely visual the bullet despawned once it reached it's destination
    //Now bullets actually do something so this is completely useless
    public void SetDestination(Vector3 end)
    {
        target = end;
    }

    //Bullet uses the Zombie physics layer so that it can only get zombie colliders
    //Handles the behavior of "hitting" a zombie
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Actually make sure that it's a zombie, even though it's the zombie layer
        //Better safe than sorry
        if (collision.gameObject.GetComponent<Enemy_Behaviour>() != null)
        {
            //Call the ReduceHealth() function on E_B that the zombie has
            collision.gameObject.GetComponent<Enemy_Behaviour>().ReduceHealth((int)bulletDamage);

            //If the bullet isn't piercing this is the end of the line for it
            if(!isPiercing)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
