using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell.asset", menuName = "Attack/Spell")]
public class Spell : AttackDefinition
{
    public Projectile projectileToFire;
    public float projectileSpeed;

    public void Cast(GameObject caster, Vector3 hotSpot, Vector3 target, int layer)
    {
        // fire projectile
        Projectile projectile = Instantiate(projectileToFire, hotSpot, quaternion.identity);
        projectile.Fire(caster, target, projectileSpeed, range);

        // set projectile's collision layer
        projectile.gameObject.layer = layer;

        // listen to projectile's collided event
        projectile.ProjectileCollided += OnProjectileCollided;
    }

    private void OnProjectileCollided(GameObject caster, GameObject target)
    {
        // attack landed on target, create attack and attack the target

        // make sure the caster and the target are still alive
        if (caster == null || target == null)
            return;

        // create the attack
        var casterStats = caster.GetComponent<Character>();
        var targetStats = target.GetComponent<Character>();

        var attack = CreateAttack(casterStats, targetStats);

        // send attack to all attackable behaviors of the target
        var attackables = target.GetComponentsInChildren(typeof(IAttackable));

        foreach (var component in attackables)
        {
            var attackable = (IAttackable) component;
            attackable.OnAttack(caster, attack);
        }
    }
}