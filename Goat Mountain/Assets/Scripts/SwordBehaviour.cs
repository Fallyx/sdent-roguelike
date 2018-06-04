using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour {

    [SerializeField]
    private Sprite defaultSword;

    [SerializeField]
    private Sprite greaterSword;

    // Use this for initialization
    void Start () {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSword;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnlockedGreaterSword()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = greaterSword;
    }
}
