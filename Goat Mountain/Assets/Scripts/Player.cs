﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private float blockSpeedFactor;

    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private int health;

    [SerializeField]
    private GameObject shield;

    private Vector2 direction;
    private Rigidbody2D rbody;
    private bool facingLeft;
    private int dashCooldown = 0;
    private bool isBlocking = false;
    private float blockSpeed;
    private float blockDashSpeed;
    private float currentSpeed;
    private float currentDashSpeed;


	// Use this for initialization
	void Start ()
    {
        direction = Vector2.zero;
        rbody = GetComponent<Rigidbody2D>();

        currentSpeed = speed;
        currentDashSpeed = dashSpeed;
        blockSpeed = speed * blockSpeedFactor;
        blockDashSpeed = dashSpeed * blockSpeedFactor;
	}


	
	// Update is called once per frame
	void Update ()
    {

        GetInput();
        // Move();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
           
        }
    }

    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }
        //rbody.velocity = direction.normalized * speed;
        rbody.AddForce(direction.normalized * currentSpeed);
        GetComponent<Animator>().SetBool("walking", direction.magnitude > 0.1);

        if(direction.x < 0)
        {
            facingLeft = true;
        }
        else if (direction.x > 0)
        {

            facingLeft = false;
        }
        GetComponent<SpriteRenderer>().flipX = facingLeft;

        if (Input.GetKey(KeyCode.J)) //dash
        {
            if(dashCooldown < 1)
            {
                rbody.AddForce(direction.normalized * currentDashSpeed);
                dashCooldown = 30;
                GetComponent<ParticleSystem>().Play();
            }
            
        }
        if(dashCooldown == 15)
        {
            GetComponent<ParticleSystem>().Stop();
        }
        dashCooldown--;

        if (Input.GetKey(KeyCode.K)) //attack
        {

        }

        bool blockKeyHeld = Input.GetKey(KeyCode.L);
        if (blockKeyHeld != isBlocking)
        {
            isBlocking = blockKeyHeld;
            shield.SetActive(isBlocking);
            if (isBlocking)
            {
                currentDashSpeed = blockDashSpeed;
                currentSpeed = blockSpeed;
            }
            else
            {
                currentSpeed = speed;
                currentDashSpeed = dashSpeed;
            }
        }
    }
}
