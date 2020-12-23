using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{

    private int _healthMax;

    private int _health;

    public HealthSystem(int currentHealth, int healthMax)
    {
        _health = currentHealth;
        _healthMax = healthMax;
    }

    public void SetHealth(int health)
    {
        _health = health;
    }

    public int GetHealth()
    {
        return _health;
    }

    public int GetHealthMax()
    {
        return _healthMax;
    }

    public float GetHealthNormalized()
    {
        return (float) _health / _healthMax;
    }

    public void Damage(int amount)
    {
        _health -= amount;
        if (_health < 0)
        {
            _health = 0;
        }

        if (_health <= 0)
        {
            Debug.Log( this + " has died" );
        }
    }

    public void Heal(int amount)
    {
        _health += amount;
        if (_health > _healthMax)
        {
            _health = _healthMax;
        }
    }

    public void HealComplete()
    {
        _health = _healthMax;
    }

    public void SetHealthMax(int healthMax, bool fullHealth)
    {
        _healthMax = healthMax;
        if (fullHealth)
            _health = _healthMax;
    }

    public bool IsDead()
    {
        return _health <= 0;
    }
    
}
