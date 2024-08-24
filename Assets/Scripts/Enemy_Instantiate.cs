using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instantiate : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyPrefab;
    [SerializeField] Transform parent;
    GameManager gameManager;
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
                gameManager.enemySpawnTime[i] = 2 + Mathf.RoundToInt(Random.Range(-2, 1));
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            else
            {
                gameManager.enemySpawnTime[i] = 5 + Mathf.RoundToInt(Random.Range(1, 2));
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            var spawnPosition = new Vector3(transform.position.x, Mathf.RoundToInt(
                                Random.Range(-2, 2)) * 2, 0); //Generate random lane for enemy
            GameObject spawnGameObject = Instantiate(enemyPrefab, spawnPosition,
                   Quaternion.identity);
            spawnGameObject.transform.parent = parent; // Parent clones to a single object
        }
    }
}
