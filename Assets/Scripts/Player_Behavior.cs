using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    //[SerializeField]
    //private GameObject Package; 
    public bool CanDeliver = false;

    //Variable that holds the current delivery item picked up by the player
    //Carter 
    private DeliveryItem currentItem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.CompareTag("Package"))
        {
            CanDeliver = true;  
        }
        */
    }

    //This Function Checks if the Player Overlaps with the Package and then Presses SPACE. 
    //Creating the Pickup mechanic
    void Delivery()
    {
        KeyCode PDKey = KeyCode.Space;
        if (Input.GetKeyDown(PDKey))
        {
            Debug.Log("SPACE Pressed");
            if (CanDeliver == true && currentItem)
            {
                //Debug.Log("Destruction Initiated");
                //Destroy(Package);
                //gameObject.tag = "HasPackage";

                //Code for giving it to the detected soldier

                RemoveDeliveryItem();
            }
            else
            {
                //use SetDeliveryItem on the current supply box

      
            }

        }
    }

    void PlayerMovement()
    {
        float GridSize = 2.0f;
        KeyCode DownKeyPressed = KeyCode.S;
        KeyCode UpKeyPressed = KeyCode.W;
        Vector3 NewPos;

        if (Input.GetKeyDown(DownKeyPressed))
        {
            
            NewPos = transform.position - new Vector3(0, GridSize, 0);
            NewPos.y = Mathf.Clamp(NewPos.y, -4, GridSize);
            transform.position = NewPos;
        }

        if (Input.GetKeyDown(UpKeyPressed))
        {
            NewPos = transform.position + new Vector3(0, GridSize, 0);
            NewPos.y = Mathf.Clamp(NewPos.y, -4, GridSize);
            transform.position = NewPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
