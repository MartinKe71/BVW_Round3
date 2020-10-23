using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSpill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") && !FindObjectOfType<MovementController>().inTheAir)
        {
            //FindObjectOfType<AudioManager>().SFX[3].Play();
            Debug.Log("you died");
            FindObjectOfType<MovementController>().Die();
        }
    }
}
