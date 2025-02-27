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
        Beam,
        Explosion,
        Wall,
        Aura,
        Zone,
        Wave,
        Chain,
        Summon,
        SelfBuff,
        Debuff,
        Trap,
        Unique
    }

    public enum SpellModifierType
    {
        Speed, // Ускорение заклинания
        Power, // Усиление урона/эффекта
        Size, // Увеличение размеров заклинания
        Duration, // Длительность эффекта
        Count, // Количество создаваемых объектов (мультиспавн)
        Ricochet, // Рикошет
        Homing, // Самонаведение
        Penetration, // Пробивание целей
        Split, // Разделение на несколько снарядов
        Bounce, // Отскакивание от поверхностей
        Pierce, // Проникающее сквозь несколько врагов
        Spread, // Расширение зоны поражения
        Chargeable, // Возможность накопления силы перед выпуском
        Delayed, // Задержка перед срабатыванием
        GravityAffected, // Подверженность гравитации
        Linger, // Задержка эффекта в области
        Clone, // Создание дополнительной копии заклинания
        Unique, // Уникальный модификатор
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
        [SerializeReference]
        public SpellData nextSpellToCast;
    }
}