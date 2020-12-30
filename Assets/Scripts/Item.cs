using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Sword,
        HealthPotion,
        Coin
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            case ItemType.Sword: return GameAssets.i.swordSprite;
            case ItemType.Coin: return GameAssets.i.coinSprite;
            case ItemType.HealthPotion: return GameAssets.i.healthPotionSprite;
            default: return GameAssets.i.defaultSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            case ItemType.Sword:
                return false;

            case ItemType.HealthPotion:
            case ItemType.Coin:
            default:
                return true;
        }
    }
}