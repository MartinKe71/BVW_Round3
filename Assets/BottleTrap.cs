using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleTrap : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
	    if (other.CompareTag("Player1") || other.CompareTag("Player2")) {
		    FindObjectOfType<AudioManager>().SFX[3].Play();
		    FindObjectOfType<MovementController>().Die();
	    }
    }
}
