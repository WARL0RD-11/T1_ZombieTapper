using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Vector3 startingPoint;

    private Vector3 target;

    [SerializeField]
    private LayerMask zombieMask;

    public void Start()
    {
        startingPoint = transform.position;
        //Debug.Log(startingPoint);
    }

    public void SetDestination(Vector3 end)
    {
        target = end;
        //Debug.Log(target);
    }

    public void Update()
    {
        if(transform.position.x <= target.x)
        {
            //Destroy(this.gameObject);
        }
    }
    /*
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.layer == zombieMask)
        //{
            collision.gameObject.GetComponent<Enemy_Behaviour>().ReduceHealth();

            Destroy(gameObject); 
        //}
    }
    */

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy_Behaviour>())
        {
            collision.gameObject.GetComponent<Enemy_Behaviour>().ReduceHealth();
            Destroy(gameObject);
        }
    }
}
