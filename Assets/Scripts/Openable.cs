using System.Collections.Generic;
using UnityEngine;

public class Openable : Interactable
{
    [SerializeField] private List<Item> items; // The list of items inside the chest
    public bool isOpened;
    private Animator _animator;

    private void Start()
    {
        items.Add(new Item {itemType = Item.ItemType.Coin, amount = 1});
        items.Add(new Item {itemType = Item.ItemType.HealthPotion, amount = 1});
        items.Add(new Item {itemType = Item.ItemType.Sword, amount = 1});
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        if (!playerInRange)
            return;

        if (isOpened)
            return;

        Debug.Log("Opening chest!");
        foreach (var item in items)
        {
            ItemGround.DropItem(transform.position + new Vector3(0, .5f, 0), item);
        }

        _animator.SetTrigger("opening");
        isOpened = true;
    }

    private void ChestOpen()
    {
        _animator.SetBool("open", true);
    }
}