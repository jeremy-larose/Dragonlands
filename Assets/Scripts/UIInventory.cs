using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory _inventory;
    private Transform _itemSlotContainer;
    private Transform _itemSlotTemplate;
    private Player player;

    private void Awake()
    {
        _itemSlotContainer = transform.Find("itemSlotContainer");
        _itemSlotTemplate = _itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in _itemSlotContainer)
        {
            if (child == _itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        var x = 0;
        var y = 0;
        var itemSlotCellSize = 70f;
        foreach (var item in _inventory.GetItemList())
        {
            var itemSlotRectTransform = Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // Use Item
                _inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop Item -- creating a new duplicate because item is a class instead of a struct.
                var duplicateItem = new Item {itemType = item.itemType, amount = item.amount};
                _inventory.RemoveItem(item);
                ItemGround.DropItem(player.GetPosition(), duplicateItem);
            };
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            var image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            var uiText = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();

            if (item.amount > 1)
                uiText.SetText($"x{item.amount.ToString()}");
            else
                uiText.SetText("");

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}