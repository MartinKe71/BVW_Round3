using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour {

	public bool ableToWalkOn;

	private GameManager _gameManager;
	// Start is called before the first frame update
    void Start() {
	    _gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update() {
	    if (ableToWalkOn) {
		    _gameManager.gridMap[(int) Mathf.Ceil(transform.position.x) - _gameManager.xOffset, (int)Mathf.Ceil(transform.position.z) - _gameManager.zOffset] = true;
		    //Debug.Log(((int) transform.position.x - _gameManager.xOffset) + " " + ((int) transform.position.z - _gameManager.zOffset) + _gameManager.gridMap[(int) transform.position.x - _gameManager.xOffset, (int) transform.position.z - _gameManager.zOffset]);
	    }
    }
}
