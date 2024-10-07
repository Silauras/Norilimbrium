using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem
{
    public class InventoryComponent : MonoBehaviour
    {
        [ItemNotNull] public List<Item> items = new();
        public WeaponItemData rightHandWeapon;
        public WeaponItemData leftHandWeapon;
        
        public bool AddItem(Item item)
        {
            items.Add(item);
            return true;
        }

        public float GetTotalWeight()
        {
            float totalWeight = 0;
            items.ForEach(item => totalWeight += item.data.weight);
            return totalWeight;
        }
    }
}