﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Manager : MonoBehaviour
{
    private float currentHealth;
    [SerializeField] private float MAX_HEALTH = 0;
    [SerializeField] private bool killable = false;
    [SerializeField] public Image healthBar;
    Enemy enemy;
    public GameObject coin;
    public GameObject life;
    float fillAmount = 1.0f;
    public bool dropLoot = true;

    public int pointsGiven;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        UpdateHealth(MAX_HEALTH);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        fillAmount = currentHealth / MAX_HEALTH;
        if(healthBar != null) healthBar.fillAmount = fillAmount;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            print("bullet collision" );
            if (enemy.GetEnemyType() == Enemy.EnemyType.HelicopterSeed)
            {
                transform.position = new Vector3(1000, 1000, 1000);
                StartCoroutine(RespawnHelicopter());
            }
            if (killable)
            {
                UpdateHealth(-Player.bulletDamage);
                Player.bulletDamage = 1;
                if (GetHealth() <= 0)
                {
                    ScoreManager.scoreValue += pointsGiven;
                    enemy.UpdateHealth(enemy.MAX_HEALTH);
                    enemy.SetState(Enemy.State.Dead);
                    if(dropLoot)
                    {
                    if (enemy.GetEnemyType() != Enemy.EnemyType.HelicopterSeed)
                    {
                        if (Random.Range(0, 8) == 0)
                        {
                            Instantiate(life, transform.position, transform.rotation);
                        } else
                        {
                            Instantiate(coin, transform.position, transform.rotation);
                        }
                      }
                    }
                }
            }
        }
    }

    IEnumerator RespawnHelicopter()
    {
        yield return new WaitForSeconds(1);
        transform.position = enemy.startPosition;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            if (enemy.GetEnemyType() == Enemy.EnemyType.HelicopterSeed)
            {
                transform.position = enemy.startPosition;
            }
            if (killable)
            {
                UpdateHealth(-Player.bulletDamage);
                Player.bulletDamage = 1;
                if (GetHealth() <= 0)
                {
                    enemy.UpdateHealth(enemy.MAX_HEALTH);
                    enemy.SetState(Enemy.State.Dead);
                }
            }
        }
    }
}
