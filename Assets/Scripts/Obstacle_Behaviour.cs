using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour
{
    [SerializeField] public int maxHealth = 10;
    private int currentHealth;

    [SerializeField]
    private GameObject warningPrefab;

    [SerializeField]
    private float warningDistance;

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

        Vector3 temp = transform.position;
        temp.x = transform.position.x + warningDistance;

        Instantiate(warningPrefab, transform.position, Quaternion.identity);
        Instantiate(warningPrefab, temp, Quaternion.identity);

        Destroy(gameObject);
    }
}
