using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int dmg;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var player = GameObject.Find("Character");
        var playerPos = player.transform.position;
        var toPlayer = playerPos - transform.position;
        if (toPlayer.magnitude > 1)
        {
            //GetComponent<Rigidbody2D>().
            GetComponent<Rigidbody2D>().AddForce(toPlayer.normalized * 0.5f);
        }
	}

    public int GetEnemyDmg()
    {
        return dmg;
    }
}
