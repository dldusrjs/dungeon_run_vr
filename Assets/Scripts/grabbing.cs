using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbing : MonoBehaviour {

    public OVRInput.Controller controller;
    public string buttonName;
    private GameObject Right_grabbedObject;
    public static bool rightIsGrabbed;
    public float GrabRadius;
    public LayerMask GrabMask;
    public static bool rightIsmoving = false;
    public static Vector3 rightControllerMovingSpeed;
    public static float rightControllerMovingSpeedNorm;

    void GrabObject()
    {
        rightIsGrabbed = true;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, GrabRadius, transform.forward, 0f, GrabMask);

        if (hits.Length > 0)
        {
            int closestHit = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < hits[closestHit].distance)
                {
                    closestHit = i;
                }
            }
            Right_grabbedObject = hits[closestHit].transform.gameObject;
            if (Right_grabbedObject.tag == "Sword" || Right_grabbedObject.tag == "Shield")
            {
                Right_grabbedObject.transform.position = transform.position;
                Right_grabbedObject.AddComponent<FixedJoint>();
                Right_grabbedObject.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
                if (Right_grabbedObject.tag == "Sword") followForce.Right_currentHandState = 1;
                if (Right_grabbedObject.tag == "Shield") followForce.Right_currentHandState = 2;
            }
            else
            {
                Right_grabbedObject.transform.position = transform.position;
                Right_grabbedObject.transform.parent = transform;
            }
        }

    }

    void DropObject()
    {
        rightIsGrabbed = false;
        if (Right_grabbedObject != null)
        {
            if (Right_grabbedObject.tag == "Sword" || Right_grabbedObject.tag == "Shield")
            {
                Destroy(Right_grabbedObject.GetComponent<FixedJoint>());
                followForce.Right_currentHandState = 0;
            }

            else
            {
                Right_grabbedObject.transform.parent = null;
                Right_grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                Right_grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(controller);
                Right_grabbedObject.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);
                Right_grabbedObject = null;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
        if (!rightIsGrabbed && Input.GetAxis(buttonName) == 1)
        {
            GrabObject();
        }

        if (rightIsGrabbed && Input.GetAxis(buttonName) < 1)
        {
            DropObject();
        }

        rightIsmoving = (Vector3.Magnitude(OVRInput.GetLocalControllerVelocity(controller)) > 0.5f);
        rightControllerMovingSpeedNorm = Vector3.Magnitude(rightControllerMovingSpeed);

        //GrabCheck();
    }
    
    void GrabCheck()
    {
        if (transform.childCount == 2) //손이 손가락 두개를 child로 가지고 있어서
        {
            followForce.Right_currentHandState = 0;
        }

        else if (transform.Find("swordHandle") != null && !(followForce.Right_currentHandState ==3))
        {
            followForce.Right_currentHandState = 1;
        }
        else if (transform.Find("shieldHandle") != null && !(followForce.Right_currentHandState == 3))
        {
            followForce.Right_currentHandState = 2;
        }
    }
}
