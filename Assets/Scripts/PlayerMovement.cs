using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Stopped at 17:30 of brackeys fps movement

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;

    public Transform playerRotation;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    Vector3 velocity;
    Vector3 dash;
    bool isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        playerController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButtonDown("Dash Left") && isGrounded)
        {
            // dash left
            dash -= transform.TransformDirection(Vector3.right * 3);

            Debug.Log(dash);
            playerController.Move(dash);
            dash = new Vector3();
        }

        if (Input.GetButtonDown("Dash Right") && isGrounded)
        {
            // dash right
            dash += transform.TransformDirection(Vector3.right * 3);

            Debug.Log(dash);
            playerController.Move(dash);
            dash = new Vector3();
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);

        dash = new Vector3();

        
    }

    
}
