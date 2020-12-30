using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private readonly List<Item> itemList;
    private readonly Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();

        AddItem(new Item {itemType = Item.ItemType.Sword, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.HealthPotion, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.Sword, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.Coin, amount = 1});


        Debug.Log($"[Inventory] Contains {itemList.Count}");
    }

    public event EventHandler OnItemListChanged;

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            var isItemAlreadyInInventory = false;
            foreach (var inventoryItem in itemList)
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    isItemAlreadyInInventory = true;
                }

            if (!isItemAlreadyInInventory) itemList.Add(item);
        }
        else
        {
            itemList.Add(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (var inventoryItem in itemList)
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }

            if (itemInInventory != null && itemInInventory.amount <= 0) itemList.Remove(itemInInventory);
        }
        else
        {
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}