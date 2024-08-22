using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instantiate : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] Transform parent;
    float time = 0;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(UpdateSpawnTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy_Behaviour.isGameOver)
        { return; }
        time += Time.deltaTime;
        if (time >= gameManager.enemySpawnTime)
        {
            time = 0f;
            var spawnPosition = new Vector3(transform.position.x, Mathf.RoundToInt(
                Random.Range(-2, 2)) * 2, 0); //Generate random lane for enemy
            GameObject spawnGameObject = Instantiate(enemyPrefab, spawnPosition,
               Quaternion.identity);
            spawnGameObject.transform.parent = parent; // Parent all clones to a single object
        }
    }
    public IEnumerator UpdateSpawnTime()
    {
        while (true)
        {
            gameManager.enemySpawnTime = 2 + Mathf.RoundToInt(Random.Range(-2, 1));
            yield return new WaitForSeconds(gameManager.enemySpawnTime);
        }
    }
}
