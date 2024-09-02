using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public enum Weapon { Rifle, Shotgun, Flamer, Sniper };

public enum SoldierState { Idle, Firing, Waiting };

public class SoldierBehavior : MonoBehaviour
{

    private SoldierState currentSState;

    private Weapon currentWeapon;

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

    private Animator animator;

    //Now that soldiers shoot over a duration
    //Give them a starting ammo count
    [SerializeField]
    private int maximumAmmo;

    private int currentAmmo;

    private int currentSPAmmo;

    private bool canShoot;

    private bool canShotgun;

    [SerializeField]
    private float rifleFireRate;

    [SerializeField]
    private float sgFireRate;

    [SerializeField]
    private float sgSpread;

    private MuzzleFlash_Behavior mfB;
    //private Animation mfF;

    [SerializeField]
    private GameObject bulletTrail;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private GameObject flamerBox;

    [SerializeField]
    private float flamerDuration;
    private bool flamerInProgress;

    [SerializeField]
    private ParticleSystem flamerVFX;

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
        canShotgun = true;
        flamerInProgress = false;

        currentWeapon = Weapon.Rifle;
        currentSState = SoldierState.Idle;

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

            if (currentSState != SoldierState.Waiting)
            {
                WeaponStateBehavior();
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

            detectedZombie = null;

            //Doesn't see a zombie so nothing should happen at the moment
        }

        //}
    }

    //Called when the soldier wants a new item to be delivered
    private void WantsNewItem()
    {

        currentSState = SoldierState.Waiting;

        animator.SetBool("isAsking", true);

        //Get a random delivery item from the game manager
        //wantedItem = gM.GetRandomDeliveryItem();
        wantedItem = gM.GetDeliveryItems()[0];

        Debug.Log("Soldier wants " + wantedItem.itemName);

        currentSpeechBubble = Instantiate(speechBubblePrefab, bubbleCoords.transform.position, Quaternion.identity);
        currentSpeechBubble.GetComponent<DesiredItemBubble>().UpdateItemSprite(wantedItem.itemSprite);

        //Set waitingForItem to true so that its not constantly asking for a new one
        waitingForItem = true;

    }

    //Called by the player when they attempt to give the soldier an item
    public void DeliverItem(DeliveryItem item)
    {

        //If the item is the correct item
        if (item == wantedItem)
        {

            animator.SetBool("isAsking", false);

            currentAmmo = maximumAmmo;

            waitingForItem = false;

            gM.AddScore(1);

            //Get rid of the current speech bubble
            Destroy(currentSpeechBubble);
            currentSpeechBubble = null;

            currentWeapon = Weapon.Rifle;

            animator.ResetTrigger("hasShotgun");
            animator.ResetTrigger("hasSniper");
            animator.ResetTrigger("hasFlamer");
        }
        else
        {
            Debug.Log(item.weapon.ToString());
            currentWeapon = item.weapon;
            currentSPAmmo = 3;
        }

        currentSState = SoldierState.Idle;

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
        yield return new WaitForSeconds(rifleFireRate);
        canShoot = true;
    }

    private IEnumerator ShotgunCooldown()
    {
        yield return new WaitForSeconds(sgFireRate);
        canShotgun = true;
    }

    public void SetFireFireRate(float newRate)
    {
        rifleFireRate = newRate;
    }

    public void ShootVisual()
    {

        mfB.PlayAnimation();

        GameObject tempTrail = Instantiate(bulletTrail, mfB.transform.position, Quaternion.identity);
        tempTrail.GetComponent<BulletBehavior>().SetAttributes(false, 1.0f,2.0f);
        tempTrail.GetComponent<Rigidbody2D>().velocity = Vector3.left * bulletSpeed;
        tempTrail.GetComponent<BulletBehavior>().SetDestination(detectedZombie.transform.position);

    }

    private void WeaponStateBehavior()
    {
        switch (currentWeapon)
        {
            case Weapon.Rifle:

                //Debug.Log("Rifle behavior");
                RifleBehavior();
                break;

            case Weapon.Flamer:

                //Debug.Log("Flamer behavior");
                FlamerBehavior();
                break;

            case Weapon.Sniper:

                //Debug.Log("Sniper behavior");
                SniperBehavior();
                break;

            case Weapon.Shotgun:

                //Debug.Log("Shotgun behavior");
                ShotgunBehavior();
                break;

            default:
                break;
        }


    }

    private void RifleBehavior()
    {
        animator.SetBool("isShooting", true);
        animator.SetBool("isAsking", false);
        if (canShoot)
        {
            canShoot = false;
            currentAmmo--;
            if (currentAmmo <= 0)
            {

                Debug.Log("Out of ammo");

                wantedItem = gM.GetDeliveryItems()[0];
                canShoot = false;
                waitingForItem = true;
                animator.SetBool("isShooting", false);
                animator.SetBool("isAsking", true);

                WantsNewItem();
            }
            else
            {
                StartCoroutine(ShootCooldown());
            }
        }
    }

    private void ShotgunBehavior()
    {
        animator.SetBool("isShooting", false);
        animator.SetBool("isAsking", false);
        animator.SetTrigger("hasShotgun");

        if (canShotgun)
        {
            canShotgun = false;
            currentSPAmmo--;
            if (currentSPAmmo <= 0)
            {
                ReturnToRifle();
            }
            else
            {
                StartCoroutine(ShotgunCooldown());
            }
        }
    }

    private void SniperBehavior()
    {
        animator.SetBool("isShooting", true);
        animator.SetBool("isAsking", false);
        animator.SetTrigger("hasSniper");

        canShoot = false;


    }

    private void FlamerBehavior()
    {
        animator.SetBool("isShooting", false);
        animator.SetBool("isAsking", false);

        canShoot = false;

        if (!flamerInProgress)
        {
            flamerBox.SetActive(true);
            flamerInProgress = true;
            StartCoroutine(FlamerDuration());
            flamerVFX.Play();
        }
    }

    private void ReturnToRifle()
    {

        animator.ResetTrigger("hasShotgun");
        animator.ResetTrigger("hasSniper");
        animator.ResetTrigger("hasFlamer");
        canShoot = false;
        currentWeapon = Weapon.Rifle;
        WantsNewItem();
    }

    public void ShotgunAttack()
    {
        mfB.PlayAnimation();
        Vector3 initialDirection = Vector3.left;
        initialDirection.y = sgSpread;

        for (int i = 0; i < 6; i++)
        {
            GameObject tempBullet = Instantiate(bulletTrail, mfB.transform.position, Quaternion.identity);

            Vector3 temp = initialDirection;

            temp.y = initialDirection.y - i * (sgSpread / 2.0f);

            tempBullet.GetComponent<Rigidbody2D>().velocity = temp * bulletSpeed;

            tempBullet.GetComponent<BulletBehavior>().SetAttributes(false, 7.0f,1.0f);


        }

    }

    public void SniperAttack()
    {
        mfB.PlayAnimation();

        GameObject tempTrail = Instantiate(bulletTrail, mfB.transform.position, Quaternion.identity);
        tempTrail.GetComponent<BulletBehavior>().SetAttributes(true, 100.0f, 5.0f);
        tempTrail.GetComponent<Rigidbody2D>().velocity = Vector3.left * bulletSpeed;
        tempTrail.GetComponent<BulletBehavior>().SetDestination(detectedZombie.transform.position);

    }

    private IEnumerator FlamerDuration()
    {
        yield return new WaitForSeconds(flamerDuration);
        flamerBox.SetActive(false);
        flamerInProgress = false;
        ReturnToRifle();
    }

}
