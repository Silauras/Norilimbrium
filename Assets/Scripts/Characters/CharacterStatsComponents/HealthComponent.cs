using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Scripts.Characters.CharacterStatsComponents
{
    public class HealthComponent : MonoBehaviour
    {
        public float maxHeath = 100f;
        public float currentHeath = 100f;
        public bool isDead = false;
        public Action isDied;

        [NonSerialized] public ArmorComponent ArmorComponent;

        private void Start()
        {
            isDead = currentHeath < 0;
        }

        public void ChangeHealth(Damage[] damages)
        {
            if (isDead) return;
            var totalAmount = 0f;
            if (ArmorComponent != null)
            {
                damages = ArmorComponent.RecalculateAmountOfDamage(damages);
            }
            totalAmount += damages.Sum(damage => damage.DamageAmount);
            Debug.Log($"totalDamage is {totalAmount}" );
            currentHeath -= totalAmount;
            currentHeath = math.clamp(currentHeath, 0f, maxHeath);
            if (currentHeath > 0f) return;
            isDead = true;
            isDied();
            currentHeath = 0;
        }
    }

    public struct Damage
    {
        public DamageType DamageType;
        public float DamageAmount;
    }
}