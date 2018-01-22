using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldSound : MonoBehaviour
{
    public AudioClip SwordShield1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sword")
        {
            //Debug.Log(Vector3.Magnitude(collision.impulse));
            if (Vector3.Magnitude(collision.impulse) > 0.15f)
            {
                GetComponent<AudioSource>().PlayOneShot(SwordShield1, Mathf.Clamp(Vector3.Magnitude(collision.impulse) / 5, 0f, 1f));
            }
        }
    }
}