public class NPCBase : Character
{
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}