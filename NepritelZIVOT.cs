using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NepritelZIVOT : MonoBehaviour {

    public float startingHealth;
    protected float health;
    protected bool dead;

    public GameObject platidlo;

    AudioSource die;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
        die = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        die.Play();

        Instantiate(platidlo, transform.position, transform.rotation);

        GameObject.Destroy(gameObject, 2f);
    }
}
