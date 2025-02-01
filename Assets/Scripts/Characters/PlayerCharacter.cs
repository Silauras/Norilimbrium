using Code.Scripts.Characters.CharacterStatsComponents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Characters
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(StaminaComponent))]
    public class PlayerCharacter : MonoBehaviour
    {
        private CharacterController _characterController;
        private HealthComponent _healthComponent;
        private StaminaComponent _staminaComponent;
        [SerializeField] private Camera playerCamera;

        #region GENERAL_VARIABLES

        public float gravity = 9.81f;
        public float lookSpeed = 0.1f;
        private Vector3 _velocity;
        public bool godMode = false;

        #endregion


        #region MOVEMENT_VARIABLES

        [Header("Movement")] public float jumpSpeed = 8f;
        public float staminaForJump = 5f;
        public float sprintingSpeed = 45f;
        public float runningSpeed = 30f;
        public float walkSpeed = 15f;
        public float crouchSpeed = 17f;
        public float lookXLimit = 80f;
        public bool useStaminaForSprint = true;
        public float staminaForSprintPerSecond = 10f;
        public float staminaForRunPerSecond = 0.1f;
        private float _maxWalkSpeed;
        private bool _runningToggle;
        private bool _isSprinting;
        private bool IsCrouched { get; set; }
        private float _cameraRotationY = 0f;
        private Vector3 _moveDirection = Vector3.zero;

        #endregion

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _healthComponent = GetComponent<HealthComponent>();
            _staminaComponent = GetComponent<StaminaComponent>();

            _maxWalkSpeed = walkSpeed;
        }

        private void Update()
        {
            #region CROUCH_INPUT
            if (InputSystem.actions["Crouch"].WasPressedThisFrame())
            {
                IsCrouched = !IsCrouched;
            }
            #endregion
            #region RUN_INPUT
            if (InputSystem.actions.FindAction("Run").WasPressedThisFrame())
            {
                _runningToggle = !_runningToggle;
            }
            #endregion
            #region SPRINT_INPUT

            if (_staminaComponent.currentStamina > 20)
            {
                _isSprinting = InputSystem.actions.FindAction("Sprint").IsPressed();   
            }
            #endregion
            #region MOVEMENT_INPUT
            var forward = transform.TransformDirection(Vector3.forward);
            var right = transform.TransformDirection(Vector3.right);
            var currentDirection = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
            #endregion
            #region SPEED_AND_STAMINA_CALCULATION
            if (_isSprinting && _staminaComponent.currentStamina > 10)
            {
                IsCrouched = false;
                _maxWalkSpeed = sprintingSpeed;
                if (!godMode && useStaminaForSprint && currentDirection.magnitude > 0.1f)
                {
                    _staminaComponent.SubtractStamina(staminaForSprintPerSecond * Time.deltaTime * currentDirection.magnitude);
                }
            }
            else if (IsCrouched)
            {
                _maxWalkSpeed = crouchSpeed;
            }
            else if (_runningToggle && _staminaComponent.currentStamina > 5)
            {
                _maxWalkSpeed = runningSpeed;
                if (!godMode && useStaminaForSprint && currentDirection.magnitude > 0.1f)
                {
                    _staminaComponent.SubtractStamina(staminaForRunPerSecond * Time.deltaTime * currentDirection.magnitude);
                }
            }
            else
            {
                _runningToggle = false;
                _maxWalkSpeed = walkSpeed;
            }
            var currentSpeed = _maxWalkSpeed * currentDirection;
            var moveDirectionY = _moveDirection.y;
            _moveDirection = forward * currentSpeed.y + right * currentSpeed.x;
            #endregion
            #region JUMP_INPUT

            if (_characterController.isGrounded)
            {
                var jumpAction = InputSystem.actions.FindAction("Jump");
                
                if (jumpAction.IsPressed() && _staminaComponent.currentStamina > 10f){
                    if (IsCrouched)
                    {
                        IsCrouched = false;
                    }
                    else
                    {
                        _moveDirection.y = jumpSpeed;
                        _staminaComponent.SubtractStamina(staminaForJump);   
                    }
                }
                else
                {
                    _moveDirection.y = 0f;
                }
            }
            else
            {
                _moveDirection.y = moveDirectionY;
            }
            _moveDirection.y -= gravity * Time.deltaTime;
            #endregion
            _characterController.Move(_moveDirection * Time.deltaTime);
            #region MOUSE_INPUT

            var rotation = InputSystem.actions.FindAction("Look").ReadValue<Vector2>() * lookSpeed;
            _cameraRotationY -= rotation.y;
            _cameraRotationY = Mathf.Clamp(_cameraRotationY, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotationY, 0, 0);
            transform.rotation *= Quaternion.Euler(0, rotation.x, 0);

            #endregion
        }
    }
}