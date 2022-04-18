using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stopped at 17:30 of brackeys fps movement

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;

    public float speed = 12f;
    public float gravity = -9.81f;

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        playerController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }
}
