using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bladeColliding_Ver_2 : MonoBehaviour {

    public static bool isCutting =false;
    public GameObject Swordhandle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine("BladeStuck");
            //followForce.ColliderEnterMotion();
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

            //followForce.angularMomentumInverse_Store = followForce.angularMomentumInverse;
            //followForce.angularMomentumInverse = 0.008f;

            //followForce.isCollidingObstacle = true;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            //followForce.ColliderStayMotion();
        }

        if (other.CompareTag("InteractiveObject"))
        {
            InteractiveStay();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            //followForce.ColliderExitMotion();

            bladeCutting.SureCutThrough();

        }
        if (other.CompareTag("InteractiveObject"))
        {
            //followForce.InteractiveExitMotion();
        }
    }

    private void InteractiveEnter()
    {
        Swordhandle.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Swordhandle.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void InteractiveStay()
    {

    }
}
