using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{
    [SerializeField] float enemySpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }
    public void EnemyMovement()
    {
        transform.position += new Vector3(1.0f, 0f, 0) * enemySpeed * Time.deltaTime;
    }
}
