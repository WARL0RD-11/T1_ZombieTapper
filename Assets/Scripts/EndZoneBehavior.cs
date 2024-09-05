using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneBehavior : MonoBehaviour
{
    //Directs to the game manager in the level
    private GameManager gM;

    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the gM variable to the game manager for communication
        gM = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Make sure that Game Manager is actually set to something before executing
        if (gM != null)
        {
            //End the game if somethinng on the zombie physics layer enters the trigger
            audioManager.PlayEnemySound(audioManager.ZombieEatFinal_Audio);
            gM.EndGame();
        }
    }
}
