using System.Linq;
using Spells;
using UnityEngine;


public class SpellCaster : MonoBehaviour
{
    public SpellData FailCastSpell;
    public Transform castPoint; // Точка старта заклинания

    public void CastSpell(SpellData spell, Vector3 start, Vector3 end)
    {
        if (spell is null)
        {
            Debug.LogError("Attempted to cast null spell");
            return;
        }

        if (spell.form is null)
        {
            Debug.LogError("Attempted to cast spell with null form");
            return;
        }

        if (!isItPossibleToCast(spell.element, spell.form.type, spell.modifiers))
        {
            Debug.LogError($"Spell {spell.element.name} is not a valid spell");
            if (spell == FailCastSpell)
            {
                return;
            }

            CastSpell(FailCastSpell, start, end);
        }

        GameObject spellObject = null;
        switch (spell.form.type)
        {
            case SpellForm.Wall:
                Vector3 direction = end - start;
                direction.Normalize();
                Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                spellObject = Instantiate(spell.form.prefab, end, rotation);
                break;
            default:
                Debug.LogWarning("Форма заклинания не реализована: " + spell.form);
                break;
        }

        if (spellObject is not null)
        {
            foreach (var modifier in spell.modifiers)
            {
                switch (modifier.type)
                {
                    case SpellModifierType.Size:

                        spellObject.transform.localScale *= modifier.value * .01f;
                        break;
                    default:
                        Debug.LogWarning("This modifier is not yet supported");
                        break;
                }
            }
        }
    }

    public bool isItPossibleToCast(ElementData element, SpellForm formType, SpellModifier[] modifiers = null)
    {
        if (element is null)
        {
            Debug.LogWarning("Spell with null element is present");
            return false;
        }

        if (!element.compatibleForms.Contains(formType))
        {
            Debug.LogWarning("Spell with incompatible form is present");
            return false;
        }

        if (modifiers is null || modifiers.Length != 0)
        {
            return true;
        }

        if (modifiers.All(modifier => modifier.compatibleForms.Contains(formType))) return true;
        Debug.LogWarning("Spell with modifier incompatible to form is present");
        return false;
    }
}