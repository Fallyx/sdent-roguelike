using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehaviour : MonoBehaviour {
    public GameObject potion;

    private float potionSpawned = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        potionSpawned -= Time.deltaTime;

        var player = GameObject.Find("Character");
        var playerPos = player.transform.position;
        var toPlayer = playerPos - transform.position;
        if (toPlayer.magnitude < 7 && potionSpawned <= 0)
        {
            var pot = Instantiate(potion);
            pot.gameObject.transform.position = transform.position + (toPlayer.normalized * 1);
            pot.GetComponent<BossProjectileBehaviour>().SetDirection(toPlayer);
            potionSpawned = 3.0f;
        }
    }
}
