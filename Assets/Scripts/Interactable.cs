using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1f; // How close to interact with the object
    public Transform interactionTransform;
    public bool playerInRange = false;
    private Transform player;


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

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    // This method is meant to be overridden
    public virtual void Interact()
    {
        Debug.Log($"Interacting with {transform.name}");
    }
}