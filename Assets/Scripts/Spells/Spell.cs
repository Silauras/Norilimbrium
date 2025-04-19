using System.Collections.Generic;
using Code.Scripts.Characters.InventorySystem.Spell;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "NewElement", menuName = "Spell System/Element")]
    public class ElementData : ScriptableObject
    {
        public string elementName;
        public Color baseColor;
        public Material baseMaterial;
        public Material emissionMaterial;
        public Texture specialTexture;
        public GameObject particleEffectPrefab;
        public string elementDescription;
        public float baseDamageMultiplier;
        public float manaCostModifier;
        public float resonanceCostModifier;
        public bool isUnique;

        public ElementType elementType;
        public ElementType[] incompatibleElements;
        public SpellForm[] compatibleForms;
    }

    public enum ElementType
    {
        Fire,
        Water,
        Air,
        Earth,
        Light,
        Darkness,
        Unique
    }

    [CreateAssetMenu(fileName = "New Spell Form", menuName = "Spell System/Spell Form")]
    public class SpellFormData : ScriptableObject
    {
        public SpellForm type;
        public GameObject prefab;
    }

    public enum SpellForm
    {
        Projectile,
        Bullet,
        Beam,
        Explosion,
        Wall,
        Aura,
        Zone,
        Wave,
        Summon,
        SelfBuff,
        Debuff,
        Trap,
        Unique
    }

    public enum SpellModifierType
    {
        Speed,
        Power,
        Size,
        Duration,
        Count,
        Ricochet,
        Homing,
        Penetration,
        Split,
        Bounce,
        Pierce,
        Spread,
        Chargeable,
        Delayed,
        Linger,
        Clone,
        Unique,
    }

    [CreateAssetMenu(fileName = "New Spell Data", menuName = "Spell System/Spell Modifier")]
    public class SpellModifier : ScriptableObject
    {
        public SpellModifierType type;
        public int value;
        public int minValue;
        public int maxValue;
        public SpellForm[] compatibleForms;

        public SpellModifier(SpellModifierType type, int value, int minValue, int maxValue)
        {
            this.type = type;
            this.value = Mathf.Clamp(value, minValue, maxValue);
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }

    [CreateAssetMenu(fileName = "New Spell Data", menuName = "Spell System/Spell Data")]
    public class SpellData : ScriptableObject
    {
        public ElementData element;
        public SpellFormData form;
        public SpellModifier[] modifiers;
        [SerializeReference] public SpellData nextSpellToCast;
    }
    public static class SpellCompatibilityMatrix
    {
        public static readonly Dictionary<ElementType, HashSet<SpellForm>> ElementToForms = new()
        {
            {
                ElementType.Fire, new HashSet<SpellForm>
                {
                    SpellForm.Projectile,
                    SpellForm.Bullet,
                    SpellForm.Beam,
                    SpellForm.Explosion,
                    SpellForm.Wall,
                    SpellForm.Aura,
                    SpellForm.Zone,
                    SpellForm.Wave,
                    SpellForm.Summon,
                    SpellForm.SelfBuff,
                    SpellForm.Debuff,
                }
            },
            {
                ElementType.Water, new HashSet<SpellForm>
                {
                    SpellForm.Projectile,
                    SpellForm.Bullet,
                    SpellForm.Explosion,
                    SpellForm.Wall,
                    SpellForm.Aura,
                    SpellForm.Zone,
                    SpellForm.Wave,
                    SpellForm.Summon,
                    SpellForm.Debuff,
                    SpellForm.Zone,
                    SpellForm.Trap,
                }
            },
            {
                ElementType.Air, new HashSet<SpellForm>
                {
                }
            },
            {
                ElementType.Earth, new HashSet<SpellForm>
                {
                    SpellForm.Projectile,
                    SpellForm.Bullet,
                    SpellForm.Wall,
                    SpellForm.Trap,
                }
            },
            {
                ElementType.Light, new HashSet<SpellForm>
                {
                    SpellForm.Beam,
                    SpellForm.Aura,
                    SpellForm.SelfBuff
                }
            },
            {
                ElementType.Darkness, new HashSet<SpellForm>
                {
                    SpellForm.Beam,
                    SpellForm.Debuff,
                    SpellForm.Wall,
                    SpellForm.Zone
                }
            },
            {
                ElementType.Unique, new HashSet<SpellForm>
                {
                    SpellForm.Unique
                }
            },
        };

        public static readonly Dictionary<SpellForm, HashSet<SpellModifierType>> FormToModifiers = new()
        {
            {
                SpellForm.Projectile,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Speed,
                    SpellModifierType.Power,
                    SpellModifierType.Size,
                    SpellModifierType.Count,
                    SpellModifierType.Ricochet,
                    SpellModifierType.Homing,
                    SpellModifierType.Penetration,
                    SpellModifierType.Split,
                    SpellModifierType.Bounce,
                    SpellModifierType.Pierce,

                }
            },
            {
                SpellForm.Bullet,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Speed,
                    SpellModifierType.Power,
                    SpellModifierType.Size,
                    SpellModifierType.Count,
                    SpellModifierType.Ricochet,
                    SpellModifierType.Homing,
                    SpellModifierType.Penetration,
                    SpellModifierType.Split,
                    SpellModifierType.Bounce,
                    SpellModifierType.Pierce,
                }
            },
            {
                SpellForm.Beam,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Duration,
                    SpellModifierType.Power,
                    SpellModifierType.Penetration,
                    SpellModifierType.Homing,
                    SpellModifierType.Split,
                    SpellModifierType.Bounce,
                    SpellModifierType.Pierce,
                    SpellModifierType.Clone,
                }
            },
            {
                SpellForm.Explosion,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Size,
                    SpellModifierType.Duration,
                    SpellModifierType.Spread
                }
            },
            {
                SpellForm.Wall,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Duration,
                    SpellModifierType.Size,
                    SpellModifierType.Linger
                }
            },
            {
                SpellForm.Aura,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Size,
                    SpellModifierType.Duration,
                    SpellModifierType.Linger
                }
            },
            {
                SpellForm.Zone,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Size,
                    SpellModifierType.Duration, SpellModifierType.Linger
                }
            },
            {
                SpellForm.Wave,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Speed,
                    SpellModifierType.Spread,
                    SpellModifierType.Power
                }
            },
            {
                SpellForm.Summon,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Count,
                    SpellModifierType.Clone,
                    SpellModifierType.Delayed
                }
            },
            {
                SpellForm.SelfBuff,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Duration,
                    SpellModifierType.Power,
                    SpellModifierType.Chargeable
                }
            },
            {
                SpellForm.Debuff,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Duration,
                    SpellModifierType.Power,
                    SpellModifierType.Spread
                }
            },
            {
                SpellForm.Trap,
                new HashSet<SpellModifierType>
                {
                    SpellModifierType.Delayed,
                    SpellModifierType.Size,
                    SpellModifierType.Linger
                }
            },
            {
                SpellForm.Unique, new HashSet<SpellModifierType>
                {
                        SpellModifierType.Unique
                }
            },
        };

        public static bool IsFormCompatibleWithElement(ElementType element, SpellForm form)
        {
            return ElementToForms.ContainsKey(element) && ElementToForms[element].Contains(form);
        }

        public static bool IsModifierCompatibleWithForm(SpellForm form, SpellModifierType modifier)
        {
            return FormToModifiers.ContainsKey(form) && FormToModifiers[form].Contains(modifier);
        }
    }
}