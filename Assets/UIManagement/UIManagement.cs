using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    [SerializeField] public string GameScene_Name;
    [SerializeField] public GameObject StartMenu;
    [SerializeField] public GameObject OptionMenu;
    [SerializeField] public GameObject InstructMenu;
    [SerializeField] public Slider volumeSlider;


    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        } else {
            Load();
        }
    }
    //Scene Controller
    public void StartGame()
    {
        SceneManager.LoadScene(GameScene_Name);
    }

    public void GetScene(string Scene_Name)
    {
        SceneManager.LoadScene(Scene_Name);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //Panel Controller
    public void Open_OptionMenu()
    {
        OptionMenu.SetActive(true);
        StartMenu.SetActive(false);
    }
    public void Backto_StartMenu()
    {
        OptionMenu.SetActive(false);
        StartMenu.SetActive(true);
    }
    public void Open_InstructMenu()
    {
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(true);
    }
    public void Backto_OptionMenu()
    {
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);
    }

    //Volume Controller
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

}
