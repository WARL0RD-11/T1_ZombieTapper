using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instantiate : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] float interval = 4f;
    float time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= interval)
        {
            time = 0f;
            var spawnPosition = new Vector3(0, Mathf.RoundToInt(Random.Range(-8, 8)), 0);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
