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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        
        //ToDo
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GetGameStatus())
        {
            EnemyMovement();
        }
    }
    private void EnemyMovement()
    {
        //Move enemy on the x-axis
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;

        //Check if enemy reached the barricade
        if(transform.position.x > 0)
        {
            gameManager.EndGame();
        }
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
        //deathAnimation.Play();
        new WaitForSeconds(waitAfterDeath);
        Destroy(gameObject);
    }
}
