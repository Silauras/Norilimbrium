using System;
using Characters.InventorySystem.Item.Armor;
using Code.Scripts.Characters.CharacterStatsComponents;
using Code.Scripts.Characters.InventorySystem;
using Code.Scripts.Characters.InventorySystem.Spell;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Characters.InventorySystem
{
    [RequireComponent(typeof(StaminaComponent))]
    public class CombatComponent: MonoBehaviour
    {
        [SerializeField] public IHandEquipment rightHandEquipment;
        [SerializeField] public IHandEquipment leftHandEquipment ;

        private ManaComponent _manaComponent;
        private ResonanceComponent _resonanceComponent;
        private StaminaComponent _staminaComponent;

        public void Start()
        {
            _manaComponent = GetComponent<ManaComponent>();
            _resonanceComponent = GetComponent<ResonanceComponent>();
            _staminaComponent = GetComponent<StaminaComponent>();
        }

        public bool CheckRightHandEquipment()
        {
            switch (rightHandEquipment)
            {
                case Spell spell:
                    return _manaComponent.currentMana > spell.ManaCost &&
                           _resonanceComponent.currentResonance > spell.ResonanceCost &&
                           _staminaComponent.currentStamina > spell.StaminaCost;
                case MeleeWeaponItemData weapon:
                    return _staminaComponent.currentStamina > weapon.staminaCost;
                case RangeWeaponItemData weapon:
                    return _staminaComponent.currentStamina > weapon.staminaCost;
                case Shield:
                    break;  
                default:
                    throw new ArgumentException("Unknown equipment type");
            }
            return false;
        }
        
        public bool CheckLeftHandEquipment()
        {
            switch (leftHandEquipment)
            {
                case Spell spell:
                    return _manaComponent.currentMana > spell.ManaCost &&
                           _resonanceComponent.currentResonance > spell.ResonanceCost &&
                           _staminaComponent.currentStamina > spell.StaminaCost;
                case MeleeWeaponItemData weapon:
                    return _staminaComponent.currentStamina > weapon.staminaCost;
                case RangeWeaponItemData weapon:
                    return _staminaComponent.currentStamina > weapon.staminaCost;
                case Shield:
                    break;  
                default:
                    throw new ArgumentException("Unknown equipment type");
            }
            return false;
        }
        
        
        public void UseRightHandEquipment(IHandEquipment rightHandEquipment)
        {
            
        }

        public void UseLeftHandEquipment(IHandEquipment leftHandEquipment)
        {
            
        }
    }
}