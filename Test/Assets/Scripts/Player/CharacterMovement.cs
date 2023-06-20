using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
public class CharacterMovement : NetworkBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private AudioListener listener;
    //private Vector3 playerVelocity;
    //private bool groundedPlayer;
    //[SerializeField] private float jumpHeight = 1.0f;
    //[SerializeField] private float gravityValue = -9.81f;

    [SerializeField] private float _rotationSpd = 1f;
    [SerializeField] private float _accumulatedRotation; 

    public CharacterController characterController;
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        //characterController = gameObject.AddComponent<CharacterController>();
        playerInput = new();
        playerInput.Enable();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vCam.Priority = 1;
        }
        else
        {
            vCam.Priority = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = playerInput.Player.Movement.ReadValue<Vector2>();
        if (IsServer && IsLocalPlayer)
        {
            Move(moveInput);
        }
        else if (IsClient && IsLocalPlayer)
        {
            MoveServerRPC(moveInput);
        }

        Vector2 mouseDelta = playerInput.Player.Look.ReadValue<Vector2>();
        if (IsServer && IsLocalPlayer)
        {
            LookAround(mouseDelta);
        }
        else if (IsClient && IsLocalPlayer)
        {
            LookAroundServerRPC(mouseDelta);
        }
    }

    private void Move(Vector2 _input)
    {
        Vector3 calcMove = _input.x * playerTransform.right + _input.y * playerTransform.forward;
        characterController.Move(calcMove * playerSpeed * Time.deltaTime);
    }

    private void LookAround(Vector2 _input)
    {
        float rotationAmount = _input.x * _rotationSpd;

        _accumulatedRotation += rotationAmount;

        transform.rotation = Quaternion.Euler(0, _accumulatedRotation, 0);

    }

    [ServerRpc]
    private void MoveServerRPC(Vector2 _input)
    {
        Move(_input);
    }

    [ServerRpc]
    private void LookAroundServerRPC(Vector2 _input)
    {
        LookAround(_input);
    }
}
