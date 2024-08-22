using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1.0f;
    //[SerializeField] Animation deathAnimation;
    [SerializeField] float waitAfterDeath = 2.0f;

    public static bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            EnemyMovement();
        }
        //ToDo
        // Enemy speed increase over time
        // Enemy Health
    }
    private void EnemyMovement()
    {
        //Move enemy on the x-axis
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;

        //Check if enemy reached the barricade
        if(transform.position.x > 0)
        {
            isGameOver = true;
            GameOver();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet" && !isGameOver)
        {
            //deathAnimation.Play();
            new WaitForSeconds(waitAfterDeath);
            Destroy(gameObject);
        }

    }
    private void GameOver()
    {
        // play animation??
        // GameOver screen
    }
}
