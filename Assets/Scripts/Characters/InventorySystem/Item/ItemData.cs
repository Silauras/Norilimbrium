using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Item/Item", fileName = "ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [Header(header: "ItemData")] public int id;
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public Vector2Int size;
    public bool isStackable;
    public int maxStackSize;
    public float weight;
    public Rarity rarity;
    public Mesh mesh;
}