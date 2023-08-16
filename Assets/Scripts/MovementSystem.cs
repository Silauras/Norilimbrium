using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementSystem : MonoBehaviour
{
    private Rigidbody2D _rb;

    public float acceleration = 20f; // Ускорение
    public float maxSpeed = 10f; // Максимальная скорость
    public float runMultiplier = 1.5f; // Множитель скорости при беге
    public KeyCode[] runKeys = { KeyCode.LeftShift, KeyCode.Joystick1Button3 };

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var movement = new Vector2(Input.GetAxis("rightMovement"), Input.GetAxis("upMovement"));
        movement.Normalize();

        var isRunning = runKeys.Any(Input.GetKey);

        var resultSpeed = isRunning ? runMultiplier * maxSpeed : maxSpeed;

        Vector2 targetVelocity = movement * resultSpeed;

        var velocity = _rb.velocity;
        var accelerationVector = (targetVelocity - velocity) * acceleration * Time.fixedDeltaTime;
        velocity += accelerationVector;
        _rb.velocity = velocity;

        // Ограничение максимальной скорости
        _rb.AddForce(Vector2.ClampMagnitude(velocity, maxSpeed));
    }
}