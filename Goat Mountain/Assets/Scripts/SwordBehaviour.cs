using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour {

    [SerializeField]
    private Sprite defaultSword;

    [SerializeField]
    private Sprite greaterSword;

    private bool isGreaterSword;

    // Use this for initialization
    void Start () {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSword;
        isGreaterSword = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnlockedGreaterSword()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = greaterSword;
        isGreaterSword = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().ApplyDamage(isGreaterSword? 20:10);
        }
    }

}
