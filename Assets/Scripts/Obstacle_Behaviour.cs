using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour
{
    [SerializeField] int health = 10;

    public int TakeDamage(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            Destroy(gameObject);
        }
        return health;
    }
}
