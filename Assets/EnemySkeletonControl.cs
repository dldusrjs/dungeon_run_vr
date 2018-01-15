using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeletonControl : MonoBehaviour {

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;


	// Use this for initialization
	void Start () {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();

        nvAgent.destination = playerTr.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
