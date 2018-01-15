using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {

    public Transform Bullet;
    private float TimeBetweenBullet = 4f;
    private float CurrentCountdown;
    private float BulletLife = 10f;
    private float BulletSpeed = 200f;
    private Transform bulletNow;



	// Use this for initialization
	void Start () {
        CurrentCountdown = TimeBetweenBullet;
	}
	
	// Update is called once per frame
	void Update () {

        if (CurrentCountdown <= 0f)
        {
            BulletFire();
        }
        CurrentCountdown -= Time.deltaTime;
    }

    void BulletFire() {
        CurrentCountdown = TimeBetweenBullet;
        bulletNow = Instantiate(Bullet, transform.position, transform.rotation);

        bulletNow.GetComponent<Rigidbody>().AddForce(-Vector3.forward * BulletSpeed);

        Destroy(bulletNow.gameObject, BulletLife);

    }

    /*IEnumerator BulletDestroy()
    {
        while (BulletLife >= 0f)
        {

            BulletLife -= 1f;
            if (BulletLife <= 0)
            {
                Destroy.bulletNow
            }

        }
        yield return new WaitForSeconds(1f);
    }
    */

}

