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

    //First-Button Selection Setup
    [SerializeField] public Button First_Menu_Play;
    [SerializeField] public Button Second_Menu_Instruct;
    [SerializeField] public Button Thrid_Menu_Back;

    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }


    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        } else {
            Load();
        }
        First_Menu_Play.Select();
    }
    //Scene Controller
    public void StartGame()
    {
        audioManager.PlaySFX(audioManager.gameStart_Audio);
        SceneManager.LoadScene(1);
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
        audioManager.PlaySFX(audioManager.gameStart_Audio);
        OptionMenu.SetActive(true);
        StartMenu.SetActive(false);
        Second_Menu_Instruct.Select();
    }
    public void Backto_StartMenu()
    {
        OptionMenu.SetActive(false);
        StartMenu.SetActive(true);
        First_Menu_Play.Select();
    }
    public void Open_InstructMenu()
    {
        audioManager.PlaySFX(audioManager.gameStart_Audio);
        OptionMenu.SetActive(false);
        InstructMenu.SetActive(true);
        Thrid_Menu_Back.Select();
    }
    public void Backto_OptionMenu()
    {
        OptionMenu.SetActive(true);
        InstructMenu.SetActive(false);
        Second_Menu_Instruct.Select();
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
