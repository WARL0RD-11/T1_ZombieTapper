using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Vector3 startingPoint;

    private Vector3 target;

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
            Destroy(this.gameObject);
        }
    }
}
