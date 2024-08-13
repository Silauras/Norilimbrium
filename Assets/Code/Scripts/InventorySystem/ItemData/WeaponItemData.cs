using UnityEngine;


[CreateAssetMenu(menuName = "Item/Weapon/Weapon", fileName = "WeaponItemData", order = 1)]
public class WeaponItemData : ItemData
{
    [Header("WeaponData")] public int damage;
    public int duration;
    public int cooldown;

    public int staminaCost;
    public bool needTwoHands;
    public bool autoReuse;
    public int kickback;
    public float useTimeInSeconds;
}