using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour {

    [SerializeField]
    private Sprite chestClosed;

    [SerializeField]
    private Sprite chestOpened;

    [Tooltip("0: Dash, 1: Greater Sword, 2: Shield")]
    [SerializeField]
    private int drop;

	// Use this for initialization
	void Start ()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = chestClosed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        		
	}

    public void OpenChest(GameObject player)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = chestOpened;

        player.GetComponent<Player>().UnlockAbility(drop);
    }
}
