using UnityEngine;

public class AttackedForce : MonoBehaviour, IAttackable
{
    public float forceToAdd;
    private Rigidbody _myRigidbody;

    private void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        var forceDirection = transform.position - attacker.transform.position;
        forceDirection.y += .5f;
        forceDirection.Normalize();

        _myRigidbody.AddForce(forceDirection * forceToAdd);
    }
}