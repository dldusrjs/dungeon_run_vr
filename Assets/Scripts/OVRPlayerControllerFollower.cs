using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRPlayerControllerFollower : MonoBehaviour {

    public GameObject OVRPlayerController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = OVRPlayerController.transform.position;
	}
}
