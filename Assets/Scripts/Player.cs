﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]


public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 8;
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;
    public bool movingRight = false;
    public bool rotation = false;
    [SerializeField] private const int MAX_HEALTH = 3;
    [SerializeField] private int score = 0;
    public int currentHealth;
    public bool isAttackable = true;
    public float direction;
    [SerializeField] GameObject lifeOne;
    [SerializeField] GameObject lifeTwo;
    [SerializeField] GameObject lifeThree;
    [SerializeField] Vector3 spawnLocation = new Vector3(-11.13f, 9.34f, 0.011f);

    Vector3 velocity;

    Controller2D controller;

    // Start is called before the first frame update
    void Start()
    {
        lifeOne.SetActive(true);
        lifeTwo.SetActive(true);
        lifeThree.SetActive(true);
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    //Stops the player from moving building up downward force when standing still.
    void Update()
    {
        switch(GetHealth())
        {
            case 0:
                lifeOne.SetActive(false);
                lifeTwo.SetActive(false);
                lifeThree.SetActive(false);
                HandleDeath();
                break;
            case 1:
                lifeOne.SetActive(false);
                lifeTwo.SetActive(false);
                lifeThree.SetActive(true);
                break;
            case 2:
                lifeOne.SetActive(false);
                lifeTwo.SetActive(true);
                lifeThree.SetActive(true);
                break;
            case 3:
                lifeOne.SetActive(true);
                lifeTwo.SetActive(true);
                lifeThree.SetActive(true);
                break;
            default:
                lifeOne.SetActive(false);
                lifeTwo.SetActive(false);
                lifeThree.SetActive(false);
                print("UNEXPECTED_HEALTH_ERROR");
                break;
        }
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

        //Gets the inputs for moving left and right.
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(input.x < 0)
        {
            direction = -1;
            movingRight = false;
        }
        else if(input.x > 0)
        {
            direction = 1;
            movingRight = true;
        }
        else { direction = 0; }

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeGrounded);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Gets key input for shooting upwards.
        if (Input.GetKeyDown(KeyCode.W))
        {
            rotation = true;
        }

        if(Input.GetKeyUp(KeyCode.W))
        {
            rotation = false;
        }

    }

    void HandleDeath()
    {
        SceneManager.LoadScene("RunAndGun");
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;

    }

    public int GetScore()
    {
        return score;
    }

    public void UpdateScore(int amount)
    {
        score += amount;
    }

    public void DealDamage(int amount)
    {
        if (!isAttackable)
            return;
        UpdateHealth(-amount);
        print("Player Health: " + GetHealth());
        isAttackable = false;
    }

    IEnumerator DamagedDelay()
    {
        yield return new WaitForSeconds(2);
        isAttackable = true;
    }
}
