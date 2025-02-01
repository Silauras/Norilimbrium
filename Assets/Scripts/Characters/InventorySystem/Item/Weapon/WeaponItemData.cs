using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem.Weapon
{
    [CreateAssetMenu(menuName = "Item/Weapon/Weapon", fileName = "WeaponItemData", order = 1)]
    public class WeaponItemData : ItemData, IHandEquipment
    {
        [Header("WeaponData")] public int damage;
        public int duration;
        public int cooldown;

        public int staminaCost;
        public bool twoHanded { get; set; }
    

        public int manaCost;
        public int resonanceCost;
        public bool autoReuse;
        public int kickback;
        public float useTimeInSeconds;
    
        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}