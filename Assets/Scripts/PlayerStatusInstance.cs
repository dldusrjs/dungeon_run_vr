using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusInstance : MonoBehaviour {

    public static PlayerStatusInstance Player;

    private float minimumDamageImpulse = 0.5f;

    public static float playerHealth = 100.0f;
    public float incomingDamageAmplifier = 8f;

    #region Singleton

    void Awake()
    {

        if (Player == null)
        {
            Player = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    // Use this for initialization
    void Start () {
        playerHealth = 100f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Vector3.Magnitude(collision.impulse) > minimumDamageImpulse)
            {
                playerHealth -= Vector3.Magnitude(collision.impulse) * incomingDamageAmplifier; // 플레이어 대미지 입음

                FindObjectOfType<AudioManager>().Play("PlayerDamage1", 0.5f);
                FindObjectOfType<AudioManager>().Play("PlayerDamage2", 0.5f);
            }
        }
    }
}
