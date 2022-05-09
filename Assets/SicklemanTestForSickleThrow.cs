using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SicklemanTestForSickleThrow : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public ParticleSystem portal;
    public ParticleSystem portal2;
    public Animator animator;
    public float cooldown = 3f;
    public bool ready = true;
    public bool doingSomething = false;
    public SpriteRenderer spriteRenderer;

    public Rigidbody2D weaponRB;
    public SpriteRenderer weaponSR;
    public Transform weaponTransform;
    public bool weaponThrown = false;
    public float weaponThrowSpeed = 10f;
    Vector3 rotation;
    Vector3 initialPosition = new Vector3(-1.27999997f, 0.236000001f, 0f);

    //Animations
    const string IDLEANIMATION = "idle";
    const string DEATHANIMATION = "death";
    const string WALKANIMATION = "walk";
    const string BASICANIMATION = "basic";
    const string STOMPPREPARATIONANIMATION = "stompPreparation";
    const string STOMPFALLINGANIMATION = "stompFalling";
    const string STOMPLANDINGANIMATION = "stompLanding";
    const string SICKLETHROWANIMATION = "sickleThrow";
    const string SICKLETHROWCATCHANIMATION = "sickleThrowCatch";
    const string SCREAMANIMATION = "screamCatch";
    string oldState = "";

    void Start()
    {
        rotation = new Vector3(0, 0, 1);
    }

    private void FixedUpdate()
    {
        if (Time.time > cooldown && ready)
        {
            doingSomething = true;
            ready = false;
            AnimatorSwitchState(SICKLETHROWANIMATION);
            
        }
        else if (!doingSomething)
        {
            AnimatorSwitchState(IDLEANIMATION);
        }
    }

    void CreatePortal()
    {
        portal.Play();
        weaponThrown = true;
    }

    void CatchTheSickle()
    {
        spriteRenderer.enabled = false;
        StartCoroutine(PortalDelay());
    }

    Vector3 newPosition;
    IEnumerator PortalDelay()
    {
        newPosition = weaponTransform.position;
        newPosition.x = newPosition.x + 2;
        yield return new WaitForSeconds(1.5f);
        portal2.Play();
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        portal.startSpeed = -10f;
        spriteRenderer.enabled = true;
        AnimatorSwitchState(SICKLETHROWCATCHANIMATION);
    }

    void WeaponCaught()
    {
        portal2.startSpeed = -10f;
        weaponThrown = false;
        weaponSR.enabled = false;
        weaponTransform.position = initialPosition;
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        doingSomething = false;
    }

    void Ready()
    {
        ready = true;
        cooldown = Time.time + 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponThrown)
        {
            weaponSR.enabled = true;
            weaponRB.velocity = new Vector2(-1 * weaponThrowSpeed, weaponRB.velocity.y);
            weaponTransform.Rotate(rotation);
        }
    }

    public void AnimatorSwitchState(string newState)
    {
        if (oldState == newState)
        {
            return;
        }

        animator.Play(newState);

        oldState = newState;
    }
}
