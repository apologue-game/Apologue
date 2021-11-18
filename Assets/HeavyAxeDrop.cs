using UnityEngine;

public class HeavyAxeDrop : MonoBehaviour
{
    public Transform groundCheck;
    public float groundCheckRange = 0.3f;
    public LayerMask whatIsGround;
    Rigidbody2D rigidBody2D;
    BoxCollider2D boxCollider2D;
    public GameObject tooltipIconGO;
    public SpriteRenderer tooltipIcon;
    public Sprite keyboardIcon;
    public Sprite gamepadIcon;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRange, whatIsGround);
        for (int i = 0; i < collidersGround.Length; i++)
        {
            if (collidersGround[i].name == "PlatformsTilemap" || collidersGround[i].name == "GroundTilemap" || collidersGround[i].name == "NothingTilemap" || collidersGround[i].CompareTag("Box") || collidersGround[i].CompareTag("FallingPlatforms"))
            {
                rigidBody2D.isKinematic = true;
                boxCollider2D.isTrigger = true;
                boxCollider2D.size = new Vector2(5, 5);
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRange);
    }
}
