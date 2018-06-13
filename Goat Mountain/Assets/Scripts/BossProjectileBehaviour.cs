using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileBehaviour : MonoBehaviour
{
    private bool facingRight = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var direction = GetComponent<Rigidbody2D>().velocity;

        if (direction.x < 0.1)
        {
            facingRight = false;
        }
        else if (direction.x > 0.1)
        {

            facingRight = true;
        }
        GetComponent<SpriteRenderer>().flipX = facingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Player>().ApplyDamage(2);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction.normalized * 5;
    }
}
