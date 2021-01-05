using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public AudioClip townMusic;
    public AudioClip overworldMusic;
    public AudioClip fireCrackle;
    public AudioClip battleMusic;

    public Transform pfCombatText;
    public Transform pfGroundItem;
    public Sprite defaultSprite;
    public Sprite swordSprite;
    public Sprite healthPotionSprite;
    public Sprite coinSprite;
    public Transform pfDialogueWindow;
    public Transform pfChatText;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }
}