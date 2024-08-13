using UnityEngine;


[CreateAssetMenu(menuName = "Item/Weapon/Range Weapon", fileName = "RangeWeaponItemData")]
public class RangeWeaponItemData : WeaponItemData
{
    public int weaponRange;
    public Projectile projectile;
    public int accuracy;
}