public class NPCBase : Character
{
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
        AudioManager.instance.PlayMusic(GameAssets.i.overworldMusic);
    }
}