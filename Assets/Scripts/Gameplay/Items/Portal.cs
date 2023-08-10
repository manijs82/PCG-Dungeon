public class Portal : WorldItem
{
    public override void OnInteract()
    {
        GameManager.Instance.GoToNewDungeon();
    }
}