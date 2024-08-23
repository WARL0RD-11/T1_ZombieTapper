using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Int to track the player's score throughout the game to be displayed
    private int playerScore;

    //The score the player will recieve on each defeated zombie
    [SerializeField]
    private int scorePerZombie;

    //The penalty in seconds that a guard will not be able to act after the wrong delivery
    [SerializeField]
    private float delayPenalty;

    //The progress the player needs for the powerup
    //Placeholder, still needs to be added
    [SerializeField]
    private int powerUpProgress;

    //Points to the playerUI script attached to the player UI
    //Will communicate and update the score for the player.
    [SerializeField]
    private PlayerUI playerUI;

    //Variable to track whether or not the game has ended
    private bool gameHasEnded;

    //Contains all the delivery items that the soldiers could want
    [SerializeField]
    private DeliveryItem[] deliveryItems;

    [SerializeField]
    private float linecastDistance;

    [SerializeField]
    public float[] enemySpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;

        gameHasEnded = false;

        Debug.Log("Game manager exists");

        AddScore(0);
    }

    //Something in the game tells the manager to add score to the player
    public void AddScore(int score)
    {

        Debug.Log("Adding " + score.ToString() + " score to the player");
        //Add the score to the existing score and store it
        playerScore += score;

        //Call the playerUI object and tell it to update the player score.
        playerUI.UpdateScoreText(playerScore);
    }

    //Called when a zombie breaches the defenses.
    public void EndGame()
    {
        gameHasEnded = true;

        GameHasEnded();
    }

    //Tells everything in the game to stop doing things
    private void GameHasEnded()
    {
        //Code to get everything and pause things like animations and movement by zombies/guards.
        //Show off the score or something.
        Debug.Log("Game is over");
        //Transition to the end game screen
    }


    //Return the current value of gameHasEnded for other scripts to get so that nothing goes wonky when the game is over.
    public bool GetGameStatus()
    {
        return gameHasEnded;
    }

    public float GetDelayPenalty()
    { return delayPenalty; }

    //Returns the whole array of delivery items
    public DeliveryItem[] GetDeliveryItems()
    { return deliveryItems; }
    
    //Returns a random delivery item that a soldier could want
    public DeliveryItem GetRandomDeliveryItem()
    {
        int index = Random.Range(0, deliveryItems.Length);

        return deliveryItems[index];
    }

    public float GetLinecastDistance()
    {
        return linecastDistance;
    }
}
