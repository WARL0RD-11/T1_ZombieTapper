using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning_Behavior : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float lifeSpan;
    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
