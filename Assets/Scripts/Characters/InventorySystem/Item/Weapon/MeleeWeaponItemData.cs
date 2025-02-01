using Code.Scripts.Characters.InventorySystem.Weapon;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Item/Weapon/Melee Weapon", fileName = "MeleeWeaponItemData", order = 2)]
public class MeleeWeaponItemData : WeaponItemData
{
    [FormerlySerializedAs("meleeDamageType")] public DamageType damageType;
}