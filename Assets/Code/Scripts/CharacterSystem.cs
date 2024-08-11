using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterSystem : MonoBehaviour
{
    private float maxStamina = 100f;
    private float currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
    }
}
