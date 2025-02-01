using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct ArmorData
{
    public DamageType DamageType;
    public int percentageOfSubtractedDamage;
}

[CreateAssetMenu(menuName = "Item/Armor/Armor", fileName = "ArmorItemData", order = 2)]
public class ArmorItemData : ItemData
{
    [Tooltip("Percentage of damage subtracted by type of Damage")]
    public ArmorData[] ArmorDatas;


    public Dictionary<DamageType, int> getArmorDictionary()
    {
        return ArmorDatas.ToDictionary(armorData => armorData.DamageType,
            armorData => armorData.percentageOfSubtractedDamage);
    }
}