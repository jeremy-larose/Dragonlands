using TMPro;
using UnityEngine;

public class UICharacterScreen : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public HealthBar healthBar;
    private Character _character;


    // Start is called before the first frame update
    private void Start()
    {
        _character = GameManager.instance.player.GetComponent<Character>();
        nameText.text = _character.charName;
        healthText.text = $"HP:{_character.GetHealth()}/{_character.GetHealthMax()}";
        healthBar.SetSize(_character.HealthSystem.GetHealthNormalized());
        Character.OnHealthChanged += CharacterOnHealthChanged;
    }

    private void OnDestroy()
    {
        Character.OnHealthChanged -= CharacterOnHealthChanged;
    }

    private void CharacterOnHealthChanged()
    {
        healthText.text = $"HP: {_character.GetHealth()} / {_character.GetHealthMax()}";
        healthBar.SetSize(_character.HealthSystem.GetHealthNormalized());
    }
}