using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{

    
	[Tooltip("Current players speed")] public float currentSpeed;

	[Tooltip("The higher the number the faster it will stop")] public float deaccelerationSpeed = 15.0f;

	[Tooltip("Force that is applied when moving forward or backward")] public float accelerationSpeed = 50000.0f;

	[Tooltip("Force that moves player into jump")] public float jumpForce = 500.0f;


    Rigidbody rb;

    private void Awake(){
		rb = GetComponent<Rigidbody>();
	}

    private Vector3 getInput()
    {
        Vector2 movementInput;
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(movementInput.x ,0, movementInput.y);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1.5f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1.5f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1.5f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1.5f;

        return moveDir;
    }

    private void FixedUpdate(){
        if(!IsLocalPlayer) return;

        Vector3 input = getInput();

        if(IsServer){
            Move(input);
        }
        else if (IsClient)
        {
            MoveServerRpc(input);
        }

    }

    private void Update()
    {
        if(!IsLocalPlayer) return;

        if(Input.GetKeyDown (KeyCode.Space)){
            if(IsServer){
                Jumping();
            } 
            else if(IsClient) {
                JumpingServerRpc();
            }
        }
        
    }

    private void Jumping()
    {
        rb.AddRelativeForce (Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Crouching()
    {

    }


    private void Move(Vector3 moveDir){
        Vector2 horizontalMovement;
        Vector3 slowDownRef;
        currentSpeed =  rb.velocity.magnitude;

        horizontalMovement = new Vector2 (rb.velocity.x, rb.velocity.z);
		rb.velocity = new Vector3 (
			horizontalMovement.x,
			rb.velocity.y,
			horizontalMovement.y
		);

        slowDownRef = Vector3.zero;  
        rb.velocity = Vector3.SmoothDamp(rb.velocity,
            new Vector3(0,rb.velocity.y,0),
            ref slowDownRef,
            deaccelerationSpeed);

        rb.AddRelativeForce (moveDir.x * accelerationSpeed * Time.deltaTime, 0, moveDir.z * accelerationSpeed * Time.deltaTime);

        if (moveDir.x != 0 || moveDir.z != 0) {
			deaccelerationSpeed = 0.5f;
		} else {
			deaccelerationSpeed = 0.1f;
		}

    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 moveDir)
    {
        Move(moveDir);
    }

    [ServerRpc]
    private void JumpingServerRpc()
    {
        Jumping();
    }

    [ServerRpc]
    private void CrouchingServerRpc()
    {
        Crouching();
    }


	[Header("Player SOUNDS")]
	[Tooltip("Jump sound when player jumps.")]
	public AudioSource _jumpSound;
	[Tooltip("Sound while player makes when successfully reloads weapon.")]
	public AudioSource _freakingZombiesSound;
	[Tooltip("Sound Bullet makes when hits target.")]
	public AudioSource _hitSound;
	[Tooltip("Walk sound player makes.")]
	public AudioSource _walkSound;
	[Tooltip("Run Sound player makes.")]
	public AudioSource _runSound;

}
