using System.Collections;
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

    [SerializeField]
    private bool hasShield;

    [SerializeField]
    private bool hasUpgradedSword;

    [SerializeField]
    private bool hasDash;

    [SerializeField]
    private GameObject DialogPanel;

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
        if (dialogShowed)
        {
            dialogShowTime -= Time.deltaTime;
            if(dialogShowTime < 0)
            {
                HidePanel();
            }
        }
        // Move();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
           
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
            ShowPanel("Equiped Greater Sword!");
        }
        else if(drop == 2 && hasShield == false)
        {
            hasShield = true;
            ShowPanel("Equiped Shield!");
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
}
