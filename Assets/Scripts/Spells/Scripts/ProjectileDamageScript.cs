using Code.Scripts.Characters.CharacterStatsComponents;
using UnityEngine;

namespace Spells.Scripts
{
    public class ProjectileDamageScript: MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision in script detected");
            var health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null)
            {
                var damageInstance = new Damage
                {
                    DamageType = DamageType.Fire,
                    DamageAmount = 10f
                };
                health.ChangeHealth(new[] { damageInstance });
            }

        }
    }
}