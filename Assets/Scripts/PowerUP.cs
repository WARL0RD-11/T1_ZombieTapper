using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;

public class PowerUP : MonoBehaviour
{
    [SerializeField]
    private TurretBehavior Turret1;

    [SerializeField]
    private TurretBehavior Turret2;

    private bool IsPlayerInTrigger;

    [SerializeField]
    private float Delay;

    [SerializeField]
    private float InitDelay;

    // Start is called before the first frame update
    void Start()
    {
        IsPlayerInTrigger = false;
        StartCoroutine(HidePowerupOnStart(this.gameObject, InitDelay));
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

            StartCoroutine(HideShowPowerUp(this.gameObject, Delay));
            //Destroy(this.gameObject);
            //StartCoroutine(HideShowPowerUp(GameObject.FindWithTag("PupED"), Delay));
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Overlapping");
            IsPlayerInTrigger = true;
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

    private IEnumerator HideShowPowerUp(GameObject PowerUpObject, float delay)
    {
        if (PowerUpObject != null)
        {
            PowerUpObject.GetComponent<SpriteRenderer>().enabled = false;
            PowerUpObject.GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(delay);
            PowerUpObject.GetComponent<SpriteRenderer>().enabled = true;
            PowerUpObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private IEnumerator HidePowerupOnStart(GameObject PowerUpObject, float InitDelay)
    {
        if (PowerUpObject != null)
        {
            PowerUpObject.GetComponent<SpriteRenderer>().enabled = false;
            PowerUpObject.GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(InitDelay);
            PowerUpObject.GetComponent<SpriteRenderer>().enabled = true;
            PowerUpObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
