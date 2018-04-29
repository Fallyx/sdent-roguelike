using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Vector2 direction;
    private Rigidbody2D rbody;

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
        rbody.velocity = new Vector2(0, 0);

        if(Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            rbody.velocity = new Vector2(0, speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            rbody.velocity = new Vector2(-speed, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
            rbody.velocity = new Vector2(0, -speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            rbody.velocity = new Vector2(speed, 0);
        }
    }
}
