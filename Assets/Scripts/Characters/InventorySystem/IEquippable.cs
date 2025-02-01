using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem
{
    public interface IHandEquipment
    {
        bool twoHanded { get; }
        void Use();
    }
}