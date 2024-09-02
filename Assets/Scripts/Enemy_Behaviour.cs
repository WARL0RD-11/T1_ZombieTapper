using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] float enemySpeed = 0.25f;
    [SerializeField] int health = 1;
    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] int enemyPower = 1;
    [SerializeField] GameObject bloodPoolPrefab;

    GameManager gameManager;
    Animator animator;
    GameObject sandBagObject;
    Obstacle_Behaviour obstacleObject;
    bool canCharacterMove = true;
    bool canAttack = false;
    private List<GameObject> targetsInRange = new List<GameObject>();

    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        sandBagObject = GameObject.FindGameObjectWithTag("Finish");
        //obstacleObject = FindAnyObjectByType<Obstacle_Behaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GetGameStatus() && canCharacterMove)
        {
            EnemyMovement();
        }
        else if(gameManager.GetGameStatus()) 
        {
            canCharacterMove = false;
            //canAttack = false;
            animator.SetBool("isZombieIdle", true);
            animator.SetBool("isAttacking", false);
        }
        //Check if enemy reached the barricade
        if (transform.position.x >= sandBagObject.transform.position.x)
        {
            animator.SetBool("isZombieEnd", true);
            canCharacterMove = false;
            gameManager.EndGame();
        }
        if (canAttack )
        {
            StartCoroutine(WaitForAttack());
        }
    }
    private void EnemyMovement()
    {
        //Move enemy on the x-axis
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;
    }

    //Function to be called by soldiers when they are delivered the correct item
    public void OnDeath()
    {
        //gameManager.AddScore(1);
        //Set the speed to 0 so corpses can't move
        enemySpeed = 0.0f;
        animator.SetBool("isZombieDead", true);
        GetComponent<BoxCollider2D>().enabled = false;

        //audioManager.PlaySFX(audioManager.ZombieEatFinal_Audio);

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            targetsInRange.Add(collision.gameObject);
            canCharacterMove = false;
            canAttack = true;
            //animator.SetBool("isZombieIdle", true);
            animator.SetBool("isAttacking", true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            canCharacterMove = true;
            canAttack = false;
            //animator.SetBool("isZombieIdle", false);
            animator.SetBool("isAttacking", false);
        }
    }

    public IEnumerator WaitForAttack()
    {
        foreach (var target in targetsInRange)
        {
            canAttack = false;
            audioManager.PlaySFX(audioManager.ZombieAttack_Audio);
            if (target && target.GetComponent<Obstacle_Behaviour>().TakeDamage(enemyPower) <= 0)
            {
                target.GetComponent<Animator>().SetBool("shouldDie", true);
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(attackCoolDown);
            canAttack = true;
        }
    }

    public void DeathOver()
    {
        Vector3 deathPos = transform.position;
        deathPos.y = deathPos.y - 0.4f;

        Instantiate(bloodPoolPrefab, deathPos, Quaternion.identity);

        audioManager.PlaySFX(audioManager.ZombieDead_Audio);

        Destroy(this.gameObject);
    }

    public void ReduceHealth(int amount)
    {
        //--health;
        health -= amount;
        if (health < 0)
        {
            OnDeath();
        }
        else
        {
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
