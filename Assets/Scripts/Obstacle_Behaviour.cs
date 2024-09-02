using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour
{
    [SerializeField] public int maxHealth = 10;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public int TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            GetComponent<Animator>().SetTrigger("isDamaged");
        }
        else
        {
            GetComponent<Animator>().ResetTrigger("isDamaged");
        }
        return currentHealth;
    }

    public void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
