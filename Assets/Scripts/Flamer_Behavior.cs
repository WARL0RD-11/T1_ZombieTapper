using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamer_Behavior : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy_Behaviour>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
