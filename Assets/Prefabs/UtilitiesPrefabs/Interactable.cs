using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    public bool active = true;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (active)
            {
                collision.GetComponent<PlayerControl>().ShowInteractionIcon();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!active)
            {
                collision.GetComponent<PlayerControl>().HideInteractionIcon();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>().HideInteractionIcon();
        }
    }
}
