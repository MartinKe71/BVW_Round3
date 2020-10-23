using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetJump : MonoBehaviour
{
    public void resetJump()
    {
        FindObjectOfType<MovementController>().inTheAir = false;
        GetComponent<Animator>().SetBool("jump", false);
        GetComponent<Animator>().SetBool("shouldLift", false);
        //FindObjectOfType<MovementController>().FinishJumping();
        FindObjectOfType<MovementController>().UnLockMovement();
        FindObjectOfType<MovementController>().ResetHasLifted();
    }

    public void resetDrink() {
	    GetComponent<Animator>().SetBool("shouldDrink", false);
    }

    public void startDrink() {
	    GetComponent<Animator>().SetBool("shouldDrink", true);
    }

    public void resetWalkFront() {
	    GetComponent<Animator>().SetBool("shouldWalk", false);
	    //FindObjectOfType<MovementController>().UnLockMovement();
	    FindObjectOfType<MovementController>().UnLockFrontMovement();
    }

    public void resetWalkBack() {
	    GetComponent<Animator>().SetBool("shouldWalk", false);
	    FindObjectOfType<MovementController>().UnLockBackMovement();
    }

    public void hasBeenLiftedUp() {
	    FindObjectOfType<MovementController>().FinishLiftUp();

    }

    // public void unlockMovementAfterWalk() {
	   //  GetComponent<MovementController>().UnLockMovement();
    // }
}
