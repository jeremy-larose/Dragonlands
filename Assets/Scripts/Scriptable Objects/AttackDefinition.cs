using UnityEngine;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
public class AttackDefinition : ScriptableObject
{
    public float cooldown;
    public float range;
    public float minDamage;
    public float maxDamage;
    public float criticalMultiplier;
    public float criticalChance;
    public AudioClip attackSound;

    public Attack CreateAttack(Character attackerStats, Character defenderStats)
    {
        float coreDamage = attackerStats.GetDamage();
        coreDamage += Random.Range(minDamage, maxDamage);
        bool isCritical = Random.value < criticalChance;
        if (isCritical)
            coreDamage *= criticalMultiplier;

        AudioManager.instance.PlaySound(attackSound);

        if (defenderStats != null)
        {
            coreDamage -= defenderStats.GetArmor();
        }

        return new Attack((int) coreDamage, isCritical);
    }
}