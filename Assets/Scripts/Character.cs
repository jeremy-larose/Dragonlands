using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character instance { get; private set; }

    public HealthSystem healthSystem;

    public string charName;

    public int currentHP { get; private set; }
    public int maxHP = 20;

    public delegate void HealthChanged();

    public static event HealthChanged OnHealthChanged;

    private void Awake()
    {
        instance = this;
        healthSystem = new HealthSystem(Dice.Roll(4, 8), 50);
        currentHP = healthSystem.GetHealth();
        TimeSystem.OnTick_5 += RegenHealth;
        charName = "Jerekai";
    }

    private void RegenHealth(object sender, TimeSystem.OnTickEventArgs e)
    {
        AddHealth(5);
    }

    private void AddHealth(int healing)
    {
        healthSystem.Heal( healing );
        OnHealthChanged?.Invoke();
        Debug.Log( $"{charName} heals for {healing}.");
        CombatText.Create(GetPosition() + new Vector3(0, 0f, -.1f), healing, false, Color.green);
    }

    public void TakeDamage(int damage)
    {
        damage = Mathf.Clamp(damage, 0, int.MaxValue); // Damage should never be negative.
        
        // Calculate damage modifiers
        bool isCriticalHit = Random.Range(0, 100) < 30;
        int damageTotal = Random.Range(1, damage);
        
        if (isCriticalHit)
            damageTotal *= 2;
        
        healthSystem.Damage( damageTotal );
        OnHealthChanged?.Invoke();
        Debug.Log( $"{charName} takes {damageTotal} points of damage." );

        CombatText.Create(GetPosition() + new Vector3( 0, 0f, -.1f ), damageTotal, isCriticalHit, 
            new Color32( 255, 128, 0, 255 ) );

        if (healthSystem.GetHealth() <= 0)
        {
            Die();
        }
    }
    
    public int GetHealth()
    {
        return healthSystem.GetHealth();
    }

    public int GetHealthMax()
    {
        return healthSystem.GetHealthMax();
    }

    public virtual void Die()
    {
        // Die in some way.
        // This method is meant to be overwritten.
        Debug.Log( $"[Character] {charName} has died.");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
