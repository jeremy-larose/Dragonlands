using UnityEngine;

public class Shopkeeper : Interactable
{
    [SerializeField] private string greetingText;
    [SerializeField] private AudioClip audioToPlay;

    private void Start()
    {
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public override void Interact()
    {
        //ChatText.Create( transform.position + new Vector3( 0f, 50f, 0f ), greetingText, gameObject.name, Color.cyan );
        AudioManager.instance.PlaySound(audioToPlay);
        Debug.Log("Interacting with Shopkeeper!");
    }
}