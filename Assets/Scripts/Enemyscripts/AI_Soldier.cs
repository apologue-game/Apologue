using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AI_Soldier : MonoBehaviour
{
    Seeker seeker;
    private Rigidbody2D rigidBody2D;
    public Transform target;

    Path path;
    int currentWaypoint = 0;
    bool endOfPath = false;

    float movementSpeed = 5f;
    float nextWaypointDistance = 1.3f;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            endOfPath = true;
            return;
        }
        else
        {
            endOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody2D.position).normalized;
        Vector2 force = direction * movementSpeed * Time.deltaTime;

        rigidBody2D.AddForce(force);

        float distance = Vector2.Distance(rigidBody2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    //Utilities
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigidBody2D.position, target.position, OnPathComplete);
        }
    }

    void Flip()
    {

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
