public class Key : WorldItem
{
    public override void OnInteract()
    {
        GameManager.Instance.HasKey = true;
        Destroy(this);
    }
}