using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    [SerializeField] public KeyCode PDKey = KeyCode.Return;
    [SerializeField] public KeyCode UpKeyPressed = KeyCode.UpArrow;
    [SerializeField] public KeyCode DownKeyPressed = KeyCode.DownArrow;

    public bool HasItem;

    //Variable that holds the current delivery item picked up by the player
    //Carter 
    private DeliveryItem currentItem;
    private GameObject CurrentSB;
    private GameObject CurrentSoldier;
    private GameManager GameManager;

    [SerializeField]
    private LayerMask supplyMask;

    [SerializeField]
    private LayerMask soldierMask;

    private Animator animator;
    private TurretBehavior TB;

    [SerializeField]
    private SpriteRenderer heldItemSprite;

    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    //This Function Checks if the Player Overlaps with the Package and then Presses SPACE. 
    //Creating the Pickup mechanic
    void Delivery()
    {
        if (Input.GetKeyDown(PDKey))
        {
            //Debug.Log("SPACE Called");
            if (HasItem == true && currentItem)
            {
                //RayCast to Soldier
                //Code for giving it to the detected soldier
                //Debug.Log("Already Has Item");

                //Carter
                //Make sure that CurrentSoldier is actually set to something
                if (CurrentSoldier)
                {
                    //Then get its SB script and try to deliver the current item, whatever it is.
                    CurrentSoldier.GetComponent<SoldierBehavior>().DeliverItem(currentItem);
                }
                audioManager.PlaySFX(audioManager.Deliver_Audio);
                RemoveDeliveryItem(); 
                HasItem = false;
                //Debug.Log("Item Delivered");

            }
            else if (HasItem == false && !currentItem)
            {
                //Carter
                //Make sure that CurrentSB is actually set to something
                if (CurrentSB)
                {
                    audioManager.PlaySFX(audioManager.pickup_Audio);
                    //Then set the player's current delivery item to the supply item from the box
                    SetDeliveryItem(CurrentSB.GetComponent<SupplyBox_Behavior>().GetSupplyItem());
                    CurrentSB.GetComponent<SupplyBox_Behavior>().TakeItem();
                }
                    HasItem = true;
               // Debug.Log("Item Picked");
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

            animator.SetBool("isUp", false);
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetKeyDown(UpKeyPressed))
        {
            NewPos = transform.position + new Vector3(0, GridSize, 0);
            NewPos.y = Mathf.Clamp(NewPos.y, -4, GridSize);
            transform.position = NewPos;

            RaycastDetections();

            animator.SetBool("isUp", true);
            animator.SetBool("isMoving", true);
        }

        animator.SetBool("isMoving", false);

    }

    private void RaycastDetections()
    {
        RaycastHit2D SoldierDetect = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), 100, soldierMask);

        RaycastHit2D SupplyDetect = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 100, supplyMask);

        if (SoldierDetect.collider)
        {
            CurrentSoldier = SoldierDetect.collider.gameObject;
            //Debug.Log("SoliderHIT");
        }
        if (SupplyDetect.collider)
        {
            CurrentSB = SupplyDetect.collider.gameObject;
            //Debug.Log("BOXHIT");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        HasItem = false;
        RaycastDetections();

        animator = GetComponent<Animator>();

        //heldItemSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetGameStatus())
        { PlayerMovement();
            Delivery();
        }
    }

    private void SetDeliveryItem(DeliveryItem newItem)
    {
        currentItem = newItem;
        heldItemSprite.sprite = currentItem.itemSprite;
    }

    private void RemoveDeliveryItem()
    {
        currentItem = null;
        heldItemSprite.sprite = null;
    }
}
