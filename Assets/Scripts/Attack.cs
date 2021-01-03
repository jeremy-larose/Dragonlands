public class Attack
{
    public Attack(int damage, bool critical)
    {
        Damage = damage;
        IsCritical = critical;
    }

    public int Damage { get; }

    public bool IsCritical { get; }
}