﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    // Start is called before the first frame update
    float moveSpeed = 10f;

    Rigidbody2D rb;

    public GameObject implosionPrefab;
    public GameObject sparksPrefab;
    [SerializeField] Player player;

    Player target;
    Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<Player>(); //Finds the player.
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed; // Moves Towards the player.
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y); // The speed in which the crystal will move.
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D col)
    {
        //print("on collision " + player.isAttackable);
        print("test" + col.gameObject.tag);
        // Spawns the implosion effect when the crystal hits an object.
        if (col.gameObject.tag.Contains("Obstacles"))
        {
            Instantiate(implosionPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(col.gameObject.tag == "Player")
        {
            Instantiate(implosionPrefab, gameObject.transform.position, Quaternion.identity);
            Debug.LogError("crystal damage player");
            player.DealDamage(1);
            print("hello");
            Destroy(gameObject);
        }
        // Spawns a seperate effect for when the player shoots at the crystal.
        else if (col.gameObject.tag == "Bullet")
        {
            Instantiate(sparksPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
