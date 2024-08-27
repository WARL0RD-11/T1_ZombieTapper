using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadMenu : MonoBehaviour
{
    [SerializeField] public Button DeadMenu_Replay;
    [SerializeField] public GameObject EndMenu;



    public void EndMenuAppear()
    {
        EndMenu.SetActive(true);
        DeadMenu_Replay.Select();
    }

    public void restart()
    {
        SceneManager.LoadScene("Staging");
    }

    public void returnToMain()
    {
        SceneManager.LoadScene("Start_Menu");
    }

}
