using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerController : NetworkBehaviour
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

    private NetworkVariable<float> hInputNetwork = new NetworkVariable<float>(0f);
    private NetworkVariable<float> vInputNetwork = new NetworkVariable<float>(0f);
    private NetworkVariable<bool> jumpInputNetwork = new NetworkVariable<bool>(false);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update(){
        if (IsOwner){
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool j = Input.GetButtonDown("Jump");
            if (h != 0 || v != 0 || j){
                SendInputServerRPC(h,v,j);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        horizontalInput = hInputNetwork.Value;
        verticalInput = vInputNetwork.Value;
        jumpInput = jumpInputNetwork.Value;

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

    [ServerRpc]
    void SendInputServerRPC(float horizontal, float vertical, bool jump){
        //ici on peut faire des tests sur les valeurs envoyés par le client pour être sur qu'il n'y a pas de triche
        hInputNetwork.Value = horizontal;
        vInputNetwork.Value = vertical;
        jumpInputNetwork.Value = jump;
    }
}
