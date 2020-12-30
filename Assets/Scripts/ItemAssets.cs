using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public GameObject pfItemGround;
    public Sprite defaultSprite;
    public Sprite swordSprite;
    public Sprite healthPotionSprite;
    public Sprite coinSprite;
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}