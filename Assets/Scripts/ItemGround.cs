using UnityEngine;

public class ItemGround : Interactable
{
    [SerializeField] private Item _item;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public static ItemGround SpawnItemGround(Vector3 position, Item item)
    {
        var transform = Instantiate(GameAssets.i.pfGroundItem, position, Quaternion.identity);

        var itemGround = transform.GetComponent<ItemGround>();
        itemGround.SetItem(item);
        return itemGround;
    }

    public static ItemGround DropItem(Vector3 dropPosition, Item item)
    {
        var randomDir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        var itemGround = SpawnItemGround(dropPosition + randomDir, item);
        itemGround.GetComponent<Rigidbody>().AddForce(randomDir, ForceMode.Impulse);
        return itemGround;
    }

    public void SetItem(Item item)
    {
        _item = item;
        _spriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem()
    {
        return _item;
    }


    public override void Interact()
    {
        if (playerInRange)
        {
            Debug.Log("Player is in range.");
        }

        if (player)
        {
            Debug.Log("[ItemGround] Adding item to inventory!");
            player.inventory.AddItem(GetItem());
            DestroySelf();
        }
        else
        {
            Debug.Log("[ItemGround] Player was null.");
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}