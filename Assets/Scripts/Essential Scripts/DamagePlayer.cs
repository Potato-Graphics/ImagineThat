﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] Player player;
    Enemy enemy;
    [SerializeField] bool destroyable;
    [SerializeField] int damageAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            if (destroyable)
            {
                enemy.UpdateHealth(enemy.MAX_HEALTH);
                enemy.SetState(Enemy.State.Dead);

            }
            player.DealDamage(damageAmount);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            if (destroyable)
            {
                enemy.UpdateHealth(enemy.MAX_HEALTH);
                enemy.SetState(Enemy.State.Dead);

            }
            player.DealDamage(damageAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //  print("test" + col.gameObject.tag);
        if (col.gameObject.tag.Equals("Player"))
        {
            if (destroyable)
            {
                enemy.UpdateHealth(-enemy.startHealth);
                enemy.SetState(Enemy.State.Dead);

            }
           player.DealDamage(damageAmount);
        }
    }
}
