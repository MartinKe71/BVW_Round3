using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {


	public int obs_width;
	
	// Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {	
	    
    }

    private void OnTriggerEnter(Collider other) {
	    if (other.CompareTag("Player1")) {
		    //Debug.Log("Front player got in touch");
		    FindObjectOfType<MovementController>().shouldBeAbleToJump = true;
		    FindObjectOfType<MovementController>().obstacleWidth = obs_width;
	    }
    }

    private void OnTriggerExit(Collider other) {
	    if (other.CompareTag("Player1")) {
		    FindObjectOfType<MovementController>().shouldBeAbleToJump = false;
	    }
    }
}
