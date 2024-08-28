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
        if (!gameManager.GetGameStatus())
        {
            EnemyMovement();
        }
        else
        {
            animator.SetBool("isZombieIdle", true);
        }
        //Check if enemy reached the barricade
        if (transform.position.x >= sandBagObject.transform.position.x)
        {
            animator.SetBool("isZombieIdle", true);
            gameManager.EndGame();
        }
    }
    private void EnemyMovement()
    {
        //if(this.gameObject.tag == "Enemy" && transform.position.x 
        //    > (sandBagObject.transform.position.x/2))
        //{
        //    enemySpeed += 0.25f; 
        //}
        //Move enemy on the x-axis
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;
    }

    //Function to be called by soldiers when they are delivered the correct item
    public void OnDeath()
    {
        gameManager.AddScore(1);
        //Set the speed to 0 so corpses can't move
        enemySpeed = 0.0f;
        animator.SetBool("isZombieDead", true);
        GetComponent<BoxCollider2D>().enabled = false;

        //Play animations or something
        //new WaitForSeconds(waitAfterDeath);
        //Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("Entered Obstacle");
            StartCoroutine(AttackObstacle());
        }
    }

    public IEnumerator AttackObstacle()
    {
        animator.SetBool("isZombieIdle", true);
        if (obstacleObject.TakeDamage(enemyPower) < 0)
        {
            animator.SetBool("isZombieIdle", false);
            StopAllCoroutines();
            yield return null;
        }
        yield return new WaitForSeconds(attackCoolDown);
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
