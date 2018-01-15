/**
 * @author : Yeonkun Lee
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareExitRoomControl : MonoBehaviour {
    public bool isNorthOpen;
    public bool isSouthOpen;

    public float DoorOpenSpeed = 0.005f;

    private Vector3 DoorNorthPosition;
    private Vector3 DoorSouthPosition;

    // Use this for initialization
    void Start () {
        DoorNorthPosition = transform.Find("DoorNorth").position;
        DoorSouthPosition = transform.Find("DoorSouth").position;
    }
	
	// Update is called once per frame
	void Update () {
        DoorUpdate();
    }

    void DoorUpdate()
    {
        if (isNorthOpen)
        {
            transform.Find("DoorNorth").position = Vector3.Slerp(transform.Find("DoorNorth").position, DoorNorthPosition + new Vector3(0, 4.3f, 0), DoorOpenSpeed);
        }
        else
        {
            transform.Find("DoorNorth").position = Vector3.Slerp(transform.Find("DoorNorth").position, DoorNorthPosition, DoorOpenSpeed);
        }
       
        if (isSouthOpen)
        {
            transform.Find("DoorSouth").position = Vector3.Slerp(transform.Find("DoorSouth").position, DoorSouthPosition + new Vector3(0, 4.3f, 0), DoorOpenSpeed);
        }
        else
        {
            transform.Find("DoorSouth").position = Vector3.Slerp(transform.Find("DoorSouth").position, DoorSouthPosition, DoorOpenSpeed);
        }
    }
}
