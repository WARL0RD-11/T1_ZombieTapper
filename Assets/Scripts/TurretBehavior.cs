using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DumpsterBehavior : MonoBehaviour
{
    [SerializeField]
    private LayerMask ZombieMask;

    [SerializeField]
    private float TargetRotationAngle;

    private float LineCastDistance;
    private GameManager GameManager;
    private Color TempColor;
    //private Vector3 TempRotation = new Vector3(0.0f, 0.0f, -45.0f);
    private float RotationSpeed = 2f;
    private float StartAngle;
    private bool IsRotatingForward = true;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        LineCastDistance = 100.0f;
        TargetRotationAngle = 45.0f ;

        StartAngle = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetGameStatus())
        {
            RotationBouncer();
        }

    }

    private void RotationBouncer()
    {
        RaycastDetection();
        float CurrentAngle = transform.eulerAngles.z;
        if (IsRotatingForward)
        {
            if(CurrentAngle < StartAngle + TargetRotationAngle)
            {
                transform.Rotate(0,0,TargetRotationAngle * RotationSpeed * Time.deltaTime);
            }
            else
            {
                IsRotatingForward = false;
            }
        }
        else
        {
            if (CurrentAngle > StartAngle)
            {
                transform.Rotate(0, 0, TargetRotationAngle * -RotationSpeed * Time.deltaTime);
            }
            else
            {
                IsRotatingForward = true;
            }
        }
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
            //Debug.Log("Turret deteced a zombie");
            Debug.DrawLine(transform.position, ZombieDetect.transform.position, TempColor);
        }
        else
        {
            //then set the debug line color to green and go off screen
            TempColor = Color.green;
            //Debug.Log("Soldier cannot see a zombie");
            //Debug.DrawLine(transform.position, transform.position - Vector3.left * -LineCastDistance, TempColor);


            //Doesn't see a zombie so nothing should happen at the moment
        }
    }
}
