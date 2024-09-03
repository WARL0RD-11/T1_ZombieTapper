using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamer_Behavior : MonoBehaviour
{

    private int damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy_Behaviour>() != null)
        {
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<Enemy_Behaviour>().ReduceHealth(50);
        }
    }

    public void SetFlamerDamage(int amount)
    {
        damage = amount;
    }
}
