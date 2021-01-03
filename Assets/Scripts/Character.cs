using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void HealthChanged();

    public string charName;
    public int maxHP = 20;
    [SerializeField] private int attackDamage;
    [SerializeField] private int armorClass;
    public GameObject weapon;

    public HealthSystem HealthSystem;
    public Inventory Inventory;


    public static Character Instance { get; private set; }

    public int CurrentHP { get; private set; }

    private void Awake()
    {
        Instance = this;
        HealthSystem = new HealthSystem(Dice.Roll(4, 8), 50);
        CurrentHP = HealthSystem.GetHealth();
        Inventory = new Inventory(UseItem);
        TimeSystem.OnTick_5 += RegenHealth;
    }

    private void Update()
    {
    }

    public int GetDamage()
    {
        return attackDamage;
    }

    public int GetArmor()
    {
        return armorClass;
    }

    public static event HealthChanged OnHealthChanged;

    private void RegenHealth(object sender, TimeSystem.OnTickEventArgs e)
    {
        if (this.CompareTag("Player"))
            AddHealth(5);
    }

    private void AddHealth(int healing)
    {
        HealthSystem.Heal(healing);
        OnHealthChanged?.Invoke();
        Debug.Log($"{charName} heals for {healing}.");
        CombatText.Create(GetPosition() + new Vector3(0, 0f, -.1f), healing, false, Color.green);
    }

    public void TakeDamage(int damage)
    {
        damage = Mathf.Clamp(damage, 0, int.MaxValue); // Damage should never be negative.

        // Calculate damage modifiers
        var isCriticalHit = Random.Range(0, 100) < 30;
        var damageTotal = Random.Range(1, damage);

        if (isCriticalHit)
            damageTotal *= 2;

        HealthSystem.Damage(damageTotal);
        OnHealthChanged?.Invoke();
        Debug.Log($"{charName} takes {damageTotal} points of damage.");

        CombatText.Create(GetPosition() + new Vector3(0, 0f, -.1f), damageTotal, isCriticalHit,
            new Color32(255, 128, 0, 255));

        if (HealthSystem.GetHealth() <= 0) Die();
    }

    public int GetHealth()
    {
        return HealthSystem.GetHealth();
    }

    public int GetHealthMax()
    {
        return HealthSystem.GetHealthMax();
    }

    public virtual void Die()
    {
        // Die in some way.
        // This method is meant to be overwritten.
        TimeSystem.OnTick_5 -= RegenHealth;

        Debug.Log($"[Character] {charName} has died.");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                Debug.Log("[Player] Using Health Potion!");
                AddHealth(Dice.Roll(1, 12));
                Inventory.RemoveItem(new Item {itemType = Item.ItemType.HealthPotion, amount = 1});
                break;
            case Item.ItemType.Coin:
                Debug.Log("[Player] Using a coin!");
                Inventory.RemoveItem(new Item {itemType = Item.ItemType.Coin, amount = 1});
                break;
            default:
                Debug.Log("[Player] Using an item, but no action defined.");
                break;
        }
    }
}