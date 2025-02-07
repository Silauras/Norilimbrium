using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Characters
{
    public class SwayBobComponent : MonoBehaviour
    {
        public CharacterController cc;
        
        [Header("Settings")] public bool sway = true;
        public bool swayRotation = true;
        public bool bobOffset = true;
        public bool bobSway = true;

        public float smooth = 10f;
        public float smoothRotation = 12f;

        [Header("Sway")] 
        public float swayStep = 0.01f;
        public float maxSwayStepDistance = 0.2f;
        private Vector3 _swayPosition = Vector3.zero;

        [Header("Sway Rotation")] 
        public float rotationStep = 4f;
        public float maxRotationStep = 5f;
        private Vector3 _swayEulerRotation = Vector3.zero;
        
        private Vector2 _lookInput = Vector2.zero;
        private Vector2 _moveInput = Vector2.zero;

        [Header("Bobbing")] 
        private float _speedCurve;

        private float CurveSin => Mathf.Sin(_speedCurve * 2); 
        // 2 for making V curve with cos in bob offset calculations
        private float CurveCos => Mathf.Cos(_speedCurve);

        public Vector3 travelLimit = Vector3.one * 0.025f;
        public Vector3 bobLimit = Vector3.one * 0.01f;
        private Vector3 _bobPosition = Vector3.zero;

        [Header("Bob Rotation")] 
        public Vector3 multiplier;
        private Vector3 _bobEulerRotation = Vector3.zero;

        public void Update()
        {
            _lookInput = InputSystem.actions.FindAction("Look").ReadValue<Vector2>();
            _moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
            Sway();
            SwayRotation();
            _speedCurve += Time.deltaTime * (cc.isGrounded ? cc.velocity.magnitude : 1) + 0.01f;
            BobOffset();
            BobRotation();

            CompositePositionRotation();
        }

        private void Sway()
        {
            if (!sway)
            {
                _swayPosition = Vector3.zero;
                return;
            }

            Vector3 invertLook = _lookInput * -swayStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxSwayStepDistance, maxSwayStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxSwayStepDistance, maxSwayStepDistance);

            _swayPosition = invertLook;
        }

        private void SwayRotation()
        {
            if (!swayRotation)
            {
                _swayEulerRotation = Vector3.zero;
                return;
            }

            Vector3 invertLook = _lookInput * -rotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

            _swayEulerRotation = new Vector3(invertLook.y, invertLook.x, invertLook.x);
        }

        void BobOffset()
        {
            if (!bobOffset)
            {
                _bobPosition = Vector3.zero;
                return;
            }
            
            _bobPosition.x = (CurveCos * bobLimit.x * (cc.isGrounded ? 1 : 0))
                            - (_moveInput.x * travelLimit.x);
            _bobPosition.y = (CurveSin * bobLimit.y)
                            - (cc.velocity.y * travelLimit.y);
            _bobPosition.z = -(_moveInput.y * travelLimit.z);
        }

        void BobRotation()
        {
            if (!bobSway)
            {
                _bobEulerRotation = Vector3.zero;
                return;
            }

            _bobEulerRotation.x = (_moveInput != Vector2.zero
                ? multiplier.x * (Mathf.Sin(2 * _speedCurve))
                : multiplier.x * (Mathf.Sin(2 * _speedCurve) / 2)); //pitch

            _bobEulerRotation.y = (_moveInput != Vector2.zero ? multiplier.y * CurveCos : 0); //yaw
            _bobEulerRotation.z = (_moveInput != Vector2.zero ? multiplier.z * CurveCos * _moveInput.x : 0);
        }

        void CompositePositionRotation()
        {
            transform.localPosition =
                Vector3.Lerp(transform.localPosition,
                    _swayPosition + _bobPosition,
                    Time.deltaTime * smooth);

            transform.localRotation =
                Quaternion.Slerp(transform.localRotation,
                    Quaternion.Euler(_swayEulerRotation) * Quaternion.Euler(_bobEulerRotation),
                    Time.deltaTime * smoothRotation);
        }
    }
}