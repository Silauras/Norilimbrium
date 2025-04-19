using Code.Scripts.Characters.InventorySystem.Spell;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Spells
{
    public class SpellBuilder
    {
        #region Singleton

        private static SpellBuilder Instance;

        public static SpellBuilder GetInstance()
        {
            return Instance ??= new SpellBuilder();
        }

        private SpellBuilder()
        {
        }

        #endregion

        public SpellCreator newSpell()
        {
            return new SpellCreator();
        }

        public class SpellCreator

        {
            private ElementType _elementType;
            private SpellForm _form;
            private SpellModifier[] _modifiers;

            internal SpellCreator()
            {
            }

            public SpellCreator Element(ElementType elementType)
            {
                _elementType = elementType;
                return this;
            }

            public SpellCreator Form(SpellForm form)
            {
                _form = form;

                return this;
            }

            public SpellCreator Modifiers(params SpellModifier[] modifiers)
            {
                _modifiers = modifiers;
                return this;
            }

            public SpellData createAndValidateSpellData()
            {

                SpellData data = ScriptableObject.CreateInstance<SpellData>();
                data.element = ScriptableObject.CreateInstance<ElementData>();
                data.element.elementType = _elementType;
                data.element.isUnique = false;
                data.form = ScriptableObject.CreateInstance<SpellFormData>();
                data.form.type = _form;
                switch (data.form.type)
                {
                    case SpellForm.Projectile:
                        data.form.prefab = Resources.Load<GameObject>("Prefabs/Spells/Forms/ProjectileSpell");
                        break;
                }

                return data;
            }
        }
    }
}