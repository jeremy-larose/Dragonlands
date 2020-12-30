using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> NPCList;
    public GameObject player;
    public GameObject enemyPrefab;
    public Material[] skins;
    private GameObject _audioManager;
    private int _skinIndex;
    private Character playerCharacter;

    private void Start()
    {
        playerCharacter = player.GetComponent<Character>();

        TimeSystem.OnTick += delegate { };
        TimeSystem.OnTick_5 += delegate { };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("[GameManager] Forcing the player to take damage!");
            playerCharacter.TakeDamage(Dice.Roll(1, 8));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemyPrefab.GetComponentInChildren<SpriteRenderer>().material = skins[_skinIndex];
            if (_skinIndex < skins.Length - 1)
                _skinIndex++;
            else
                _skinIndex = 0;

            NPCList.Add(enemy);
        }
    }

    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);

        player = GameObject.Find("Player");
    }

    #endregion
}