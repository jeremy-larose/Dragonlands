using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICharacterScreen : MonoBehaviour
{
    private Character _character;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public HealthBar healthBar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _character = GameManager.instance.player.GetComponent<Character>();
        nameText.text = _character.charName;
        healthText.text = $"HP: {_character.GetHealth()} / {_character.GetHealthMax()}";
        healthBar.SetSize( _character.healthSystem.GetHealthNormalized());
        Character.OnHealthChanged += CharacterOnHealthChanged;
    }

    private void CharacterOnHealthChanged()
    {
        healthText.text = $"HP: {_character.GetHealth()} / {_character.GetHealthMax()}";
        healthBar.SetSize( _character.healthSystem.GetHealthNormalized());
    }

    private void OnDestroy()
    {
        Character.OnHealthChanged -= CharacterOnHealthChanged;
    }
}
