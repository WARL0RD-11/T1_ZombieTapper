using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1.0f;
    //[SerializeField] Animation deathAnimation;
    [SerializeField] float waitAfterDeath = 2.0f;
    [SerializeField] int health = 1;

    GameManager gameManager;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
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
            animator.SetBool("isGameOver", true);
        }
        //Check if enemy reached the barricade
        if (transform.position.x > 0)
        {
            animator.SetBool("isGameOver", true);
            gameManager.EndGame();
        }
    }
    private void EnemyMovement()
    {
        //Move enemy on the x-axis
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet" && !gameManager.GetGameStatus())
        {
            --health;
            if(health <=0)
            {
                KillEnemy();
            }
        }

    }
    private void KillEnemy()
    {
        animator.SetBool("isZombieDead", true);
        new WaitForSeconds(waitAfterDeath);
        Destroy(gameObject);
    }

    //Function to be called by soldiers when they are delivered the correct item
    public void OnDeath()
    {
        //Set the speed to 0 so corpses can't move
        enemySpeed = 0.0f;

        //Play animations or something
        new WaitForSeconds(waitAfterDeath);
        Destroy(gameObject);
    }
}
