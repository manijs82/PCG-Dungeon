namespace Interaction
{
    public class Key : Interactable
    {
        public override void OnInteract()
        {
            GameManager.Instance.HasKey = true;
            Destroy(gameObject);
        }
    }
}