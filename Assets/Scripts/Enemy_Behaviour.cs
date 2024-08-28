using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] float enemySpeed = 0.25f;
    [SerializeField] int health = 1;
    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] int enemyPower = 1;

    GameManager gameManager;
    Animator animator;
    GameObject sandBagObject;
    Obstacle_Behaviour obstacleObject;
    bool canCharacterMove = true;
    bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        sandBagObject = GameObject.FindGameObjectWithTag("Finish");
        obstacleObject = FindAnyObjectByType<Obstacle_Behaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GetGameStatus() && canCharacterMove)
        {
            EnemyMovement();
        }
        else
        {
            canCharacterMove = false;
            animator.SetBool("isZombieIdle", true);
        }
        //Check if enemy reached the barricade
        if (transform.position.x >= sandBagObject.transform.position.x)
        {
            canCharacterMove = false;
            gameManager.EndGame();
        }
        if(canAttack)
        {
            StartCoroutine(AttackObstacle());
        }
        else if(canCharacterMove)
        {
            animator.SetBool("isZombieIdle", false);
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
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            canCharacterMove = false;
            canAttack = true;
            animator.SetBool("isZombieIdle", true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            canCharacterMove = true;
            canAttack = false;
            animator.SetBool("isZombieIdle", false);
        }
    }

    public IEnumerator AttackObstacle()
    {
        canAttack = false;
        if (obstacleObject.TakeDamage(enemyPower) == 0)
        {
           Destroy(obstacleObject.gameObject);
           StopAllCoroutines();
           yield return null;
        }
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    public void DeathOver()
    {
        Destroy(this);
    }

    public void ReduceHealth()
    {
        --health;
        if(health < 0)
        {
            OnDeath();
        }
    }
}
