using UnityEngine;

public class Weapon : MonoBehaviour
{
    public AttackDefinition attackDefinition;
    public Character wielder;
    private bool hitOnce = false;
    private Character targetCharacter;

    private void Awake()
    {
        wielder = GetComponentInParent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isAttackable = other.gameObject.GetComponent(typeof(IAttackable)) != null;

        if (!hitOnce)
        {
            if (isAttackable)
            {
                Debug.Log($"[Weapon] {other.name} is being attacked with {this.name}!");
                AttackTarget(other.gameObject);
                hitOnce = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hitOnce)
        {
            hitOnce = false;
        }
    }

    public void AttackTarget(GameObject target)
    {
        targetCharacter = target.GetComponent<Character>();
        var attack = attackDefinition.CreateAttack(wielder, targetCharacter);
        var attackables = target.GetComponentsInChildren(typeof(IAttackable));

        foreach (var component in attackables)
        {
            var attackable = (IAttackable) component;
            attackable.OnAttack(gameObject, attack);
        }
    }
}