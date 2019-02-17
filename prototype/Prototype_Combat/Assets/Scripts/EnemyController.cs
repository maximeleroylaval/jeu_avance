using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {

    public float radiusDetection = 15.0f;
    public float radiusAttack = 5.0f;

    public GameObject lockedTarget;

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        this.chaseAndAttack("Player");
	}

    void chaseAndAttack(string tag)
    {
        this.lockedTarget = this.getTargetToChase(tag);

        if (this.lockedTarget != null)
        {
            this.chase(this.lockedTarget);
            if (this.canAttack(this.lockedTarget))
            {
                this.attack(this.lockedTarget);
            } else if (animator != null)
            {
                animator.SetBool("Attack", false);
            }
        } else if (animator != null)
        {
            animator.SetBool("Chase", false);
        }
    }

    bool canAttack(GameObject target)
    {
        if (this.isTargetInRadius(target, this.radiusAttack)) // && l'animation d'attaque est finie
        {
            return true;
        }
        return false;
    }

    void chase(GameObject target)
    {
        if (animator != null)
        {
            animator.SetBool("Chase", true);
        }
        // AI
    }

    void attack(GameObject target)
    {
        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }
        target.GetComponent<EntityController>().takeDamage(this.getDamage());
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

    bool isTargetInRadius(GameObject target, float radius)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < radius)
        {
            return true;
        }
        return false;
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
