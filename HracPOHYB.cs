using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HracCONTROLLER))]
public class HracPOHYB : MonoBehaviour {

    public float _walkingSpeed = 10f;

    HracCONTROLLER controller;

    Camera viewCamera;

    public Transform gunHolder;
    public Transform Crosshair;
   
    
	// Use this for initialization
	void Start () {

        controller = GetComponent<HracCONTROLLER>();


        viewCamera = Camera.main;
        Cursor.visible = false;
       

	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * _walkingSpeed;
        controller.Move(moveVelocity);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero * gunHolder.position.y);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            Debug.DrawLine(ray.origin, point, Color.red);
            
            controller.LookAt(point);
            Crosshair.transform.position = point;

           
        }

        
    }

    
}
