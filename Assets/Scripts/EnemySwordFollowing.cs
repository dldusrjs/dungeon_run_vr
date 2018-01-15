using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordFollowing : MonoBehaviour {

    public GameObject originalHand;
    public GameObject CenterOfBody;

    private Vector3 vector3HandToHand;
    private Vector3 vector3AngularHandToHand;
    private Vector3 imgHandVelocity;
    private Vector3 eulerAngleHandToHand;
    private Vector3 imgHandAngularVelocity;

    private float control_K = 40.0f;
    private float control_C = 2.0f;
    private float controlRotation_K = 30.0f;
    private float controlRotation_C = 3f;
    private float HandExhaustValue;
    private float HandEnergyValue;
    private float HandAngularExhaustValue;

    private float RegenarationSpeed = 40f;
    private float lowerLimitValue = 0f;
    private float ExhaustSpeed = 120f;
    private float AngularExhaustSpeed = 0.4f;

    private bool isHandExhaustedAtAll = false;


    // Use this for initialization
    void Start () {
        HandEnergyValue = 100f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        DefaultMotion();
        HandExhaustCalcul();
        HandRegen();
        Exhaust();

        if (HandEnergyValue < 5)
        {
            StartCoroutine("Exhausted");
        }
        Debug.Log(HandEnergyValue);
    }

    void DefaultMotion()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            vector3HandToHand = originalHand.transform.position - transform.position;
            eulerAngleHandToHand = originalHand.transform.localEulerAngles - transform.localEulerAngles;
            imgHandVelocity = GetComponent<Rigidbody>().velocity;
            imgHandAngularVelocity = GetComponent<Rigidbody>().angularVelocity;
            GetComponent<Rigidbody>().AddForce(vector3HandToHand * control_K - imgHandVelocity * control_C);

            Vector3 x = Vector3.Cross(originalHand.transform.forward.normalized, -transform.forward.normalized);
            Vector3 y = Vector3.Cross(originalHand.transform.right.normalized, -transform.right.normalized);

            float theta_x = Mathf.Asin(x.magnitude);
            float theta_y = Mathf.Asin(y.magnitude);

            Vector3 w = x.normalized * theta_x + y.normalized * theta_y; /// Time.fixedDeltaTime;

            //Quaternion q = transform.rotation * GetComponent<Rigidbody>().inertiaTensorRotation;
            //vector3AngularHandToHand = q * Vector3.Scale(GetComponent<Rigidbody>().inertiaTensor, (Quaternion.Inverse(q) * w));
            GetComponent<Rigidbody>().AddTorque(w * controlRotation_K - imgHandAngularVelocity * controlRotation_C);
        }
    }

    void HandExhaustCalcul()
    {
        HandExhaustValue = Vector3.Magnitude(transform.position - originalHand.transform.position);
        HandExhaustValue = Mathf.Round(HandExhaustValue * 1000f) / 1000f;
        HandAngularExhaustValue = Quaternion.Angle(transform.rotation, originalHand.transform.rotation);

    }

    void ExhaustBreaking()
    {
        control_K = 0f;
        control_C = 0f;
        controlRotation_K = 0f;
        controlRotation_C = 0f;
        //GetComponent<EnemySwordFollowing>().enabled = false;
    }

    void ExhaustRecover()
    {
        control_K = 30.0f;
        control_C = 2.0f;
        controlRotation_K = 20.0f;
        controlRotation_C = 1f;
    }

    void HandRegen()
    {
        if (HandEnergyValue < 100 && !isHandExhaustedAtAll)
        {
            HandEnergyValue += Time.deltaTime * RegenarationSpeed;
        }
    }

    void Exhaust()
    {
        if (!isHandExhaustedAtAll)
        {
            float exhaustDistance = HandExhaustValue; //가상손과 앵커와의 거리

            if (HandEnergyValue > lowerLimitValue)
            {
                HandEnergyValue -= Time.deltaTime * exhaustDistance * ExhaustSpeed;
                HandEnergyValue -= Time.deltaTime * HandAngularExhaustValue * AngularExhaustSpeed;
            }
            else
            {
                StartCoroutine("Exhausted");
            }
        }
    }

    IEnumerator Exhausted()
    {
        HandEnergyValue = 0f;
        isHandExhaustedAtAll = true;
        lowerLimitValue = 50f;
        ExhaustBreaking();
        GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitForSeconds(5);
        isHandExhaustedAtAll = false;
        lowerLimitValue = 0f;
        ExhaustRecover();
        HandEnergyValue = 120f;
        GetComponent<Rigidbody>().useGravity = false;

    }

}
