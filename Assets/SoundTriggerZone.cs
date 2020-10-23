using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerZone : MonoBehaviour {
	
	public float hearingDistance = 3.0f;
	
	private bool _isPlaying;
    // Start is called before the first frame update
    void Start() {
	    _isPlaying = false;
    }

    // Update is called once per frame
    void Update() {
	    if (Vector3.Distance(FindObjectOfType<ResetJump>().transform.position, transform.position) < hearingDistance && _isPlaying == false) {
		    _isPlaying = true;
		    FindObjectOfType<AudioManager>().BGM[1].Play();;
	    } else if(Vector3.Distance(FindObjectOfType<ResetJump>().transform.position, transform.position) > hearingDistance && _isPlaying == true) {
		    _isPlaying = false;
		    FindObjectOfType<AudioManager>().BGM[1].Stop();;
	    }
    }
}
