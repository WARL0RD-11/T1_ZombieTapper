using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background_Audio;
    public AudioClip Rifle_Audio;
    public AudioClip Shotgun_Audio;
    public AudioClip Pistol_Audio;
    public AudioClip Sniper_Audio;
    public AudioClip pickup_Audio;
    public AudioClip ZombieDead_Audio;
    public AudioClip gameStart_Audio;
    public AudioClip gameEnd_Audio;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicSource.clip = background_Audio;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
