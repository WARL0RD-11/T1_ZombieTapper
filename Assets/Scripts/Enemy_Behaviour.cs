using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1.0f;
    //[SerializeField] Animation deathAnimation;
    [SerializeField] int health = 1;

    GameManager gameManager;
    Animator animator;
    GameObject sandBagObject;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        sandBagObject = GameObject.FindGameObjectWithTag("Finish");
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
        if (transform.position.x >= sandBagObject.transform.position.x)
        {
            animator.SetBool("isGameOver", true);
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
