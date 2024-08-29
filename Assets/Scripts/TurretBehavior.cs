using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    [SerializeField]
    private LayerMask ZombieMask;

    [SerializeField]
    private float TargetRotationAngle; //amplitude?

    [SerializeField]
    public bool IsActivated;

    private float LineCastDistance;
    private GameManager GameManager;
    private Color TempColor;
    //private Vector3 TempRotation = new Vector3(0.0f, 0.0f, -45.0f);
    private float RotationSpeed = 2f; //frequency
    private Quaternion StartRot;
    private Quaternion EndRot;
    //private bool IsRotatingForward = true;

    private Enemy_Behaviour DetectedZombie;

    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        LineCastDistance = 11.0f;
        //TargetRotationAngle = 45.0f;

        StartRot = transform.rotation;
        IsActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetGameStatus())
        {
            if (IsActivated)
            {
                RotationBouncer();
                Invoke("DeactivateTurret", 5);
                //RotationBouncer(0.0f);
            }
        }

    }

    private void RotationBouncer()
    {
        RaycastDetection();
        Debug.Log("Turret Firing");
        audioManager.PlaySFX(audioManager.ScreenClean_Audio);
        //float CurrentAngle = transform.eulerAngles.z;
        float oscillation = Mathf.Sin(Time.deltaTime * RotationSpeed) + TargetRotationAngle;
        EndRot = StartRot * Quaternion.Euler(0,0,oscillation) ;
        transform.rotation = Quaternion.Lerp(transform.rotation, EndRot, Time.deltaTime * RotationSpeed);

    }

    private void RaycastDetection()
    {
        RaycastHit2D ZombieDetect = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), LineCastDistance, ZombieMask);
        TempColor = Color.green;
        //Debug.Log("Soldier cannot see a zombie");
        Debug.DrawLine(transform.position, transform.position - Vector3.left * -LineCastDistance, TempColor);
        if (ZombieDetect.collider != null)
        {
            TempColor = Color.red;
            Debug.DrawLine(transform.position, ZombieDetect.transform.position, TempColor);
            DetectedZombie = ZombieDetect.collider.gameObject.GetComponent<Enemy_Behaviour>();
            DetectedZombie.OnDeath();
        }
        else
        {
            TempColor = Color.green;
            //Debug.Log("Soldier cannot see a zombie");
            Debug.DrawLine(transform.position, transform.position - Vector3.left * -LineCastDistance, TempColor);


            //Doesn't see a zombie so nothing should happen at the moment
        }
    }

    private void DeactivateTurret()
    {
        IsActivated = false;
        Debug.Log("Turret Deactivated");
        EndRot = StartRot * Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, StartRot, Time.deltaTime * RotationSpeed);
    }
}
