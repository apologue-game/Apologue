using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
	private float length, startpos;
	public GameObject cam;
    public GameObject parallaxingParent;
	public float parallaxEffect;
    public float verticalPosition;
    float temp;
    float dist;
    public float parallaxLimit = 0f;

    private void Start()
    {
        startpos = transform.position.x;
        length = this.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        if (cam.transform.position.x < parallaxLimit)
        {
            parallaxingParent.transform.parent = transform;
            temp = (cam.transform.position.x * (1 - parallaxEffect));
            dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startpos + dist, verticalPosition, transform.position.z);

            if (temp > startpos + length)
            {
                startpos += length;
            }
            else if (temp < startpos - length)
            {
                startpos -= length;
            }
            return;
        }
        parallaxingParent.transform.parent = null;
    }
}