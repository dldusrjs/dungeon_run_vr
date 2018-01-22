using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordSound : MonoBehaviour {
    public AudioClip SwordShield1;

    public AudioClip SwordMetal1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sword")
        {
            //Debug.Log(Vector3.Magnitude(collision.impulse));
            if (Vector3.Magnitude(collision.impulse) > 0.05f)
            {
                GetComponent<AudioSource>().PlayOneShot(SwordMetal1, Mathf.Clamp(Vector3.Magnitude(collision.impulse) / 5, 0f, 1f));
            }
        }
        if (collision.collider.tag == "Shield")
        {
            //Debug.Log(Vector3.Magnitude(collision.impulse));
            if (Vector3.Magnitude(collision.impulse) > 0.15f)
            {
                GetComponent<AudioSource>().PlayOneShot(SwordShield1, Mathf.Clamp(Vector3.Magnitude(collision.impulse) / 5, 0f, 1f));
            }
        }
    }
}
