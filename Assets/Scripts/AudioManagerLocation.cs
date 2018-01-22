using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerLocation : MonoBehaviour {

    public Transform PlayerTr;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = PlayerTr.position;
        transform.rotation = PlayerTr.rotation;
	}
}
