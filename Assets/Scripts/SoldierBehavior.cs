using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class SoldierBehavior : MonoBehaviour
{
    //Private components needed by the soldier
    private BoxCollider2D bC2D;
    private SpriteRenderer spRend;

    //Bool to manage if the guard can be delivered to.
    //Delivering the wrong item to the guard means they are stunned
    //private bool isStunned;

    //The duration that the guard is stunned if the wrong delivery is given
    private float stunDuration;

    //Points to the game manager in the scene
    private GameManager gM;

    //The SO Delivery item that the soldier currently wants
    private DeliveryItem wantedItem;

    //Bool that changes depeneding on if the solider needs an item or not
    private bool waitingForItem;

    //Color of the debug line trace used for testing
    //No functionality in final project
    private Color tempColor;

    //Layermask for the raycast2D to only see zombies to prevent funky interactions
    [SerializeField]
    private LayerMask zombieMask;

    [SerializeField]
    private GameObject speechBubblePrefab;

    private GameObject currentSpeechBubble;

    [SerializeField]
    private GameObject bubbleCoords;

    private float linecastDistance;

    private Enemy_Behaviour detectedZombie;

    //Index of animations
    //0 -> idle
    //1 -> asking
    //2 -> shooting
    private Animator animator;

    //private bool isShooting;

    //Now that soldiers shoot over a duration
    //Give them a starting ammo count
    [SerializeField]
    private int maximumAmmo;

    private int currentAmmo;

    private bool canShoot;

    [SerializeField]
    private float fireRate;

    private MuzzleFlash_Behavior mfB;
    //private Animation mfF;

    [SerializeField]
    private GameObject bulletTrail;

    [SerializeField]
    private float bulletSpeed;

    //Audio
    AudioManager audioManager;
    private int GuntypeSound;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        //Gets all the components referenced in the code
        bC2D = GetComponent<BoxCollider2D>();
        spRend = GetComponent<SpriteRenderer>();
        gM = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();

        mfB = GetComponentInChildren<MuzzleFlash_Behavior>();

        //mfF = GetComponentInChildren<Animation>();
          
        //Gets the stun duration that will be suffered from the game manager
        stunDuration = gM.GetDelayPenalty();

        //Initializes waitingForItem to false
        waitingForItem = false;

        linecastDistance = gM.GetLinecastDistance();

        //isShooting = false;

        currentAmmo = maximumAmmo;

        canShoot = true;

        //Test code for stun functionality
        //BecomeStunned();

        //Change the following line if complete.
        //GuntypeSound = 0;
        GuntypeSound = UnityEngine.Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {

        //Only looks for a zombie if the soldier is not currently stunned
        //if (!isStunned)
        //{

        //Send out a 2D raycast and store the hit result (if it exists) in *hit*
        //Only detects colliders on the Zombie layer to prevent clipping with anything else
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), linecastDistance, zombieMask);
        //If hit has a result
        if (hit.collider != null)
        {
            //then set the debug line color to red and end it on the position
            tempColor = Color.red;
            //Debug.Log("Soldier deteced a zombie");
            Debug.DrawLine(transform.position, hit.transform.position, tempColor);

            detectedZombie = hit.collider.gameObject.GetComponent<Enemy_Behaviour>();

            if (currentAmmo > 0 && canShoot)
            {
                //If this is the first time they've detected a zombie
                /*if (!waitingForItem)
                {
                    //Start wanting a new item
                    WantsNewItem();

                    //Set the target of the soldier to the enemy that triggered the check
                    detectedZombie = hit.collider.gameObject.GetComponent<Enemy_Behaviour>();
                }
                */

                animator.SetBool("isShooting", true);
                animator.SetBool("isAsking", false);

                mfB.PlayAnimation();
                //mfF.Play();

                GameObject tempTrail = Instantiate(bulletTrail, mfB.transform.position, Quaternion.identity);
                tempTrail.GetComponent<Rigidbody2D>().velocity = Vector3.left * bulletSpeed * Time.deltaTime;
                tempTrail.GetComponent<BulletBehavior>().SetDestination(detectedZombie.transform.position);

                //DO DAMAGE TO THE ZOMBIE
                canShoot = false;

                detectedZombie.ReduceHealth();
                detectedZombie = null;

                StartCoroutine(ShootCooldown());

                //Play Audio
                if (GuntypeSound == 1)
                {
                    //Rifle
                    audioManager.PlaySFX(audioManager.Rifle_Audio);
                } else if (GuntypeSound == 2){
                    //Shotgun
                    audioManager.PlaySFX(audioManager.Shotgun_Audio);
                } else if (GuntypeSound == 3){
                    //Pistol
                    audioManager.PlaySFX(audioManager.Pistol_Audio);
                } else if (GuntypeSound == 4){
                    //Sniper
                    audioManager.PlaySFX(audioManager.Sniper_Audio);
                }

                currentAmmo--;
                if(currentAmmo <= 0 && !waitingForItem)
                {
                    animator.SetBool("isShooting", false);
                    animator.SetBool("isAsking", true);

                    WantsNewItem();
                    //Need Change the Following Line: Assign corresponding SoundClip according to gun type
                    GuntypeSound = UnityEngine.Random.Range(1, 5);
                    audioManager.PlaySFX(audioManager.AskWeapon_Audio);
                }

            }

        }
        //If hit does not have a result
        else
        {
            //then set the debug line color to green and go off screen
            tempColor = Color.green;
            //Debug.Log("Soldier cannot see a zombie");
            Debug.DrawLine(transform.position, transform.position - Vector3.left * -linecastDistance, tempColor);

            animator.SetInteger("animIndex", 0);

            animator.SetBool("isShooting", false);

            //Doesn't see a zombie so nothing should happen at the moment
        }

        //}
    }

    //Called when the soldier wants a new item to be delivered
    private void WantsNewItem()
    {

        animator.SetBool("isAsking", true);

        //Get a random delivery item from the game manager
        wantedItem = gM.GetRandomDeliveryItem();

        Debug.Log("Soldier wants " + wantedItem.itemName);

        currentSpeechBubble = Instantiate(speechBubblePrefab, bubbleCoords.transform.position, Quaternion.identity);
        currentSpeechBubble.GetComponent<DesiredItemBubble>().UpdateItemSprite(wantedItem.itemSprite);

        //Set waitingForItem to true so that its not constantly asking for a new one
        waitingForItem = true;

        //animator.SetBool("isAsking", true);
        /*
        if (!isShooting)
        {
            animator.SetInteger("animIndex", 1);
        }
        */
    }

    //Called by the player when they attempt to give the soldier an item
    public void DeliverItem(DeliveryItem item)
    {
        //animator.SetBool("isAsking", false);
        //Get rid of the current speech bubble
        Destroy(currentSpeechBubble);
        currentSpeechBubble = null;

        //If the item is the correct item
        if (item == wantedItem)
        {
            //Do something
            //Shoot the zombie
            //Make sure that detectedZombie actually has a value
            /*
            if (detectedZombie)
            {
                //Kill the zombie
                detectedZombie.OnDeath();
                waitingForItem = false;
                //animator.SetBool("isShooting", true);
                if (!isShooting)
                {
                    //animator.SetInteger("animIndex", 2);
                    isShooting = true;
                }
            }
            //Give the player some points, subject to change
            */

            animator.SetBool("isAsking", false);

            currentAmmo = maximumAmmo;

            waitingForItem = false;

            gM.AddScore(1);
        }
        //If the item is the wrong item
        else
        {
            //No longer stun the soldier, decided that they would just do nothing
            //Leaving code in case we need it again later.
            //Stun the soldier
            //waitingForItem = false;
            //BecomeStunned();
        }
    }

    //Called when the player delivers the wrong item to the soldier
    private void BecomeStunned()
    {
        //Set isStunned to true to prevent detection and asking for a new item
        //isStunned = true;

        //Start the StunBehavior coroutine
        StartCoroutine(StunBehavior());

        Debug.Log("soldier has become stunned");

        animator.SetInteger("animIndex", 0);
    }

    //Part of stun behavior
    //Basically waits the duration specified in GameManager's penalty
    //Then recovers the soldier from stun to act as normal
    private IEnumerator StunBehavior()
    {
        yield return new WaitForSeconds(stunDuration);
        //isStunned = false;
        Debug.Log("soldier recovered from stun");
    }

    private void ShootingOver()
    {
        //isShooting = false;
        animator.SetInteger("animIndex", 0);
        waitingForItem = false;
        detectedZombie = null;
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void SetFireFireRate(float newRate)
    {
        fireRate = newRate;
    }
}
