using UnityEngine;

public class GroundItemSpawner : MonoBehaviour
{
    public Item item;

    private void Awake()
    {
        ItemGround.SpawnItemGround(transform.position, item);
        Destroy(gameObject);
    }
}