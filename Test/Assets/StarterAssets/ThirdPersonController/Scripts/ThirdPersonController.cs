﻿using UnityEngine;
using UnityEditor;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using Unity.Netcode;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : NetworkBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public CinemachineVirtualCamera _cinemachineVirtualCamera;
        public AudioListener listener;
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        public bool Crouch = false;

        public float health = 100f;

        public bool isDeath = false;

        [Header("Aim & Shoot")]
        [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
        [SerializeField] private Transform debugTransform;
        [SerializeField] private Transform pfBulletProjectile;
        [SerializeField] private Transform spawnBulletLocation;
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private Transform VFXBlood;
        [SerializeField] AudioClip shootingSound;
        [SerializeField] private List<GameObject> spawnedMuzzle = new List<GameObject>();
        [SerializeField] private List<GameObject> spawnedBullet = new List<GameObject>();

        [Header("Bomb")]
        public GameObject smokeGrenadePrefab;
        public GameObject explosionGrenadePrefab;
        public float throwForce = 10f;
        public int grenadeDamage = 50;
        public float explosionRadius = 10f;
        public GameObject spawnGrenadeLoc;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private Animator anim;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            anim = GetComponent<Animator>();

        }

        private void Start()
        {
            GameStateManager.Instance.OnGameStateChange += Pause;

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsClient && IsOwner)
            {
                _playerInput = GetComponent<PlayerInput>();
                _playerInput.enabled = true;
            }
            if (IsOwner)
            {
                listener.enabled = true;
                _cinemachineVirtualCamera.Priority = 10;
            }
            else
            {
                _cinemachineVirtualCamera.Priority = 0;
            }
        }

        private void Update()
        {
            if (IsOwner)
            {
                //Vector3 mouseWorldPosition = Vector3.zero;
                //Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
                //Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
                //if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
                //{
                //    debugTransform.position = raycastHit.point;
                //    mouseWorldPosition = raycastHit.point;
                //}
                _hasAnimator = TryGetComponent(out _animator);

                JumpAndGravity();
                GroundedCheck();
                Move();
                Crouching();
                takeDamage();
                Die();
                Shoot();
                if (Input.GetKeyDown(KeyCode.G))
                {
                    ThrowSmokeGrenade();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ThrowExplosionGrenade();
                }
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            if (_input.sprint)
            {
                Crouch = true;
                anim.SetBool("Crouch", false);
            }
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        Crouch = true;
                        anim.SetBool("Crouch", false);
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void Crouching()
        {
            if (_input.crouch)
            {
                if (Crouch == true)
                {
                    Crouch = false;
                    anim.SetBool("Crouch", true);
                    _input.crouch = false;
                }
                else
                {
                    Crouch = true;
                    anim.SetBool("Crouch", false);
                    _input.crouch = false;
                }
            }
        }

        private void Shoot()
        {
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }
            if (_input.shoot)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletLocation.position).normalized;
                ShootServerRpc(aimDir);
                AudioSource.PlayClipAtPoint(shootingSound, spawnBulletLocation.position, 0.5f);
                _input.shoot = false;
            }
        }

        private void Pause(object sender, GameStateManager.GameStateEventArgs newGameState)
        {
            enabled = newGameState.state == GameStateManager.GameState.Gameplay;
        }

        private void takeDamage()
        {
            if (_input.takeDmg)
            {
                health -= 20f;
                _input.takeDmg = false;
                if (health == 0f)
                {
                    isDeath = true;
                }
            }

        }
        private void Die()
        {
            if (isDeath == true)
            {
                anim.SetBool("isDeath", true);
            }
        }

        private void Revive()
        {
            //New transform
            //...
            //...

            if (isDeath == true)
            {
                anim.SetBool("isDeath", false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BulletProjectile>() != null)
            {
                SpawnBloodOnCollideWBulletServerRpc();
                health -= 20f;
                Healthbar.Instance.UpdateHealthBar(health);
                if (health == 0f)
                {
                    isDeath = true;
                    NMLGameMultiplayer.Instance.PlayerDie();
                }
            }
        }
        private void ThrowSmokeGrenade()
        {
            ThrowSmokeGrenadeServerRpc();
        }
        private void ThrowExplosionGrenade()
        {
            // Instantiate the explosion grenade prefab
            ThrowExplosionGrenadeServerRpc();

            // Get the forward direction of the player's camera
            Vector3 throwDirection = spawnGrenadeLoc.transform.forward;

            // Apply the throw force to the explosion grenade in the throw direction
        }
        [ServerRpc]
        private void ShootServerRpc(Vector3 aimDir)
        {
            Transform bullet = Instantiate(pfBulletProjectile, spawnBulletLocation.position, Quaternion.LookRotation(aimDir, Vector3.up));
            bullet.GetComponent<NetworkObject>().Spawn();
        }
        [ServerRpc]
        private void SpawnBloodOnCollideWBulletServerRpc()
        {
            Transform bloodEffect = Instantiate(VFXBlood, transform.position, Quaternion.identity);
            bloodEffect.GetComponent<NetworkObject>().Spawn();
        }
        [ServerRpc]
        private void ThrowSmokeGrenadeServerRpc()
        {
            // Instantiate the smoke grenade prefab
            GameObject smokeGrenade = Instantiate(smokeGrenadePrefab, transform.position, Quaternion.identity);
            smokeGrenade.GetComponent<NetworkObject>().Spawn();

            // Get the forward direction of the player's camera
            Vector3 throwDirection = spawnGrenadeLoc.transform.forward;

            // Apply the throw force to the smoke grenade in the throw direction
            Rigidbody rb = smokeGrenade.GetComponent<Rigidbody>();
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        }
        [ServerRpc]
        private void ThrowExplosionGrenadeServerRpc()
        {
            GameObject explosionGrenade = Instantiate(explosionGrenadePrefab, transform.position, Quaternion.identity);
            explosionGrenade.GetComponent<NetworkObject>().Spawn();
            Rigidbody rb = explosionGrenade.GetComponent<Rigidbody>();
            Vector3 throwDirection = spawnGrenadeLoc.transform.forward;
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }
}