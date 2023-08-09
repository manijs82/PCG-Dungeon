using UnityEngine;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        private void Interact()
        {
            var center = new Vector2(transform.position.x, transform.position.y);
            var size = new Vector2(3, 3);
            var colliders = Physics2D.OverlapBoxAll(center, size, 0);
            foreach (var col in colliders)
            {
                var interactable = col.GetComponent<Interactable>();
                if (interactable == null) continue;
                interactable.OnInteract();
                return;
            }
        }
    }
}