using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject characterScreen;
    public GameObject inventoryScreen;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) LoadCharacterScreen();

        if (Input.GetKeyDown(KeyCode.I)) LoadInventoryScreen();
    }

    private void LoadInventoryScreen()
    {
        inventoryScreen.SetActive(!inventoryScreen.gameObject.activeInHierarchy);
    }

    private void LoadCharacterScreen()
    {
        characterScreen.SetActive(!characterScreen.gameObject.activeInHierarchy);
    }
}