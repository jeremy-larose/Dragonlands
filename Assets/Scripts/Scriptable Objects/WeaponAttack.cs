using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
public class WeaponAttack : AttackDefinition
{
    public Rigidbody weaponPrefab;

    public void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        // Check if defender is in range of the attacker
        if (Vector3.Distance(attacker.transform.position, defender.transform.position) > range)
            return;

        // Check if the defender is in front of the player
        if (!attacker.transform.IsFacingTarget(defender.transform))
        {
            Debug.Log("[WeaponAttack] Defender is not in front of the player.");
            return;
        }

        // the attack will now connect
        var attackerStats = attacker.GetComponent<Character>();
        var defenderStats = defender.GetComponent<Character>();

        var attack = CreateAttack(attackerStats, defenderStats);
        var attackables = defender.GetComponentsInChildren(typeof(IAttackable));

        foreach (IAttackable attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }
    }
}