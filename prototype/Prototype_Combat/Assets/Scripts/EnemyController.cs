﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {

    public float radiusDetection = 15.0f;

    public GameObject lockedTarget;

    public GameObject activeWeapon;

    private Animator animator;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.isAlive())
            return;
        this.chaseAndAttack("Player");
	}

    void chaseAndAttack(string tag)
    {
        this.lockedTarget = this.getTargetToChase(tag);

        if (this.lockedTarget != null)
        {
            this.chase(this.lockedTarget);
            activeWeapon.GetComponent<WeaponController>().attack(this, this.lockedTarget);
        } else if (animator != null)
        {
            animator.SetBool("Chase", false);
        }
    }

    void chase(GameObject target)
    {
        if (animator != null)
        {
            animator.SetBool("Chase", true);
        }
        this.transform.LookAt(target.transform);
    }

    GameObject getTargetToChase(string tag)
    {
        List<GameObject> players = this.detectTargetsInRadius(tag, this.radiusDetection);
        GameObject toChase = null;

        foreach (GameObject player in players)
        {
            toChase = player;
        }
        return toChase;
    }

    List<GameObject> detectTargetsInRadius(string tag, float radius)
    {
        List<GameObject> detected = new List<GameObject>();
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject target in targets)
        {
            if (this.isTargetInRadius(target, radius))
            {
                detected.Add(target);
            }
        }
        return detected;
    }
}
