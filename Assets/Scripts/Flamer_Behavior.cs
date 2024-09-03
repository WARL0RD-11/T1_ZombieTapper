using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamer_Behavior : MonoBehaviour
{

    private int damage;

    //Every zombie that enters the trigger box gets hit by the flamethrower and gets burnt to death
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Still check that it's truly a zombie is overlapping with the trigger
        //Better safe than sorry
        if (collision.GetComponent<Enemy_Behaviour>() != null)
        {
            //Do damage
            collision.gameObject.GetComponent<Enemy_Behaviour>().ReduceHealth(damage);
        }
    }

    //Set the damage that the flamer does
    //Called in Soldier_Behavior.cs Start() function
    //In the S_B serialize fields
    public void SetFlamerDamage(int amount)
    {
        damage = amount;
    }
}
