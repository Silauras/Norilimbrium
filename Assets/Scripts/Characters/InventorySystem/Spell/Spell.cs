using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem.Spell
{
    public class Spell : IHandEquipment
    {
        
        public int ManaCost;
        public int ResonanceCost;
        public int StaminaCost;
        public bool twoHanded { get; set; }
        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}