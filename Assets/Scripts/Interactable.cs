using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1f; // How close to interact with the object
    public Transform interactionTransform;
    private bool hasInteracted = false;

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    public virtual void Interact()
    {
        Debug.Log($"[Interactable]: Interacting with {transform.gameObject.name}");
    }
}