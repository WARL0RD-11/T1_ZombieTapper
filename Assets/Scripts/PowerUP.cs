using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour
{
    [SerializeField]
    private TurretBehavior Turret1;

    [SerializeField]
    private TurretBehavior Turret2;

    private bool IsPlayerInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        IsPlayerInTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && (IsPlayerInTrigger) && (this.gameObject.CompareTag("PupTurret")))
        {
            Debug.Log("Key Pressed");
            Turret1.IsActivated = true;
            Turret2.IsActivated = true;
            Debug.Log("Turret SET TO TRUE");
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Overlapping");
            IsPlayerInTrigger=true;
        }
    }
}
