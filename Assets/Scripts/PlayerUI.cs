using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    //Points to the attached TextMeshPro component
    private TMP_Text scoreText;

    public void Start()
    {
        //Get the TMP component and set it
        scoreText = GetComponentInChildren<TMP_Text>();
    }


    //Called by the Game Manager
    //Updates the score text to match the player's current score.
    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();

        Debug.Log("Updated the player score text");
    }
}
