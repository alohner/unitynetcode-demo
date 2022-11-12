using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool grounded;
    public float playerSpeed = 2f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump")){
            jumpInput = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        grounded = controller.isGrounded;
        if (grounded && playerVelocity.y < 0){
            playerVelocity.y = 0f;
        }
        
        Vector3 move = new Vector3(-verticalInput, 0 , horizontalInput);
        controller.Move(move * Time.deltaTime * playerSpeed);
        
        if (jumpInput && grounded){
            jumpInput = false;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
    }
}
