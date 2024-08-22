using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    [SerializeField] public KeyCode PDKey = KeyCode.Space;
    [SerializeField] public KeyCode UpKeyPressed = KeyCode.W;
    [SerializeField] public KeyCode DownKeyPressed = KeyCode.S;

    public bool HasItem;

    //Variable that holds the current delivery item picked up by the player
    //Carter 
    private DeliveryItem currentItem;
    private GameObject CurrentSB;
    private GameObject CurrentSoldier;

    [SerializeField]
    private LayerMask supplyMask;

    [SerializeField]
    private LayerMask soldierMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Package"))
        {
            //Debug.Log("Collision with Package"); //Successful Log
            //CurrentSB = collision.gameObject.GetComponent<SupplyBox_Behavior>();
            //Debug.Log(CurrentSB.GetSupplyItem().itemName);
        }
    }

    //This Function Checks if the Player Overlaps with the Package and then Presses SPACE. 
    //Creating the Pickup mechanic
    void Delivery()
    {
        if (Input.GetKeyDown(PDKey))
        {
            Debug.Log("SPACE Called");
            if (HasItem == true && currentItem)
            {
                //RayCast to Soldier
                //Code for giving it to the detected soldier
                Debug.Log("Already Has Item");
                RemoveDeliveryItem(); 
            }
            else if (HasItem == false && !currentItem)
            {
                SetDeliveryItem(CurrentSB.GetComponent<SupplyBox_Behavior>().GetSupplyItem());
                HasItem = true;
                Debug.Log("Item Picked");
            }

        }
    }

    void PlayerMovement()
    {
        float GridSize = 2.0f;
        Vector3 NewPos;

        if (Input.GetKeyDown(DownKeyPressed))
        {
            
            NewPos = transform.position - new Vector3(0, GridSize, 0);
            NewPos.y = Mathf.Clamp(NewPos.y, -4, GridSize);
            transform.position = NewPos;
            RaycastDetections();
        }

        if (Input.GetKeyDown(UpKeyPressed))
        {
            NewPos = transform.position + new Vector3(0, GridSize, 0);
            NewPos.y = Mathf.Clamp(NewPos.y, -4, GridSize);
            transform.position = NewPos;

            RaycastDetections();
        }

    }

    private void RaycastDetections()
    {
        RaycastHit2D SoldierDetect = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), 100, soldierMask);

        RaycastHit2D SupplyDetect = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 100, supplyMask);
        //Debug.Log("hello world");
        if (SoldierDetect.collider)
        {
            CurrentSoldier = SoldierDetect.collider.gameObject;
            Debug.Log("SoliderHIT");
        }
        if (SupplyDetect.collider)
        {
            CurrentSB = SupplyDetect.collider.gameObject;
            Debug.Log("BOXHIT");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HasItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Delivery();
    }

    private void SetDeliveryItem(DeliveryItem newItem)
    {
        currentItem = newItem;
    }

    private void RemoveDeliveryItem()
    {
        currentItem = null;
    }
}
