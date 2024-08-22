using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{

    void PlayerMovement()
    {
        float horizontal = 0;
        float vertical = 0;
        float GridSize = 1.0f;
        bool MovingUp = true;
        KeyCode DownKeyPressed = KeyCode.S;
        KeyCode UpKeyPressed = KeyCode.W;
        Vector3 NewPos;

        if (Input.GetKeyDown(DownKeyPressed))
        {
            NewPos = transform.position - new Vector3(0, GridSize, 0);
            transform.position = NewPos;
        }

        if (Input.GetKeyDown(UpKeyPressed))
        {
            NewPos = transform.position + new Vector3(0, GridSize, 0);
            transform.position = NewPos;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
}
