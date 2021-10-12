using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleArcherAI : MonoBehaviour
{
    //Self references
    FemaleArcher femaleArcher;
    Animator animator;
    SpriteRenderer spriteRenderer;
    SpriteRenderer bodyRenderer;
    SpriteRenderer armsRenderer;

    //Targeting
    GameObject karasu;
    Transform karasuTransform;
    Transform currentTarget = null;
    float hDistance;
    bool facingLeft = true;

    //Ignore collision with player
    public BoxCollider2D boxCollider2D;
    BoxCollider2D boxCollider2DKarasu;
    CircleCollider2D karasuParryCollider;
    CircleCollider2D karasuBlockCollider;

    //Spawn location
    int myID;
    string myName = "";
    Vector3 spawnLocation;
    public GameObject spawn;

    //Attacking
    public float attackCooldown = 2.5f;
    float nextAttack = 0f;
    bool currentlyAttacking = false;

    private void Awake()
    {
        //Targeting references
        karasu = GameObject.FindGameObjectWithTag("Player");
        karasuTransform = karasu.transform;

        //Self references and initializations
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        femaleArcher = GetComponent<FemaleArcher>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bodyRenderer = GameObject.Find("archerBodyNoArms").GetComponent<SpriteRenderer>();
        armsRenderer = GameObject.Find("archerArms").GetComponent<SpriteRenderer>();

        //Ignore collider collisions
        boxCollider2DKarasu = karasu.GetComponent<BoxCollider2D>();
        karasuParryCollider = karasu.transform.Find("ParryCollider").GetComponent<CircleCollider2D>();
        karasuBlockCollider = karasu.transform.Find("BlockCollider").GetComponent<CircleCollider2D>();

        //Spawn location references
        myID = GameMaster.enemyID++;
        myName = "Archer" + myID + "_SpawnLocation";
        spawnLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawn = new GameObject(myName);
        spawn.transform.position = spawnLocation;
        currentTarget = spawn.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(boxCollider2D, boxCollider2DKarasu);
        InvokeRepeating("InCombat", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hDistance = Mathf.Abs(transform.position.x - karasuTransform.position.x);
        if (transform.position.x > karasuTransform.position.x && !facingLeft && !currentlyAttacking)
        {
            Flip();
        }
        else if (transform.position.x < karasuTransform.position.x && facingLeft && !currentlyAttacking)
        {
            Flip();
        }
        //Karasu parry and block colliders need to be ignored repeatedly because they're getting disabled and enabled multiple times
        if (currentTarget != null)
        {
            if (Time.time > nextAttack)
            {
                Attack();
            }

            Physics2D.IgnoreCollision(boxCollider2D, karasuParryCollider);
            Physics2D.IgnoreCollision(boxCollider2D, karasuBlockCollider);
        }
    }

    //Combat system
    void Attack()
    {
        nextAttack += Time.time + attackCooldown;
        currentlyAttacking = true;
        Debug.Log("Sprite renderer disabled");
        animator.SetTrigger("animFemaleArcherAttack");
        StartCoroutine(BowWindUp());
    }

    IEnumerator BowWindUp()
    {
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = false;
        bodyRenderer.enabled = true;
        armsRenderer.enabled = true;
    }

    //Aiming method

    //Shoot method

    //Utilities
    void InCombat()
    {
        if (hDistance < 15 && currentTarget != karasuTransform)
        {
            currentTarget = karasuTransform;
        }
        else if (hDistance > 15 && currentTarget != null)
        {
            currentTarget = null;
            //Heal archer if target gets out of range
            femaleArcher.currentHealth = femaleArcher.maxHealth;
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
