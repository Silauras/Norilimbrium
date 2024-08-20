using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int maxHeath;
    public int currentHeath;
    public bool isDead;
    public Action isDied;

    [NonSerialized] public ArmorComponent ArmorComponent;

    private void Start()
    {
        isDead = currentHeath > 0;
    }

    public void ChangeHealth(Damage[] damages)
    {
        if (isDead) return;
        var totalAmount = 0;
        if (ArmorComponent != null)
        {
            damages = ArmorComponent.RecalculateAmountOfDamage(damages);
            totalAmount += damages.Sum(damage => damage.DamageAmount);
        }
        currentHeath += totalAmount;
        currentHeath = math.clamp(currentHeath, 0, maxHeath);
        if (currentHeath != 0) return;
        isDead = true;
        isDied();
        currentHeath = 0;
    }
}

public struct Damage
{
    public DamageType DamageType;
    public int DamageAmount;
}