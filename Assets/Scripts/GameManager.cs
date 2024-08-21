using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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

    //Variable to track whether or not the game has ended
    private bool gameHasEnded;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;

        gameHasEnded = false;
    }

    //Something in the game tells the manager to add score to the player
    public void AddScore(int score)
    {
        //Add the score to the existing score and store it
        playerScore += score;
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
    }

}
