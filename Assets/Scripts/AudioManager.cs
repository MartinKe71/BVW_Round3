using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource[] BGM;
	public AudioSource[] SFX;
	
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFallDownSFX() {
	    SFX[0].Play();
    }
}
