using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftGrabbing : MonoBehaviour
{

    public OVRInput.Controller L_controller;
    public string L_buttonName;
    public static GameObject Left_grabbedObject;
    public static bool Left_isGrabbed;
    public float Left_GrabRadius;
    public LayerMask GrabMask;
    public static bool L_ismoving = false;
    public static Vector3 L_controllerMovingSpeed;
    public static float L_controllerMovingSpeedNorm;

    void L_GrabObject()
    {
        Left_isGrabbed = true;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, Left_GrabRadius, transform.forward, 0f, GrabMask);

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
            Left_grabbedObject = hits[closestHit].transform.gameObject;
            if (Left_grabbedObject.tag == "Sword" || Left_grabbedObject.tag == "Shield")
            {
                Left_grabbedObject.transform.position = transform.position;
                Left_grabbedObject.AddComponent<FixedJoint>();
                Left_grabbedObject.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
                if (Left_grabbedObject.tag == "Sword") LeftFollowForce.Left_currentHandState = 1;
                if (Left_grabbedObject.tag == "Shield") LeftFollowForce.Left_currentHandState = 2;
            }
            else
            {
                Left_grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                Left_grabbedObject.transform.position = transform.position;
                Left_grabbedObject.transform.parent = transform;
            }
        }

    }

    void L_DropObject()
    {
        Left_isGrabbed = false;
        if (Left_grabbedObject != null)
        {
            if (Left_grabbedObject.tag == "Sword" || Left_grabbedObject.tag == "Shield")
            {
                Destroy(Left_grabbedObject.GetComponent<FixedJoint>());
                LeftFollowForce.Left_currentHandState = 0;
            }

            else
            {
                Left_grabbedObject.transform.parent = null;
                Left_grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                Left_grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(L_controller);
                Left_grabbedObject.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(L_controller);
                Left_grabbedObject = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!Left_isGrabbed && Input.GetAxis(L_buttonName) == 1)
        {
            L_GrabObject();
        }

        if (Left_isGrabbed && Input.GetAxis(L_buttonName) < 1)
        {
            L_DropObject();
        }

        L_ismoving = (Vector3.Magnitude(OVRInput.GetLocalControllerVelocity(L_controller)) > 0.5f);
        L_controllerMovingSpeedNorm = Vector3.Magnitude(L_controllerMovingSpeed);

        //GrabCheck();
    }
    
    void GrabCheck()
    {
        if (transform.childCount == 2) //손이 손가락 두개를 child로 가지고 있어서
        {
            LeftFollowForce.Left_currentHandState = 0;
        }

        else if (transform.Find("swordHandle") != null && !(LeftFollowForce.Left_currentHandState == 3))
        {
            LeftFollowForce.Left_currentHandState = 1;
        }
        else if (transform.Find("shieldHandle") != null && !(LeftFollowForce.Left_currentHandState == 3))
        {
            LeftFollowForce.Left_currentHandState = 2;
        }
    }
}
