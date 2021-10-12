using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsAndBowRotation : MonoBehaviour
{
    Transform karasuTransform;

    public int rotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        karasuTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(karasuTransform.position) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
    }
}

