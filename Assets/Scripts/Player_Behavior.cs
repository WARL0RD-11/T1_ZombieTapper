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
        KeyCode KeyPressed = KeyCode.Space;
        Vector3 NewPos;

        if (Input.GetKeyDown(KeyPressed))
        {
            if ((transform.position.y >= -5.0f) && (transform.position.y < 5.0f))
            { 
                MovingUp = true;
                Debug.Log("TRUE");
            }
            else if (transform.position.y >= 5.0f)
            {
                MovingUp = false;
                Debug.Log("False");
            }

            if (MovingUp)
            {
                NewPos = transform.position + new Vector3(0, GridSize, 0);
            }
            else
            {
                NewPos = transform.position - new Vector3(0, GridSize, 0);
            }

            if (Input.GetKey(KeyCode.A))
            {
                horizontal -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontal += 1;
            }

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
