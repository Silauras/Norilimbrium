using System;
using Code.Scripts.Characters.CharacterStatsComponents;
using UnityEngine;

namespace Spells.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class ProjectileScript : SpellScript
    {
        public Vector3 targetPosition;
        public float speed = 10f;
        public float damage = 10f;
        private float _distanceTraveled = 0f;
        private Vector3 _startPosition;
        private void Start()
        {
            _startPosition = transform.position;
            foreach (var spellModifier in modifiers)
            {
                switch (spellModifier.type)
                {
                    case SpellModifierType.Speed:
                        speed *= (spellModifier.value  * 0.01f);
                        break;
                    case SpellModifierType.Power:
                        damage *= (spellModifier.value * 0.01f) ;
                        break;
                    case SpellModifierType.Size:
                        transform.localScale *= (spellModifier.value * 0.01f);
                        break;
                    case SpellModifierType.Count:
                        break;
                    case SpellModifierType.Ricochet:
                        break;
                    case SpellModifierType.Homing:
                        break;
                    case SpellModifierType.Penetration:
                        break;
                    case SpellModifierType.Split:
                        break;
                    case SpellModifierType.Bounce:
                        break;
                    case SpellModifierType.Pierce:
                        break;
                    default:
                        throw new NotImplementedException("SpellModifier not supported by ProjectileScript");
                }
            }
        }

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            _distanceTraveled = Vector3.Distance(_startPosition, transform.position);

            if (_distanceTraveled >= 1000f)
            {
                Destroy(gameObject);
            }

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Explode();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision detected");
            var health = other.gameObject.GetComponent<HealthComponent>();
            if (health != null)
            {
                var damageInstance = new Damage
                {
                    DamageType = DamageType.Fire,
                    DamageAmount = damage
                };
                health.ChangeHealth(new[] { damageInstance });
            }

            Activate();
        }

        void Explode()
        {
            Debug.Log("Projectile exploded!");
            Destroy(gameObject);
        }

        public override void Activate()
        {
            Explode();
        }
    }
}