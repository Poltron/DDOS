using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour 
{
    [Range(1f, 1000f)]
    public float rotationSpped = 0f;
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(-Vector3.forward, rotationSpped * Time.deltaTime);
	}
}
