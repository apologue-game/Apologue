using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailAI : MonoBehaviour, IEnemy
{
    public Animator animator;
    public Rigidbody2D rigidBody2D;
    public float speed = 0f;
    public int movingTowardsIndex = 1;
    public float direction = 0;
    public Transform[] positions;
    public bool facingLeft = true;

    public float distance;
    #region IEnemy clutter
    Animator IEnemy.animator { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int maxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float currentHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isPartOfCluster { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isReadyToAttack { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isTakingDamage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool inCombat { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public IEnemy.EnemyType enemyType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    #endregion
    public bool isDead { get; set; }
    public bool isStaggered { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Awake()
    {
        isDead = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<KarasuEntity>().TakeDamage(2, null);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.Play("death");
            return;
        }
        distance = transform.position.x - positions[movingTowardsIndex].position.x;

        if (Mathf.Abs(distance) < 0.1f)
        {
            movingTowardsIndex++;
            if (movingTowardsIndex == positions.Length)
            {
                movingTowardsIndex = 0;
            }
            Flip();
        }

        if (transform.position.x > positions[movingTowardsIndex].position.x)
        {
            direction = -1;
            
        }
        if (transform.position.x < positions[movingTowardsIndex].position.x)
        {
            direction = 1;
        }
        rigidBody2D.velocity = new Vector2(direction * speed, rigidBody2D.velocity.y);
    }

    public void TakeDamage(float damage, bool? specialInteraction)
    {
        StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        isDead = true;
        yield return new WaitForSeconds(3f);
        GameMaster.DestroyGameObject(gameObject);
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
