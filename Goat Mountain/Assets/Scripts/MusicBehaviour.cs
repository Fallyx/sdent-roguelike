using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehaviour : MonoBehaviour {

    private float keyPressedTime;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        keyPressedTime -= Time.deltaTime;
        if (keyPressedTime <0)
        {
            GetInput();
        }
	}

    private void GetInput()
    {
        if(Input.GetKey(KeyCode.M))
        {
            this.gameObject.GetComponent<AudioSource>().mute = !this.gameObject.GetComponent<AudioSource>().mute;
            keyPressedTime = 0.5f;
        }
    }
}
