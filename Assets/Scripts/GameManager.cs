using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> NPCList;
    public GameObject player;
    private Character playerCharacter;
    private GameObject _audioManager;
    
    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy( gameObject );
        }

        instance = this;
        DontDestroyOnLoad( gameObject );
    }
    #endregion
    
    void Start()
    {
        player = GameObject.Find( "Player");
        playerCharacter = player.GetComponent<Character>();

        TimeSystem.OnTick += delegate
        {
        };

        TimeSystem.OnTick_5 += delegate
        {
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log( "[GameManager] Forcing the player to take damage!");
            playerCharacter.TakeDamage( Dice.Roll( 1, 8 ));
        }
    }
}
