using UnityEngine;

public class AttackedDebug : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        if (attack.IsCritical)
            Debug.Log("CRITICAL HIT!");

        Debug.Log($"{attacker.name} attacked {name} for {attack.Damage}");
    }
}