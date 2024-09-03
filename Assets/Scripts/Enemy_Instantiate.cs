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

            if (enemyPrefab.gameObject.name == "Enemy")
            {
                spawnRate = 2.0f;
                RandSpawnRate(spawnRate,i, true);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            else if(enemyPrefab.gameObject.name == "Enemy 1")
            {
                spawnRate = 3.0f;
                RandSpawnRate(spawnRate, i, false);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            else
            {
                spawnRate = 15f;
                RandSpawnRate(spawnRate, i, false);
                yield return new WaitForSeconds(gameManager.enemySpawnTime[i]);
            }
            var spawnPosition = new Vector3(transform.position.x, Mathf.RoundToInt(
                                Random.Range(-2, 2)) * 1.5f, 0); //Generate random lane for enemy
            GameObject spawnGameObject = Instantiate(enemyPrefab, spawnPosition,
                   Quaternion.identity);
            spawnGameObject.transform.parent = parent; // Parent clones to a single object

            audioManager.PlaySFX(audioManager.ZombieAppear_Audio);
        }
    }

    private void RandSpawnRate(float spawnRate,int i, bool canCreateHoard)
    {
        int playerScore = gameManager.GetPlayerScore();
        if(playerScore >= 10 && playerScore < 20)
        {
            spawnRate -= 0.25f; 
        }
        else if(playerScore >= 20)
        {
            spawnRate -= 0.5f;
        }
        if (canCreateHoard && Mathf.RoundToInt(Random.Range(0, createHoard)) == createHoard)
        {
            spawnRate = 0f;
        }
        gameManager.enemySpawnTime[i] = spawnRate + Mathf.RoundToInt(Random.Range(0, 2));
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
