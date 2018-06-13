using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnragedBoss : MonoBehaviour {

    [SerializeField]
    private GameObject enraged;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        enraged.SetActive(true);
        enraged.transform.position = transform.position;
    }
}
