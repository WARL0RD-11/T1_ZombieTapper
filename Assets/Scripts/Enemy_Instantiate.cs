using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instantiate : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyPrefab;
    [SerializeField] Transform parent;
    GameManager gameManager;
    private int createHoard = 100;
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
        while (true)
        {

            if (enemyPrefab.gameObject.name == "Enemy")
            {
                RandSpawnRate(i, true);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            else
            {
                RandSpawnRate(i, false);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            var spawnPosition = new Vector3(transform.position.x, Mathf.RoundToInt(
                                Random.Range(-2, 2)) * 2, 0); //Generate random lane for enemy
            GameObject spawnGameObject = Instantiate(enemyPrefab, spawnPosition,
                   Quaternion.identity);
            spawnGameObject.transform.parent = parent; // Parent clones to a single object
        }
    }

    private void RandSpawnRate(int i, bool canCreateHoard)
    {
        float spawnRate = 5.0f;
        int playerScore = gameManager.GetPlayerScore();
        if(playerScore >= 10 && playerScore < 20)
        {
            spawnRate -= 0.5f; 
        }
        else if(playerScore >= 20)
        {
            spawnRate -= 1f;
        }
        if (canCreateHoard && Mathf.RoundToInt(Random.Range(0, createHoard)) == createHoard)
        {
            gameManager.enemySpawnTime[i] = 0f;
            spawnRate = 0f;
        }
        gameManager.enemySpawnTime[i] = spawnRate + Mathf.RoundToInt(Random.Range(0, 2));
        if (createHoard != 0)
        {
            createHoard--;
        }
    }
}
