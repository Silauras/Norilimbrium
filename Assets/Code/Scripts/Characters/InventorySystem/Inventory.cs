using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Characters.InventorySystem
{
    public class Inventory
    {
        public int width;
        public int height;
        public Item[,] grid;
        // GridInventory

        public Inventory(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new Item[width, height];
        }
        
        public bool AddItem(Item item, Vector2Int position)
        {
            if (!CanPlaceItem(item, position)) return false;
            for (var x = 0; x < item.data.size.x; x++)
            {
                for (var y = 0; y < item.data.size.y; y++)
                {
                    grid[position.x + x, position.y + y] = item;
                }
            }

            return true;
        }

        private bool CanPlaceItem(Item item, Vector2Int position)
        {
            if (position.x + item.data.size.x > width || position.y + item.data.size.y > height)
                return false;

            for (int x = 0; x < item.data.size.x; x++)
            {
                for (int y = 0; y < item.data.size.y; y++)
                {
                    if (grid[position.x + x, position.y + y] != null)
                        return false;
                }
            }

            return true;
        }

        public void RemoveItem(Item item)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid[x, y] == item)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        public Item GetItem(Vector2Int position)
        {
            return grid[position.x, position.y];
        }

        public bool AddItemToStack(Item item)
        {
            if (!item.data.isStackable) return false;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var existingItem = grid[x, y];
                    if (existingItem == null || existingItem.data.itemName != item.data.itemName ||
                        existingItem.currentStackSize >= existingItem.data.maxStackSize) continue;
                
                    var spaceLeft = existingItem.data.maxStackSize - existingItem.currentStackSize;
                    if (item.currentStackSize <= spaceLeft)
                    {
                        existingItem.currentStackSize += item.currentStackSize;
                        return true;
                    }
                    else
                    {
                        existingItem.currentStackSize = existingItem.data.maxStackSize;
                        item.currentStackSize -= spaceLeft;
                    }
                }
            }

            return false;
        }

        public float GetTotalWeight()
        {
            float totalWeight = 0;
            var countedItems = new HashSet<Item>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var item = grid[x, y];
                    if (item == null || countedItems.Contains(item)) continue;
                    totalWeight += item.data.weight * item.currentStackSize;
                    countedItems.Add(item);
                }
            }

            return totalWeight;
        }
    }
}