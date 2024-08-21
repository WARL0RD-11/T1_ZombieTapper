using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehavior : MonoBehaviour
{
    //Private components needed by the soldier
    private BoxCollider2D bC2D;
    private SpriteRenderer spRend;

    //Bool to manage if the guard can be delivered to.
    //Delivering the wrong item to the guard means they are stunned
    private bool isStunned;

    //The duration that the guard is stunned if the wrong delivery is given
    private float stunDuration;

    //Points to the game manager in the scene
    private GameManager gM;

    private DeliveryItem wantedItem;

    private bool waitingForItem;

    private Color tempColor;

    [SerializeField]
    private LayerMask zombieMask;

    void Start()
    {
        bC2D = GetComponent<BoxCollider2D>();
        spRend = GetComponent<SpriteRenderer>();
        gM = FindObjectOfType<GameManager>();

        stunDuration = gM.GetDelayPenalty();

        waitingForItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), Mathf.Infinity, zombieMask);
        if (hit.collider!=null)
        {
            tempColor = Color.red;
            //Debug.Log("Soldier deteced a zombie");
            Debug.DrawLine(transform.position, hit.transform.position, tempColor);
            if (!waitingForItem)
            {
                WantsNewItem();
            }

        }
        else
        {
            tempColor = Color.green;
            //Debug.Log("Soldier cannot see a zombie");
            Debug.DrawLine(transform.position, Vector3.left * 100, tempColor);
        }


    }

    //Called when the soldier wants a new item to be delivered
    private void WantsNewItem()
    {
        wantedItem = gM.GetRandomDeliveryItem();

        Debug.Log("Soldier wants " + wantedItem.itemName);
    }
}
