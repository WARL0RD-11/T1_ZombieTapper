using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour
{
    [SerializeField] public int health = 10;


    public int TakeDamage(int damage)
    {
        health -= damage;
        GetComponent<Animator>().SetTrigger("isDamaged");
        return health;
    }

    public void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
