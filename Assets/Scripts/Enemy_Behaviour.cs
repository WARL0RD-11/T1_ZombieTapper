using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] int health = 1;
    [SerializeField] int enemyPower = 1;
    [SerializeField] float enemySpeed = 0.25f;
    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] GameObject bloodPoolPrefab;
    [SerializeField] private float AnimTimeScale;
    [SerializeField] public float spawnRate = 0f;

    GameManager gameManager;
    Animator animator;
    GameObject sandBagObject;
    AudioManager audioManager;

    bool canCharacterMove = true;
    bool canAttack = false;
    List<GameObject> targetsInRange = new List<GameObject>();

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        sandBagObject = GameObject.FindGameObjectWithTag("Finish");
        animator.speed = AnimTimeScale;
    }

    void Update()
    {
        if (!gameManager.GetGameStatus() && canCharacterMove)
        {
            EnemyMovement();
        }
        else if(gameManager.GetGameStatus()) 
        {
            canCharacterMove = false;
            canAttack = false;  
            animator.SetBool("isZombieIdle", true);
            animator.SetBool("isAttacking", false);
        }
        if (transform.position.x >= sandBagObject.transform.position.x)
        {
            animator.SetBool("isZombieEnd", true);
            canCharacterMove = false;
            gameManager.EndGame();
        }
        if (canAttack)
        {
            StartCoroutine(WaitForAttack());
        }
    }
    private void EnemyMovement()
    {
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;
    }
    public void OnDeath()
    {
        enemySpeed = 0.0f;
        animator.SetBool("isZombieDead", true);
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            targetsInRange.Add(collision.gameObject);
            canCharacterMove = false;
            canAttack = true;
            animator.SetBool("isAttacking", true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            canCharacterMove = true;
            canAttack = false;
            animator.SetBool("isAttacking", false);
        }
    }

    public IEnumerator WaitForAttack()
    {
        foreach (var target in targetsInRange)
        {
            canAttack = false;
            audioManager.PlayEnemySound(audioManager.ZombieAttack_Audio);
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
        audioManager.PlayEnemySound(audioManager.ZombieDead_Audio);
        gameManager.AddScore(1);
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
