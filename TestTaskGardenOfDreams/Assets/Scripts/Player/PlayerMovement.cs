using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Joystick joystick;
    [SerializeField, Range(0, 5)] float speed;
    protected Vector3 direct;



    void FixedUpdate()
    {
        direct.x = Input.GetAxis("Horizontal");
        direct.y = Input.GetAxis("Vertical");
        if (joystick.Horizontal + joystick.Vertical != 0)
        {
            direct = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
        }
        Debug.Log(direct);
        Move(direct.normalized);
    }

    private void Move(Vector3 direct)
    {
        playerBody.velocity = direct * speed;
    }
}
