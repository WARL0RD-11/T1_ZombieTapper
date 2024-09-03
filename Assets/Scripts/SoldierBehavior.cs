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

    private GameObject currentSpeechBubble;

    private float linecastDistance;

    private Enemy_Behaviour detectedZombie;

    private Animator animator;

    private int currentAmmo;

    private int currentSPAmmo;

    private bool canShoot;

    private bool canShotgun;

    private MuzzleFlash_Behavior mfB;
    //private Animation mfF;

    private bool flamerInProgress;

    [Header("Component Settings")]

    [SerializeField]
    private GameObject speechBubblePrefab;

    [SerializeField]
    private GameObject bubbleCoords;

    [SerializeField]
    private GameObject flamerBox;
    
    [SerializeField]
    private GameObject bulletTrail;

    [SerializeField]
    private ParticleSystem flamerVFX;

    [SerializeField]
    private Flamer_Behavior fBehavior;

    //Layermask for the raycast2D to only see zombies to prevent funky interactions
    [SerializeField]
    private LayerMask zombieMask;

    [Header("Attack Settings")]

    [SerializeField]
    private int maximumAmmo;

    [SerializeField]
    private float rifleFireRate;

    [SerializeField]
    private float sgFireRate;

    [SerializeField]
    private float sgSpread;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private float flamerDuration;

    [SerializeField]
    private int flamerDamage;

    [SerializeField]
    private int sniperDamage;

    [SerializeField]
    private int shotgunDamage;

    [SerializeField]
    private int rifleDamage;

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

            //Set the detectedZombie to a variable so we can reference it later down the line this frame
            detectedZombie = hit.collider.gameObject.GetComponent<Enemy_Behaviour>();

            //If the soldier isn't currently waiting for an item
            //Do the weapon state machine
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

            animator.SetBool("isShooting", false);

            //No zombie, no variable
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

        if(currentSpeechBubble)
        {
            Destroy(currentSpeechBubble);
            currentSpeechBubble = null;
        }

        currentSpeechBubble = Instantiate(speechBubblePrefab, bubbleCoords.transform.position, Quaternion.identity);
        currentSpeechBubble.GetComponent<DesiredItemBubble>().UpdateItemSprite(wantedItem.itemSprite);

        //Set waitingForItem to true so that its not constantly asking for a new one
        waitingForItem = true;

    }

    //Called by the player when they attempt to give the soldier an item
    public void DeliverItem(DeliveryItem item)
    {

        //If the item is the default rifle
        //Fill their ammo back up and get back to shooting
        if (item.weapon == Weapon.Rifle)
        {

            animator.SetBool("isAsking", false);

            currentAmmo = maximumAmmo;

            waitingForItem = false;

            //gM.AddScore(1);

            //Get rid of the current speech bubble
            //Destroy(currentSpeechBubble);
            //currentSpeechBubble = null;

            currentWeapon = Weapon.Rifle;

            canShoot = true;

            animator.ResetTrigger("hasShotgun");
            animator.ResetTrigger("hasSniper");
            animator.ResetTrigger("hasFlamer");
        }
        //if it's a special weapon
        //switch to that weapon and give them 3 shots if it's the shotgun
        //the other two weapons function differently
        else
        {
            Debug.Log(item.weapon.ToString());
            currentWeapon = item.weapon;
            currentSPAmmo = 3;

            if(waitingForItem)
            {
                waitingForItem = false;
            }
        }
        //Get rid of the speech bubble regardless of if they got rifle bullets or special ammo
        if(currentSpeechBubble)
        {
            Destroy(currentSpeechBubble);
            currentSpeechBubble = null;
        }
        //set their state back to idle 
        currentSState = SoldierState.Idle;

    }
    //Called by the animator to tell the code that the shooting animation finished
    private void ShootingOver()
    {
        //isShooting = false;
        animator.SetInteger("animIndex", 0);
        waitingForItem = false;
        detectedZombie = null;
    }
    //Cooldown between shots allowed by the code
    //Might be outdated, just shoots based on the animation now
    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(rifleFireRate);
        canShoot = true;
    }
    //Cooldown between shotgun shots
    //Same as rifle, might be outdated since it's all anim based
    private IEnumerator ShotgunCooldown()
    {
        yield return new WaitForSeconds(sgFireRate);
        canShotgun = true;
    }
    //Set the firerate of the soldier through outside factors
    //Made for the powerup, however
    //Need to fix now that anim based shooting is a thing, idk if this actually does anything now
    public void SetFireFireRate(float newRate)
    {
        rifleFireRate = newRate;
    }
    //Used to be just making the visual
    //Now handles all the Rifle shooting logic
    public void ShootVisual()
    {
        //Play the muzzle flash animation
        mfB.PlayAnimation();

        //Create the bullet prefab and set all the attributes 
        GameObject tempTrail = Instantiate(bulletTrail, mfB.transform.position, Quaternion.identity);
        tempTrail.GetComponent<BulletBehavior>().SetAttributes(false, rifleDamage,2.0f);
        tempTrail.GetComponent<Rigidbody2D>().velocity = Vector3.left * bulletSpeed;
        tempTrail.GetComponent<BulletBehavior>().SetDestination(detectedZombie.transform.position);

        //Since a bullet was shot, reduce ammo by 1
        currentAmmo--;
        //If the soldier has run out of ammo
        if (currentAmmo <= 0)
        {
            //Then make them ask for ammo and wait
            //Debug.Log("Out of ammo");

            wantedItem = gM.GetDeliveryItems()[0];
            canShoot = false;
            waitingForItem = true;
            animator.SetBool("isShooting", false);
            animator.SetBool("isAsking", true);

            WantsNewItem();
        }

    }
    //State machine for the weapon type the soldier currently has, called in update when the soldier can actually see something
    //Not actually sure if this is useful anymore
    //WAS useful when all attack logic was linetracing, but now that bullets actually interact in the game might be outdated and could be refactored
    //Still nice to have
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
    //Weapon.Rifle state behavior
    //Outdated, mostly does nothing now
    //Behavior now executed by the animation itself to line up with the visuals
    private void RifleBehavior()
    {
        animator.SetBool("isShooting", true);
        animator.SetBool("isAsking", false);
        if (canShoot)
        {
            //canShoot = false;
            //currentAmmo--;

            //Debug.Log(currentAmmo);
            /*
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
            */
        }
    }
    //Weapon.Shotgun behavior
    //Makes sure that the soldier is able to shoot all their shotgun shells before switching back to rifle
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

    //Weapon.Sniper behavior
    //Only shoots a single time before switching back
    private void SniperBehavior()
    {
        animator.SetBool("isShooting", true);
        animator.SetBool("isAsking", false);
        animator.SetTrigger("hasSniper");
        canShoot = false;
    }

    //Weapon.Flamer behavior
    //Unlike the other weapons, just uses a trigger box instead of bullets
    private void FlamerBehavior()
    {
        animator.SetBool("isShooting", false);
        animator.SetBool("isAsking", false);
        animator.SetTrigger("hasFlamer");

        canShoot = false;

        if (!flamerInProgress)
        {
            flamerBox.SetActive(true);
            flamerInProgress = true;
            StartCoroutine(FlamerDuration());
            flamerVFX.Play();
            fBehavior.SetFlamerDamage(flamerDamage);
        }
    }

    //Code called when running out of special ammos
    //Switches back to rifle and waits for ammo to reload
    private void ReturnToRifle()
    {

        animator.ResetTrigger("hasShotgun");
        animator.ResetTrigger("hasSniper");
        animator.ResetTrigger("hasFlamer");
        canShoot = false;
        currentWeapon = Weapon.Rifle;
        WantsNewItem();
    }

    //Actual shotgun behavior
    //Creates a spread of 5 (currently) bullets in a fan
    //Big damage with no falloff
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

            tempBullet.GetComponent<BulletBehavior>().SetAttributes(false, shotgunDamage,1.0f);


        }

    }

    //Actual sniper behavior
    //A single shot but does massive damage and pierces through all the zombies making a strong wave clear or anti-tank option
    //The supply box shooouuuld have a long cooldown to compensate, making this a last resort weapon
    public void SniperAttack()
    {
        mfB.PlayAnimation();

        GameObject tempTrail = Instantiate(bulletTrail, mfB.transform.position, Quaternion.identity);
        tempTrail.GetComponent<BulletBehavior>().SetAttributes(true, sniperDamage, 5.0f);
        tempTrail.GetComponent<Rigidbody2D>().velocity = Vector3.left * bulletSpeed;
        tempTrail.GetComponent<BulletBehavior>().SetDestination(detectedZombie.transform.position);

    }

    //Handles the flamer behavior
    //Actual attack behavior is handled in FLamer_Behavior.cs since it uses a separate triggerBox2D to do weapon logic
    //Just lasts for a duration before switching back to rifle.
    private IEnumerator FlamerDuration()
    {
        yield return new WaitForSeconds(flamerDuration);
        flamerBox.SetActive(false);
        flamerInProgress = false;
        ReturnToRifle();
    }

}
