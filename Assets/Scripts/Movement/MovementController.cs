using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum MovingDirection {
    up,
    down,
    left,
    right,
    front,
    back
}



public class MovementController : MonoBehaviour {
	public bool useAirConsole = false;
	public bool anotherMovement = false;
	public float threshHoldForFallDown = 2.9f;
	public float unitStep = 1f;
	public float moveDurationTime = 0.5f;
	public bool isJumping;
    public bool inTheAir;
	
	// Position for first 2, facing dir for 3-4
	public Vector3[] respawnData;
	
	// Person in fron, keyboard(WASD)
	public GameObject front;

	// Person in back, keyboard(arrows)
	public GameObject back;

	// Special movement for jump
	public bool shouldBeAbleToJump = false;
	private bool _hasliffted = false;
	public int obstacleWidth = -1;

	private bool _frontCanMove;
	private bool _backCanMove;
	private bool _canMove;
	private GameManager _gameManager;
	
	private void Start() {
		_gameManager = FindObjectOfType<GameManager>();
		_frontCanMove = true;
		_backCanMove = true;
		_canMove = true;
		isJumping = false;
        inTheAir = false;
        front.transform.position = respawnData[0];
		back.transform.position = respawnData[1];
		front.transform.forward = respawnData[2];
		back.transform.forward = respawnData[3];
	}


	private void Update() {
	    if (!useAirConsole) {
	        MovementCheck();
        }
		//BackPersonFaceUpdate();
	    JumpCheck();
	    CheckRuleForDistance();
    }

	private void BackPersonFaceUpdate() {
		back.transform.LookAt(front.transform.position);
	}

	private void CheckRuleForDistance() {
		if (Vector3.Distance(front.transform.position, back.transform.position) > threshHoldForFallDown) {
			Debug.Log("You break the rule, should fall down");
			if(front.GetComponent<Animator>().GetBool("shouldFall") == false){
				front.GetComponent<Animator>().SetBool("shouldFall", true);
				back.GetComponent<Animator>().SetBool("shouldFallLong", true);
				StartCoroutine(PrepForRespawn(5.0f));
				FindObjectOfType<AudioManager>().PlayFallDownSFX();
			}		
			_canMove = false;
		}
	}

    private void JumpCheck() {
	    if (shouldBeAbleToJump && Vector3.Distance(front.transform.position, back.transform.position) <= unitStep) {
		    if (Input.GetKeyDown(KeyCode.Q) && _hasliffted == false && isJumping == false) {
			    //MovePlayer(MovingDirection.up, front);
			    //front.transform.rotation = Quaternion.Euler(0,0,45);
			    front.GetComponent<Animator>().SetBool("shouldLift", true);
			    back.GetComponent<Animator>().SetBool("shouldLift", true);
			    _canMove = false;
                inTheAir = true;
			    //_hasliffted = true;
		    }
		    else if (Input.GetKeyDown(KeyCode.Q) && _hasliffted && isJumping) {
			    //MovePlayer(MovingDirection.down, front);
			    //front.transform.rotation = Quaternion.Euler(0,0,0);
			    front.GetComponent<Animator>().SetBool("shouldLift", false);
			    back.GetComponent<Animator>().SetBool("shouldLift", false);
			    isJumping = false;
			    _hasliffted = false;
			    _canMove = true;
		    } 
		    else if (Input.GetKeyDown(KeyCode.Space) && _hasliffted && isJumping) {
				// front.transform.rotation = Quaternion.Euler(0,0,0);
				isJumping = false;
				FindObjectOfType<AudioManager>().SFX[2].Play();
				front.GetComponent<Animator>().SetBool("jump", true);
				back.GetComponent<Animator>().SetBool("jump", true);
				front.GetComponent<Rigidbody>().DOMove(front.transform.position + front.transform.forward * obstacleWidth, 0.6f);
				back.GetComponent<Rigidbody>().DOMove(back.transform.position + back.transform.forward * obstacleWidth, 0.6f);
				//MovePlayer(MovingDirection.down, front);
		    }
		    else if (Input.GetKeyDown(KeyCode.Space) && _hasliffted == false) {
			    // Something bad together
		    }
	    }
    }

    public void FinishLiftUp() {
	    _hasliffted = true;
    }

    // public void FinishJumping() {
	   //  isJumping = false;
    // }


    public void JumpCheckForAC(GameObject whoIsJumping) {
	    if (shouldBeAbleToJump && Vector3.Distance(front.transform.position, back.transform.position) <= unitStep) {
		    if (_hasliffted == false && whoIsJumping.CompareTag("Player1") && isJumping == false) {
			    //MovePlayer(MovingDirection.up, front);
			    //front.transform.rotation = Quaternion.Euler(0,0,45);
			    front.GetComponent<Animator>().SetBool("shouldLift", true);
			    back.GetComponent<Animator>().SetBool("shouldLift", true);
			    _canMove = false;
			    isJumping = true;
                inTheAir = true;
                //_hasliffted = true;
                //StartCoroutine(CouldBeLiftUp());
            }
		    else if (whoIsJumping.CompareTag("Player1") && _hasliffted && isJumping) {
			    //MovePlayer(MovingDirection.down, front);
			    //front.transform.rotation = Quaternion.Euler(0,0,0);
			    front.GetComponent<Animator>().SetBool("shouldLift", false);
			    back.GetComponent<Animator>().SetBool("shouldLift", false);
			    _canMove = true;
			    _hasliffted = false;
			    isJumping = false;
                inTheAir = false;
            }
		    
		    else if (_hasliffted && whoIsJumping.CompareTag("Player2") && isJumping) {
			    // //front.transform.rotation = Quaternion.Euler(0,0,0);
			    // front.transform.position += new Vector3(obstacleWidth, 0,0);
			    // back.transform.position += new Vector3(obstacleWidth, 0, 0);
			    //MovePlayer(MovingDirection.down, front);
			    isJumping = false;
			    FindObjectOfType<AudioManager>().SFX[2].Play();
			    front.GetComponent<Animator>().SetBool("jump", true);
			    back.GetComponent<Animator>().SetBool("jump", true);
                front.GetComponent<Rigidbody>().DOMove(front.transform.position + front.transform.forward * obstacleWidth, 0.6f);
                back.GetComponent<Rigidbody>().DOMove(back.transform.position + back.transform.forward * obstacleWidth, 0.6f);

       //         front.transform.position += front.transform.forward * obstacleWidth;
			    //back.transform.position += back.transform.forward * obstacleWidth;
		    }
		    else if (Input.GetKeyDown(KeyCode.Space) && _hasliffted == false) {
			    // Something bad together
		    }
	    }
    }
    

    private void MovementCheck() {
	    if (Input.GetKeyDown(KeyCode.W))
		    MovePlayer(MovingDirection.front, front);
	    else if (Input.GetKeyDown(KeyCode.S))
		    MovePlayer(MovingDirection.back, front);
	    else if (Input.GetKeyDown(KeyCode.A))
		    MovePlayer(MovingDirection.left, front);
	    else if (Input.GetKeyDown(KeyCode.D))
		    MovePlayer(MovingDirection.right, front);

	    if (Input.GetKeyDown(KeyCode.UpArrow))
		    MovePlayer(MovingDirection.front, back);
	    else if (Input.GetKeyDown(KeyCode.DownArrow))
		    MovePlayer(MovingDirection.back, back);
	    else if (Input.GetKeyDown(KeyCode.LeftArrow))
		    MovePlayer(MovingDirection.left, back);
	    else if (Input.GetKeyDown(KeyCode.RightArrow))
		    MovePlayer(MovingDirection.right, back);
    }

    public void MovePlayer(MovingDirection direction, GameObject whoIsMoving) {
	    if (_canMove) {
		    if (anotherMovement == false) {
			    if (whoIsMoving.CompareTag("Player1") && _frontCanMove ) {
				    MovingLogic1(direction, whoIsMoving);
			    }
			    else if (whoIsMoving.CompareTag("Player2") && _backCanMove) {
				    MovingLogic1(direction, whoIsMoving);
			    }
		    } else {
			    MovingLogic2(direction, whoIsMoving);
		    }
	    }
    }

    private void MovingLogic1(MovingDirection direction, GameObject whoIsMoving) {
	    Vector3 movingVector = new Vector3(0, 0, 0);
	    switch (direction) {
		    case MovingDirection.back:
			    //whoIsMoving.transform.LookAt(new Vector3(0,0,-1));
			    if (whoIsMoving.GetComponent<Rigidbody>().rotation == Quaternion.Euler(0, 180, 0)) {
				    movingVector = new Vector3(0, 0, -1 * unitStep);
			    } else {
				    whoIsMoving.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			    }

			    //whoIsMoving.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			    //movingVector = new Vector3(0, 0, -1 * unitStep);
			    break;
		    case MovingDirection.front:
			    //whoIsMoving.transform.LookAt(new Vector3(0,0,1));
			    if (whoIsMoving.GetComponent<Rigidbody>().rotation == Quaternion.Euler(new Vector3(0, 0, 0))) {
				    movingVector = new Vector3(0, 0, 1 * unitStep);
			    } else {
				    whoIsMoving.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			    }

			    //whoIsMoving.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			    break;
		    case MovingDirection.left:
			    //whoIsMoving.transform.LookAt(new Vector3(-1,0,0));
			    if (whoIsMoving.GetComponent<Rigidbody>().rotation == Quaternion.Euler(new Vector3(0, -90, 0))) {
				    movingVector = new Vector3(-1 * unitStep, 0, 0);
			    } else {
				    whoIsMoving.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0, -90, 0));
			    }

			    //whoIsMoving.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
			    break;
		    case MovingDirection.right:
			    //whoIsMoving.transform.LookAt(new Vector3(1,0,0));
			    if (whoIsMoving.GetComponent<Rigidbody>().rotation == Quaternion.Euler(new Vector3(0, 90, 0))) {
				    movingVector = new Vector3(1 * unitStep, 0, 0);
			    } else {
				    whoIsMoving.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0, 90, 0));
			    }

			    //whoIsMoving.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
			    break;
		    case MovingDirection.up:
			    movingVector = new Vector3(0, 1 * unitStep, 0);
			    break;
		    case MovingDirection.down:
			    movingVector = new Vector3(0, -1 * unitStep, 0);
			    break;
		    default:
			    break;
	    }

	    if (whoIsMoving.CompareTag("Player1") && movingVector != Vector3.zero) {
		    movingVector = CheckSelfCollide(whoIsMoving.transform.position + movingVector, back.transform.position)
			    ? movingVector
			    : Vector3.zero;
		    front.GetComponent<Animator>().SetBool("shouldWalk", movingVector != Vector3.zero);
	    } else if (whoIsMoving.CompareTag("Player2") && movingVector != Vector3.zero) {
		    movingVector = CheckSelfCollide(whoIsMoving.transform.position + movingVector, front.transform.position)
			    ? movingVector
			    : Vector3.zero;
		    back.GetComponent<Animator>().SetBool("shouldWalk", movingVector != Vector3.zero);
	    }
	    whoIsMoving.GetComponent<Rigidbody>().DOMove(whoIsMoving.transform.position + movingVector, moveDurationTime);
	    if(movingVector != Vector3.zero){
			//_canMove = false;
			if (whoIsMoving.CompareTag("Player1")) {
				_frontCanMove = false;
				StartCoroutine(SafeGuardForMoveFlag(whoIsMoving));
			}
			else if (whoIsMoving.CompareTag("Player2")) {
				_backCanMove = false;
				StartCoroutine(SafeGuardForMoveFlag(whoIsMoving));
			}
	    }
	    //front.GetComponent<Rigidbody>().DOMove(front.transform.position + front.transform.forward * obstacleWidth, 0.6f);

    }

    IEnumerator SafeGuardForMoveFlag(GameObject go) {
	    yield return new WaitForSecondsRealtime(moveDurationTime);
	    if (go.CompareTag("Player1")) {
		    front.GetComponent<Animator>().SetBool("shouldWalk", false);
		    _frontCanMove = true;
	    }
	    else if (go.CompareTag("Player2")) {
		    back.GetComponent<Animator>().SetBool("shouldWalk", false);
		    _backCanMove = true;
	    }
    }
    
    

    private void MovingLogic2(MovingDirection direction, GameObject whoIsMoving) {
	    switch (direction) {
		    case MovingDirection.back:
			    whoIsMoving.GetComponent<Rigidbody>().MovePosition(whoIsMoving.transform.position + CheckMovement(whoIsMoving, -1));
			    //whoIsMoving.transform.position = CheckMovement(whoIsMoving, -1);
			    break;
		    case MovingDirection.front:
			    whoIsMoving.GetComponent<Rigidbody>().MovePosition(whoIsMoving.transform.position + CheckMovement(whoIsMoving, 1));
			    //whoIsMoving.transform.position = CheckMovement(whoIsMoving, 1);
			    break;
		    case MovingDirection.left:
			    //if (whoIsMoving.CompareTag("Player1")) {
				    whoIsMoving.transform.Rotate(whoIsMoving.transform.up, -90);
			    //}
			    break;
		    case MovingDirection.right:
			    //if (whoIsMoving.CompareTag("Player1")) {
				    whoIsMoving.transform.Rotate(whoIsMoving.transform.up, 90);
			    //}
			    break;
		    case MovingDirection.up:
			    whoIsMoving.GetComponent<Rigidbody>().MovePosition(whoIsMoving.transform.position + new Vector3(0, unitStep, 0));
			    break;
		    case MovingDirection.down:
			    whoIsMoving.GetComponent<Rigidbody>().MovePosition(whoIsMoving.transform.position - new Vector3(0, unitStep, 0));
			    break;
	    }
    }



    private Vector3 CheckMovement(GameObject whoIsMoving, int sign) {
	    //Debug.Log(whoIsMoving.transform.forward);
	    Vector3 temp = whoIsMoving.transform.forward * unitStep;
	    //Debug.Log(temp);
	    // if (whoIsMoving.CompareTag("Player2")) {
		   // temp = new Vector3(temp.x >= 0 ? Mathf.Ceil(temp.x) : Mathf.Floor(temp.x), temp.y, temp.z >= 0 ? Mathf.Ceil(temp.z) : Mathf.Floor(temp.z));
	    // }
	    // temp = new Vector3(temp.x > 0 ? Mathf.Ceil(temp.x) : Mathf.Floor(temp.x), temp.y,
		   //  temp.z > 0 ? Mathf.Ceil(temp.z) : Mathf.Floor(temp.z));
		   bool result = whoIsMoving.CompareTag("Player1") ? CheckSelfCollide(whoIsMoving.transform.position + sign * temp,back.transform.position) : CheckSelfCollide(whoIsMoving.transform.position + sign * temp,front.transform.position);
	    return result ? sign * temp : Vector3.zero;
    }
    
    private bool CheckSelfCollide(Vector3 target, Vector3 alpha) {
	    if(Vector3.Distance(target,alpha) < 0.1f){
		    //Debug.Log("should not be moved any more");
		    if(front.GetComponent<Animator>().GetBool("shouldFall") == false){
			    Debug.Log("Starting playing anime" + front.GetComponent<Animator>().GetBool("shouldFall"));
				front.GetComponent<Animator>().SetBool("shouldFall", true);
				back.GetComponent<Animator>().SetBool("shouldFall", true);
				StartCoroutine(PrepForRespawn(2.0f));
				FindObjectOfType<AudioManager>().SFX[1].Play();
		    }
		    _canMove = false;
		    return false;
	    }
		//Debug.Log("x: " + (Mathf.Ceil(target.x) - _gameManager.xOffset) + "  z: " + (Mathf.Ceil(target.z) - _gameManager.zOffset) + _gameManager.gridMap[(int)Mathf.Ceil(target.x) - _gameManager.xOffset, (int)Mathf.Ceil(target.z) - _gameManager.zOffset]);
	    bool temp = _gameManager.gridMap[(int)Mathf.Ceil(target.x) - _gameManager.xOffset, (int)Mathf.Ceil(target.z) - _gameManager.zOffset];
	    //front.GetComponent<Animator>().SetBool("shouldWalk",temp);
		return _gameManager.gridMap[(int)Mathf.Ceil(target.x) - _gameManager.xOffset, (int)Mathf.Ceil(target.z) - _gameManager.zOffset];
	    //return true;
    }

    public void Die() {
	    if(front.GetComponent<Animator>().GetBool("shouldFall") == false){
		    Debug.Log("Starting playing anime" + front.GetComponent<Animator>().GetBool("shouldFall"));
		    front.GetComponent<Animator>().SetBool("shouldFall", true);
		    back.GetComponent<Animator>().SetBool("shouldFall", true);
		    StartCoroutine(PrepForRespawn(2.0f));
		    FindObjectOfType<AudioManager>().PlayFallDownSFX();
	    }
	    _canMove = false;
    }


    void ResetFlag() {
	    _hasliffted = false;
	    _canMove = true;
	    _frontCanMove = true;
	    _backCanMove = true;
	    front.transform.position = respawnData[0];
	    front.transform.forward = respawnData[2];
	    back.transform.position = respawnData[1];
	    back.transform.forward = respawnData[3];
	    front.GetComponent<Animator>().SetBool("shouldFall", false);
	    front.GetComponent<Animator>().SetBool("shouldWalk", false);
	    back.GetComponent<Animator>().SetBool("shouldFall", false);
	    back.GetComponent<Animator>().SetBool("shouldWalk", false);
	    back.GetComponent<Animator>().SetBool("shouldFallLong", false);
    }

    IEnumerator PrepForRespawn(float waitTime) {
	    yield return new WaitForSecondsRealtime(waitTime);
	    ResetFlag();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit!");
        if (other.transform.CompareTag("Obstacles"))
        {
            Debug.Log("Don't move forward!");
        }
    }

    public void LockMovement() {
	    _canMove = false;
    }

    public void UnLockMovement() {
	    _canMove = true;
    }

    public void ResetHasLifted()
    {
        _hasliffted = false;
    }

    public void UnLockFrontMovement() {
	    _frontCanMove = true;
    }

    public void UnLockBackMovement() {
	    _backCanMove = true;
    }
}
