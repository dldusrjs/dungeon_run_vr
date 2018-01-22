using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEntranceRoomControl : MonoBehaviour {
    public bool isNorthOpen;
    public bool isEastOpen;
    public bool isWestOpen;

    public float DoorOpenSpeed = 0.005f;

    private Vector3 DoorNorthPosition;
    private Vector3 DoorEastPosition;
    private Vector3 DoorWestPosition;


    // Use this for initialization
    void Start () {
        DoorNorthPosition = transform.Find("DoorNorth").position;
        DoorEastPosition = transform.Find("DoorEast").position;
        DoorWestPosition = transform.Find("DoorWest").position;
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
        if (isEastOpen)
        {
            transform.Find("DoorEast").position = Vector3.Slerp(transform.Find("DoorEast").position, DoorEastPosition + new Vector3(0, 4.3f, 0), DoorOpenSpeed);
        }
        else
        {
            transform.Find("DoorEast").position = Vector3.Slerp(transform.Find("DoorEast").position, DoorEastPosition, DoorOpenSpeed);
        }
        if (isWestOpen)
        {
            transform.Find("DoorWest").position = Vector3.Slerp(transform.Find("DoorWest").position, DoorWestPosition + new Vector3(0, 4.3f, 0), DoorOpenSpeed);
        }
        else
        {
            transform.Find("DoorWest").position = Vector3.Slerp(transform.Find("DoorWest").position, DoorWestPosition, DoorOpenSpeed);
        }

    }
}
