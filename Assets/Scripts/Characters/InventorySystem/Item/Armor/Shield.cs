using Code.Scripts.Characters.InventorySystem;

namespace Characters.InventorySystem.Item.Armor
{
    public class Shield: ArmorItemData, IHandEquipment
    {
        public bool twoHanded { get; set; }
        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}