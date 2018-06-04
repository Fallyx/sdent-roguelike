using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        UpdateText("");
        this.transform.parent.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpdateText(string dialogText)
    {
        text = GetComponent<Text>();
        text.text = dialogText;
    }
}
