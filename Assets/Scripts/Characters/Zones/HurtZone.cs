using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Characters.CharacterStatsComponents;
using UnityEngine;

namespace Code.Scripts.Zones
{
    public class HurtZone : MonoBehaviour
    {
        public DamageType damageType = DamageType.Physical;
        public float damageAmount = 10f;
        public float damageInterval = 0.5f;
        private readonly List<HealthComponent> _healthComponents = new();
        

        private void OnTriggerEnter(Collider other)
        {
            var healthComponent = other.GetComponent<HealthComponent>();
            if (healthComponent == null || _healthComponents.Contains(healthComponent)) return;
            _healthComponents.Add(healthComponent);
            if (_healthComponents.Count == 1)
            {
                InvokeRepeating(nameof(ApplyDamage), 0f, damageInterval);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var healthComponent = other.GetComponent<HealthComponent>();
            if (healthComponent == null || !_healthComponents.Contains(healthComponent)) return;
            _healthComponents.Remove(healthComponent);
            if (_healthComponents.Count == 0)
            {
                CancelInvoke(nameof(ApplyDamage));
            }
        }

        private void ApplyDamage()
        {
            foreach (var healthComponent in _healthComponents)
            {
                if (healthComponent == null) continue;
                var damage = new Damage
                {
                    DamageType = damageType,
                    DamageAmount = damageAmount
                };
                healthComponent.ChangeHealth(new[] { damage });
            }
        }
    }
}