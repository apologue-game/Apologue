using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            collision.GetComponent<PlayerControl>().ShowInteractionIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.name == "SlideCollider")
        {
            collision.GetComponent<PlayerControl>().HideInteractionIcon();
        }
    }
}
