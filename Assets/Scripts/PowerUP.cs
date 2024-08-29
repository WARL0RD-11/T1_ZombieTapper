using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour
{
    [SerializeField]
    private TurretBehavior Turret1;

    [SerializeField]
    private TurretBehavior Turret2;

    [SerializeField]
    private SoldierBehavior Soldier1;

    [SerializeField]
    private SoldierBehavior Soldier2;

    [SerializeField]
    private SoldierBehavior Soldier3;

    [SerializeField]
    private SoldierBehavior Soldier4;

    private bool IsPlayerInTrigger;

    [SerializeField]
    private float NewRate;

    // Start is called before the first frame update
    void Start()
    {
        IsPlayerInTrigger = false;
        NewRate = .1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && (IsPlayerInTrigger) && (this.gameObject.CompareTag("PupTurret")))
        {
            Debug.Log("Key Pressed");
            Turret1.IsActivated = true;
            Turret2.IsActivated = true;
            Debug.Log("Turret SET TO TRUE");
            Destroy(this.gameObject);
            Destroy(GameObject.FindWithTag("PupED"));
        }

        if (Input.GetKeyDown(KeyCode.S) && (IsPlayerInTrigger) && (this.gameObject.CompareTag("PupED")))
        {
            Debug.Log("ENERGY DRINK");
            Soldier1.SetFireFireRate(NewRate);
            Soldier2.SetFireFireRate(NewRate);
            Soldier3.SetFireFireRate(NewRate);
            Soldier4.SetFireFireRate(NewRate);
            Destroy(this.gameObject);
            Destroy(GameObject.FindWithTag("PupTurret"));
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Not HEre ");
            IsPlayerInTrigger = false;
        }
    }
}
