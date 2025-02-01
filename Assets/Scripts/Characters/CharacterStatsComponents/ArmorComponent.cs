using System.Collections.Generic;
using Code.Scripts.Characters.CharacterStatsComponents;
using UnityEngine;


[RequireComponent(typeof(HealthComponent))]
public class ArmorComponent : MonoBehaviour
{
    public Dictionary<DamageType, int> Armor;

    private void Start()
    {
        GetComponent<HealthComponent>().ArmorComponent = this;
    }

    public Damage[] RecalculateAmountOfDamage(Damage[] damages)
    {
        var resultDamages = new Damage[damages.Length];
        for (var damageIndex = 0; damageIndex < damages.Length; damageIndex++)
        {
            resultDamages[damageIndex] = new Damage
            {
                DamageType = damages[damageIndex].DamageType,
                DamageAmount =
                    (int)(Armor.GetValueOrDefault(key: damages[damageIndex].DamageType, defaultValue: 0) * 0.01f *
                        damages[damageIndex].DamageAmount + damages[damageIndex].DamageAmount)
            };
        }

        return resultDamages;
    }
}