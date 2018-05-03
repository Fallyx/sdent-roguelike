using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;


    [SerializeField]
    private float dashSpeed;

    private Vector2 direction;
    private Rigidbody2D rbody;
    private bool facingLeft;
    private int dashCooldown = 0;

	// Use this for initialization
	void Start ()
    {
        direction = Vector2.zero;
        rbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        // Move();

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
        rbody.AddForce(direction.normalized * speed);
        if(direction.x < 0)
        {
            facingLeft = true;
        }
        else if (direction.x > 0)
        {

            facingLeft = false;
        }
        GetComponent<SpriteRenderer>().flipX = facingLeft;

        if (Input.GetKey(KeyCode.J))
        {
            if(dashCooldown < 1)
            {
                rbody.AddForce(direction.normalized * dashSpeed);
                dashCooldown = 30;
                GetComponent<ParticleSystem>().Play();
            }
            
        }
        if(dashCooldown == 15)
        {
            GetComponent<ParticleSystem>().Stop();
        }
        dashCooldown--;
    }
}
