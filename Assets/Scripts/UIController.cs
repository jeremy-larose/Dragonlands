using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject characterScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadCharacterScreen();
        }
    }

    private void LoadCharacterScreen()
    {
        characterScreen.SetActive(!characterScreen.gameObject.activeInHierarchy);
    }
}
