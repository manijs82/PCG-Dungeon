namespace Interaction
{
    public class Portal : Interactable
    {
        public override void OnInteract()
        {
            GameManager.Instance.GoToNewDungeon();
        }
    }
}