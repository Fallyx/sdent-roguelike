using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private float blockSpeedFactor;

    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private GameObject shield;

    [SerializeField]
    private bool hasShield;

    [SerializeField]
    private bool hasUpgradedSword;

    [SerializeField]
    private bool hasDash;

    [SerializeField]
    private GameObject DialogPanel;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Image healthbar;

    [SerializeField]
    private GameObject gameOverCanvas;
    
    private AudioSource hurtSound;
    private AudioSource deathtSound;
    private AudioSource dashSound;

    private int hp = 100;
    private Vector2 direction;
    private Rigidbody2D rbody;
    private bool facingLeft;
    private int dashCooldown = 0;
    private int attackCooldown = 0;
    private bool isBlocking = false;
    private float blockSpeed;
    private float blockDashSpeed;
    private float currentSpeed;
    private float currentDashSpeed;
    private bool dialogShowed;
    private float dialogShowTime;
    private bool recentlyHit = false;
    private float recentlyHitTime;
    private bool isDead = false;
    private float healCooldown = 0;

	// Use this for initialization
	void Start ()
    {
        direction = Vector2.zero;
        rbody = GetComponent<Rigidbody2D>();

        currentSpeed = speed;
        currentDashSpeed = dashSpeed;
        blockSpeed = speed * blockSpeedFactor;
        blockDashSpeed = dashSpeed * blockSpeedFactor;

        var sounds = GetComponents<AudioSource>();
        hurtSound = sounds[0];
        deathtSound = sounds[1];
        dashSound = sounds[2];
	}


	
	// Update is called once per frame
	void Update ()
    {
        if (!isDead)
        {
            GetInput();

            if (dialogShowed)
            {
                dialogShowTime -= Time.deltaTime;
                if (dialogShowTime < 0)
                {
                    HidePanel();
                }
            }
            if (recentlyHit)
            {
                recentlyHitTime -= Time.deltaTime;
                if (recentlyHitTime < 0)
                {
                    recentlyHit = false;
                    GetComponent<Animator>().SetBool("hitByEnemy", false);
                }
            }

            if(healCooldown <= 0)
            {
                if (hp < 100)
                {
                    UpdateHealth(hp+1);
                }
                healCooldown = 3;
            }
            else
            {
                healCooldown -= Time.deltaTime;
            }
        }
        else
        {
            GameOver();
        }
        // Move();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && recentlyHit == false)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            var enemyPos = enemy.transform.position;
            var toEnemy = enemyPos - transform.position;
            enemy.GetComponent<Rigidbody2D>().AddForce(toEnemy.normalized * 200);
            GetComponent<Rigidbody2D>().AddForce(toEnemy.normalized * -200);

            int dmg = enemy.GetEnemyDmg();
            ApplyDamage(dmg);
           
        }
        if(collision.gameObject.tag == "Teleporter")
        {
            transform.position = collision.transform.GetChild(0).position; // Teleporter has only 1 child: Waypoint
        }
        if(collision.gameObject.tag == "Chest")
        {
            if (collision.gameObject.GetComponent<ChestBehaviour>() != null)
            {
                collision.gameObject.GetComponent<ChestBehaviour>().OpenChest(this.gameObject);
            }
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

        if (direction.x < 0)
        {
            facingLeft = true;
        }
        else if (direction.x > 0)
        {

            facingLeft = false;
        }
        GetComponent<SpriteRenderer>().flipX = facingLeft;

        if (Input.GetKey(KeyCode.J) && hasDash) //dash
        {
            if(dashCooldown < 1)
            {
                rbody.AddForce(direction.normalized * currentDashSpeed);
                dashCooldown = 30;
                GetComponent<ParticleSystem>().Play();
                dashSound.Play();
            }
            
        }
        if(dashCooldown == 15)
        {
            GetComponent<ParticleSystem>().Stop();
        }
        dashCooldown--;

        if (Input.GetKey(KeyCode.K) && attackCooldown < 1) //attack
        {
            attackCooldown = 30;
            if (facingLeft)
            {
                GetComponent<Animator>().SetBool("attackingLeft", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("attacking", true);
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("attacking", false);
            GetComponent<Animator>().SetBool("attackingLeft", false);
        }
        attackCooldown--;

        bool blockKeyHeld = Input.GetKey(KeyCode.L);
        if (blockKeyHeld != isBlocking && hasShield)
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

    public void UnlockAbility(int drop)
    {
        if(drop == 0 && hasDash == false)
        {
            hasDash = true;
            ShowPanel("Dash has been activated!");
        }
        else if(drop == 1 && hasUpgradedSword == false)
        {
            hasUpgradedSword = true;
            var sword = this.gameObject.transform.Find("sword").gameObject;
            sword.GetComponent<SwordBehaviour>().UnlockedGreaterSword();
            ShowPanel("Equipped Greater Sword!");
        }
        else if(drop == 2 && hasShield == false)
        {
            hasShield = true;
            ShowPanel("Equipped Shield!");
        }
    }

    public void ApplyDamage(int dmg)
    {
        recentlyHit = true;
        recentlyHitTime = 1f;

        if (isBlocking)
        {
            dmg /= 2;
        }
        hurtSound.Play();
        UpdateHealth((hp -= dmg) < 0 ? 0 : hp);
    }

    private void UpdateHealth(int newHp)
    {
        hp = newHp;

        float hpFill = (float)hp;
        hpFill /= 100;

        healthText.GetComponent<ChangeText>().UpdateText(hp.ToString());
        healthbar.GetComponent<HPBarBehaviour>().UpdateHPBar(hpFill);
        //GetComponent<Animator>().SetBool("hitByEnemy", true);

        if (hp <= 0 && !isDead)
        {
            isDead = true;

            deathtSound.Play();
        }
    }

    private void ShowPanel(string dialogText)
    {
        var textGameObject = DialogPanel.transform.Find("DialogText").gameObject;
        textGameObject.GetComponent<ChangeText>().UpdateText(dialogText);

        dialogShowed = true;
        DialogPanel.SetActive(true);
        dialogShowTime = 5.0f;
    }

    private void HidePanel()
    {
        DialogPanel.SetActive(false);
    }

    private void GameOver()
    {
        gameOverCanvas.SetActive(true);
    }
}
