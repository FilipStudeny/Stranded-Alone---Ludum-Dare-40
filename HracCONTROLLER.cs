using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HracCONTROLLER : MonoBehaviour {

    Vector3 velocity;
    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 lookCorectionPoint = new Vector3(lookPoint.x,transform.position.y, lookPoint.z);
        transform.LookAt(lookCorectionPoint);

    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position += velocity * Time.fixedDeltaTime);
    }
}
