using Code.Scripts.Characters.InventorySystem.Weapon;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Item/Weapon/Range Weapon", fileName = "RangeWeaponItemData")]
public class RangeWeaponItemData : WeaponItemData
{
    public int weaponRange;
    [FormerlySerializedAs("projectile")] 
    public ProjectileItemData projectileItemData;
    public int accuracy;
}