using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instantiate : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyPrefab;
    [SerializeField] Transform parent;
    GameManager gameManager;
    private int createHoard = 100;

    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            StartCoroutine(SpawnEnemy(enemyPrefab[i], i));
        }
    }

    private void Update()
    {
        if (gameManager.GetGameStatus())
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator SpawnEnemy(GameObject enemyPrefab, int i)
    {
        float spawnRate = 0.0f;
        while (true)
        {
            if (enemyPrefab.gameObject.name == "Runner")
            {
                spawnRate = 0.75f;
                RandSpawnRate(spawnRate,i, true);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            else if(enemyPrefab.gameObject.name == "Clicker")
            {
                spawnRate = 3.5f;
                RandSpawnRate(spawnRate, i, false);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            else if(enemyPrefab.gameObject.name == "Bloater")
            {
                if (gameManager.GetPlayerScore() % 50 == 0)
                {
                    spawnRate = 10f;
                }
                else
                {
                    spawnRate = 25f;
                }
                //spawnRate = 25f;
                RandSpawnRate(spawnRate, i, false);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            int randomLane = Mathf.RoundToInt(Random.Range(-3, 3));
            if(randomLane != 0)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x, Mathf.RoundToInt(3 / randomLane), 0);
                //Generate random lane for enemy
                GameObject spawnGameObject = Instantiate(enemyPrefab, spawnPosition,
                       Quaternion.identity);
                spawnGameObject.transform.parent = parent; // Parent clones to a single object
                
                audioManager.PlayEnemySound(audioManager.ZombieAppear_Audio);
            } 
        }
    }

    private void RandSpawnRate(float spawnRate,int i, bool canCreateHoard)
    {
        int playerScore = gameManager.GetPlayerScore();
        if(playerScore >= 10 && playerScore < 20)
        {
            spawnRate -= 0.25f; 
        }
        else if(playerScore % 50 == 0)
        {
            spawnRate -= 0.5f;
        }
        if (canCreateHoard && Mathf.RoundToInt(Random.Range(0, createHoard)) == createHoard)
        {
            spawnRate = 0f;
        }
        gameManager.enemySpawnTime[i] = spawnRate + Mathf.RoundToInt(Random.Range(0, 1));
        if (createHoard != 0)
        {
            createHoard--;
        }
        else
        {
            createHoard = 100;
        }
    }
}
