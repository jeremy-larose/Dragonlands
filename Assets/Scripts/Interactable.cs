using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1.5f; // How close to interact with the object
    public Transform interactionTransform;
    public bool playerInRange = false;
    public Player player;


    private void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<Player>();
        playerInRange = true;
    }

    public virtual void OnTriggerExit(Collider other)
    {
        player = null;
        playerInRange = false;
    }

    // This method is meant to be overridden
    public virtual void Interact()
    {
        Debug.Log($"Interacting with {transform.name}");
    }
}