
using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem
{
    [System.Serializable]
    public class Item
    {
        public ItemData data;
        public int currentStackSize;
        
        public Item(ItemData data)
        {
            this.data = data;
            this.currentStackSize = 1;
        }

        public Item Clone()
        {
            return new Item(data) { currentStackSize = this.currentStackSize };
        }
    }
}