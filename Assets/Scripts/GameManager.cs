using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int width, length;

	public bool[,] gridMap;
	//public Vector2[] cannotReach;
	public int xOffset, zOffset;
	
	
    // Start is called before the first frame update
    void Start() {
	    gridMap = new bool[length, width];
	    // for (int i = 0; i < cannotReach.Length; i++) {
		   //  hasBeenOccupied[(int)cannotReach[i].x, (int)cannotReach[i].y] = true;
	    // }
    }

    // Update is called once per frame
    void Update() {
	    if (Input.GetKeyDown(KeyCode.I)) {
		    for (int i = 0; i < length; i++) {
			    for (int j = 0; j < width; j++) {
				    if (gridMap[i, j]) {
					    Debug.Log("x: " + i + " z: " + j);
				    }
			    }
		    }
	    }   
    }
}
