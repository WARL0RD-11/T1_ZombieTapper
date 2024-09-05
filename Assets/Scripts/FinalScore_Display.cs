using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore_Display : MonoBehaviour
{
    public TextMeshProUGUI finalS_text;

    //Directs to the game manager in the level
    private GameManager gM;

    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        if (gM != null){
            finalS_text.SetText(gM.GetPlayerScore().ToString());
        }
    }

}
