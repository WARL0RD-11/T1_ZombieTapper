using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPrompter_Behavior : MonoBehaviour
{

    [SerializeField] private float lifeSpan;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
