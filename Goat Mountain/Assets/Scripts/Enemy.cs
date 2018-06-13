using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int dmg;

    [SerializeField]
    private Image hpBar;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int speed;

    private int health;
    
    private int hitCooldown;

    private bool facingRight;

    // Use this for initialization
    void Start () {
        health = maxHealth;
        hitCooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        var player = GameObject.Find("Character");
        var playerPos = player.transform.position;
        var toPlayer = playerPos - transform.position;
        if (toPlayer.magnitude < 7)
        {
            //GetComponent<Rigidbody2D>().
            GetComponent<Rigidbody2D>().AddForce(toPlayer.normalized * speed);
        }
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

        hitCooldown--;
	}

    public void ApplyDamage(int damage)
    {
        if (hitCooldown > 0)
        {
            return;
        }
        health -= damage;
        health = System.Math.Max(health, 0);
        hitCooldown = 10;

        float hpFill = (float)health / (float)maxHealth;

        hpBar.GetComponent<HPBarBehaviour>().UpdateHPBar(hpFill);
        if(health == 0)
        {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }

    public int GetEnemyDmg()
    {
        return dmg;
    }
}
