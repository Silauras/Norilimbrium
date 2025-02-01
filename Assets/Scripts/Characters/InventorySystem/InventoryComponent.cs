using System.Collections.Generic;
using Characters.InventorySystem;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem
{
    public class InventoryComponent : MonoBehaviour
    {
        [ItemNotNull] public List<Item> items = new();
        public GameObject rightHand;
        public GameObject leftHand;
        public IHandEquipment rightHandEquipment;
        public IHandEquipment leftHandEquipment;

        private CombatComponent _combatComponent = null;

        private void Start()
        {
            _combatComponent = GetComponentInParent<CombatComponent>();
        }

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

        public void SetRightHandEquipment(IHandEquipment equipment)
        {
            if (equipment.twoHanded)
            {
                leftHandEquipment = null;
                if (_combatComponent)
                {
                    _combatComponent.leftHandEquipment = null;
                }
            }

            rightHandEquipment = equipment;
            if (_combatComponent)
            {
                _combatComponent.rightHandEquipment = equipment;
            }
        }

        public void SetLeftHandEquipment(IHandEquipment equipment)
        {
            if (equipment.twoHanded)
            {
                SetRightHandEquipment(equipment);
            }
            else
            {
                leftHandEquipment = equipment;
                if (_combatComponent)
                {
                    _combatComponent.leftHandEquipment = equipment;
                }
            }
        }
    }
}