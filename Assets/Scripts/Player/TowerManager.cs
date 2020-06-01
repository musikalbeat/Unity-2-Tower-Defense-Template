﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    //public TowerType self;
    public Animator anim;
    [SerializeField]
    private int currentHealth = 100;
    private SpriteRenderer spr;
    private int healthyLevelHealth = 60;
    public GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        //currentHealth = self.health;
        FindNextTarget();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
        if (currentTarget != null)
        {
            AttackTarget();
        }
        else
        {
            FindNextTarget();
        }
        if (currentHealth < 0)
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateSprite()
    {
        // Changes Sprite color
        if (currentHealth >= healthyLevelHealth)
        {
            //spr.color = self.healthyColor;
        }
        else
        {
            //spr.color = self.damagedColor;
        }
    }

    void FindNextTarget()
    {
        // Try to find closest enemy

        if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
        {
            foreach (GameObject target in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (currentTarget == null)
                {
                    currentTarget = target;
                }
                else if (
                    Vector3.Distance(this.transform.position,
                    currentTarget.transform.position) > Vector3.Distance(this.transform.position,
                    target.transform.position))
                {
                    currentTarget = target;
                }
            }
        }

        if (currentTarget == null)
        {
            currentTarget = GameObject.FindGameObjectWithTag("Enemy");
        }
        if(anim != null)
        {
            if (currentTarget != null)
            {
                anim.SetBool("hasTarget", true);
            }
            else
            {
                anim.SetBool("hasTarget", false);
            }
        }
    }

    public Quaternion GetDir()
    {
        Vector2 direction = currentTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        return rot;
    }

    void AttackTarget()
    {
        Debug.DrawLine(this.transform.position, currentTarget.transform.position);
        // Fire projectile
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyManager>() != null)
        {
            currentHealth -= collision.gameObject.GetComponent<EnemyManager>().self.damage;
            Destroy(collision.gameObject);
        }
    }
}
