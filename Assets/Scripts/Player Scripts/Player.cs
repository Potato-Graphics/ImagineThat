﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]



public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public float timeToWallUnstick;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    static int ID = 0;

    float accelerationTimeGrounded = .1f;
    float accelerationTimeAirborne = .2f;
    float moveSpeed = 7;
    float gravity = -20;
    float jumpVelocity;
    float velocityXSmoothing;

    public bool movingRight = false;
    public bool rotation = false;

    [SerializeField] private const int MAX_HEALTH = 3;
    [SerializeField] private int score = 0;
    public int currentHealth;
    ///*public */[SerializeField] bool isAttackable = true;
    public bool isAttackable = true;
    public float direction;
    [SerializeField] GameObject lifeOne = null;
    [SerializeField] GameObject lifeTwo = null;
    [SerializeField] GameObject lifeThree = null;
    [SerializeField] Vector3 spawnLocation = new Vector3(-11.13f, 9.34f, 0.011f);

    Vector3 velocity;

    float cooldown;

    Controller2D controller;

    public int myID;

    // Start is called before the first frame update
    void Start()
    {
        myID = ID++;
        lifeOne.SetActive(true);
        lifeTwo.SetActive(true);
        lifeThree.SetActive(true);
        controller = GetComponent<Controller2D>();
        isAttackable = true;

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    //Stops the player from moving building up downward force when standing still.
    void Update()
    {
        //Gets the inputs for moving left and right.
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);


        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if(velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }

        if (GetHealth() <= 0)
        {
            lifeOne.SetActive(false);
            lifeTwo.SetActive(false);
            lifeThree.SetActive(false);
            HandleDeath();
        }
        if (GetHealth() == 1)
        {
            lifeOne.SetActive(false);
            lifeTwo.SetActive(false);
            lifeThree.SetActive(true);
        }
        if (GetHealth() == 2)
        {
            lifeOne.SetActive(false);
            lifeTwo.SetActive(true);
            lifeThree.SetActive(true);
        }
        if (GetHealth() == 3)
        {
            lifeOne.SetActive(true);
            lifeTwo.SetActive(true);
            lifeThree.SetActive(true);
        }

        if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x ==0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below) 
            {
            velocity.y = jumpVelocity;
            }
        }

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
        SceneManager.LoadScene(1);
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
            if (isAttackable == true)
            {
                UpdateHealth(-amount);
                isAttackable = false;
                StartCoroutine(DamagedDelay());
            }
    }

    IEnumerator DamagedDelay()
    {
        yield return new WaitForSeconds(2);
        isAttackable = true;
    }
}
