using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> NPCList;
    public GameObject player;
    
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
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find( "Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
