using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HracKAMERA : MonoBehaviour {

    Transform target;            
    public float smoothing = 5f;       

    Vector3 offset;

  

    void Start()
    {

        if (GameObject.FindGameObjectWithTag("HRAC") != null)
        {
            target = GameObject.FindGameObjectWithTag("HRAC").transform;
            offset = transform.position - target.position;
        }

       
    }

    void FixedUpdate()
    {

        if (GameObject.FindGameObjectWithTag("HRAC") != null)
        {
            Vector3 targetCamPos = target.position + offset;


            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
        
    }
}