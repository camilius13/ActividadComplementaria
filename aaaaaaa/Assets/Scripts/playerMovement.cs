using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public float walkingSpeed = 12f;
    public float sprintSpeed = 12f;
    public float jumpHeigth = 3f;

    public float lerpTime;

    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Information")]
    Vector3 velocity;

    public float actualSpeed;
    [Header("Booleans")]
    
    public bool isGrounded;
    public bool isRunning;
    
    void Start()
    {
        
    }

    
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward *  z).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float targetSpeed = isRunning ? sprintSpeed : walkingSpeed;

        actualSpeed = Mathf.Lerp(actualSpeed,targetSpeed,lerpTime*Time.deltaTime);


        controller.Move(move * actualSpeed * Time.deltaTime);


        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity* Time.deltaTime);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }
}
