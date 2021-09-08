using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HracLASER : MonoBehaviour {

    public int damage = 20;
    public float timeBetweanShots = 100f;
    public float LaserRange = 20f;

    float timer;
    Ray shootRay;
    RaycastHit hit;

    public Transform LaserMuzzle;
    public LineRenderer LaserLine;

    public float fireRate = 0.25f;

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
  
    private float nextFire;

    AudioSource gunShoot;

    void Start()
    {
        LaserLine = GetComponentInChildren<LineRenderer>();
        gunShoot = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            LASER();
        }

    }

    void LASER()
    {
        nextFire = Time.time + fireRate;

        StartCoroutine(LaserEffect());

        RaycastHit hit;

        LaserLine.SetPosition(0, LaserMuzzle.position);

        if (Physics.Raycast(LaserMuzzle.position, LaserMuzzle.forward, out hit,LaserRange))
        {
            MineralyZIVOT mineralsHealth = hit.collider.GetComponent<MineralyZIVOT>();
            NepritelZIVOT enemyHealth = hit.collider.GetComponent<NepritelZIVOT>();

            if(mineralsHealth != null)
            {
                mineralsHealth.TakeDamage(damage,hit);
            }

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, hit);
            }

            LaserLine.SetPosition(1, hit.point);

        }
        else
        {
           LaserLine.SetPosition(1, LaserMuzzle.position + (LaserMuzzle.forward * LaserRange));
        }
    }

    private IEnumerator LaserEffect()
    {
        gunShoot.Play();
       LaserLine.enabled = true;

        yield return shotDuration;

       LaserLine.enabled = false;
    }
}

