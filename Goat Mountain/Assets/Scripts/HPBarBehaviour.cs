using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarBehaviour : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHPBar(float hpFill)
    {
        GetComponent<Image>().fillAmount = hpFill;
    }
}
