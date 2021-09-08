using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NepritelPOHYB : NepritelZIVOT {

    public enum State { Idle, Chasing, Attacking };
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;

    HracSTATY targetEntity;

    CapsuleCollider capsuleCollider;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    public int damage = 2;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    Animator animator;

    bool canAttack;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        canAttack = true;

        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        if (GameObject.FindGameObjectWithTag("HRAC") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("HRAC").transform;
            targetEntity = target.GetComponent<HracSTATY>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }

       
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update()
    {
        if (targetEntity.fuelSlider.value == 10)
        {
            damage = 5;
            health = 10;
        }
        else if (targetEntity.fuelSlider.value == 20)
        {
            damage = 10;
            health = 30;
        }
        else if (targetEntity.fuelSlider.value == 30)
        {
            damage = 20;
            health = 40;
        }
        else if (targetEntity.fuelSlider.value == 40)
        {
            damage = 30;
            health = 80;
        }
        else if (targetEntity.fuelSlider.value == 50)
        {
            damage = 40;
            health = 100;
        }

        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }

            }
        }

        if (dead)
        {
            canAttack = false;
            currentState = State.Idle;
            animator.SetBool("Dying", true);
            animator.SetBool("Attacking", false);
            animator.SetBool("Mooving", false);

            pathfinder.enabled = false;
            capsuleCollider.isTrigger = true;
        }

    }

    IEnumerator Attack()
    {
        if (canAttack)
        {
            currentState = State.Attacking;
            pathfinder.enabled = false;

            animator.SetBool("Attacking", true);
            animator.SetBool("Mooving", false);

            Vector3 originalPosition = transform.position;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

            float attackSpeed = 3;
            float percent = 0;


            bool hasAppliedDamage = false;

            while (percent <= 1)
            {

                if (percent >= .5f && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    targetEntity.TakeDamage(damage);
                }

                percent += Time.deltaTime * attackSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

                yield return null;
            }

            animator.SetBool("Attacking", false);
            animator.SetBool("Mooving", true);

            currentState = State.Chasing;
            pathfinder.enabled = true;
        }
        
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                    animator.SetBool("Mooving", true);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}

