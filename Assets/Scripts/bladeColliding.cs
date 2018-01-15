using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bladeColliding : MonoBehaviour {

    public static bool isCutting =false;

    public GameObject Swordhandle;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine("BladeStuck");
            followForce.Right_currentHandState = 3;
            isCutting = true;
        }
        if (other.CompareTag("InteractiveObject"))
        {
            InteractiveEnter();
        }
    }

    IEnumerator BladeStuck()
    {
        while (followForce.control_K > 1f)
        {
            followForce.control_K = followForce.control_K - 10f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            followForce.Right_currentHandState = 3;
        }
        if (other.CompareTag("InteractiveObject"))
        {
            InteractiveEnter();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            followForce.Right_currentHandState = 1;
            bladeCutting.SureCutThrough();/////////////고쳐야함

        }
        if (other.CompareTag("InteractiveObject"))
        {
            followForce.Right_currentHandState = 1;
        }
    }

    public void InteractiveEnter()
    {
        Swordhandle.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Swordhandle.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
