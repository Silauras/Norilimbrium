using UnityEngine;

namespace Spells.Scripts
{
    public abstract class SpellScript: MonoBehaviour
    {
        public ElementType element;
        public SpellModifier[] modifiers;

        public abstract void Activate();
    }
}