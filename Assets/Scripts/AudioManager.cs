using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource EnemySource;
    [SerializeField] AudioSource GunSource;
    [SerializeField] AudioSource FlameSource;
    [SerializeField] AudioSource TurretSource;


    [Header("Audio Clip")]
    public AudioClip background_Audio;
    public AudioClip Rifle_Audio;
    public AudioClip Shotgun_Audio;
    public AudioClip Flamethrower_Audio;
    public AudioClip Sniper_Audio;
    public AudioClip pickup_Audio;
    public AudioClip ZombieDead_Audio;
    public AudioClip ZombieAttack_Audio;
    public AudioClip ZombieAppear_Audio;
    public AudioClip ZombieEatFinal_Audio;
    public AudioClip gameStart_Audio;
    public AudioClip gameEnd_Audio;
    public AudioClip Deliver_Audio;
    public AudioClip ScreenClean_Audio;
    public AudioClip AskWeapon_Audio;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
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

    public void PlayEnemySound(AudioClip clip)
    {
        EnemySource.PlayOneShot(clip);
    }

    public void PlayGun(AudioClip clip)
    {
        GunSource.PlayOneShot(clip);
    }

    public void PlayFlame(AudioClip clip)
    {
        FlameSource.PlayOneShot(clip);
    }
    public void PlayTurret(AudioClip clip)
    {
        TurretSource.PlayOneShot(clip);
    }
}
