using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFollowForce : MonoBehaviour {

    public GameObject originalHand;

    private Vector3 vector3HandToHand;
    private Vector3 vector3AngularHandToHand;
    private Vector3 imgHandVelocity;
    private Vector3 eulerAngleHandToHand;
    private Vector3 imgHandAngularVelocity;

    private static float Left_accelerationCount = 0;
    private float Left_accelerationConstant = 0.1f;

    public static float Left_control_K = 100.0f;
    public static float Left_control_K_Accel = 0f; // Acceleration 보너스 증가치를 의미
    public static float Left_control_C = 10.0f;
    public static float Left_controlRotation_K = 20f;
    public static float Left_controlRotation_C = 5f;

    public static float LeftHandExhaustValue;

    public static bool Left_isCollidingObstacle = false;

    public static int Left_currentHandState = 0;
    //0: 손에 아무것도 쥐지 않은 평범한 상태
    //1: 손에 칼
    //2: 손에 방패
    //3: 충돌

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        Left_DefaultMotion();
        Left_MotionSwitching();
        Left_Acceleration();

        LeftHandExhaust();
    }

    void Left_DefaultMotion()
    {
        vector3HandToHand = originalHand.transform.position - transform.position;
        eulerAngleHandToHand = originalHand.transform.localEulerAngles - transform.localEulerAngles;
        imgHandVelocity = GetComponent<Rigidbody>().velocity;
        imgHandAngularVelocity = GetComponent<Rigidbody>().angularVelocity;
        GetComponent<Rigidbody>().AddForce(vector3HandToHand * (Left_control_K + Left_control_K_Accel) - imgHandVelocity * Left_control_C);

        Vector3 x = Vector3.Cross(originalHand.transform.forward.normalized, -transform.forward.normalized);
        Vector3 y = Vector3.Cross(originalHand.transform.right.normalized, -transform.right.normalized);

        float theta_x = Mathf.Asin(x.magnitude);
        float theta_y = Mathf.Asin(y.magnitude);

        Vector3 w = x.normalized * theta_x + y.normalized * theta_y; /// Time.fixedDeltaTime;

        //Quaternion q = transform.rotation * GetComponent<Rigidbody>().inertiaTensorRotation;
        //vector3AngularHandToHand = q * Vector3.Scale(GetComponent<Rigidbody>().inertiaTensor, (Quaternion.Inverse(q) * w));
        GetComponent<Rigidbody>().AddTorque(w * Left_controlRotation_K - imgHandAngularVelocity * Left_controlRotation_C);
    }

    void Left_MotionSwitching()
    {
        switch (Left_currentHandState)
        {
            case 0: //아무것도 쥐지 않기
                IdleMotion();
                break;
            case 1: // 칼 잡기
                SwordGrabMotion();
                break;
            case 2: // 방패 잡기
                ShieldGrabMotion();
                break;
            case 3: // 칼로 오브젝트 진입
                CollidingMotion();
                break;
            default:
                IdleMotion();
                break;
        }
    }

    void IdleMotion()
    {
        Left_control_K = 100.0f;
        Left_control_C = 10.0f;
        //Debug.Log("left-idle");
    }

    void SwordGrabMotion()
    {
        Left_control_K = 100.0f;
        Left_control_C = 8.0f;
        //Debug.Log("left-sword");
    }

    void ShieldGrabMotion()
    {
        Left_control_K = 100.0f;
        Left_control_C = 8.0f;
    }

    void CollidingMotion()
    {
        Left_control_K = 0.4f;
        Left_control_K_Accel = 0f;
        Left_control_C = 5.0f;
        //Debug.Log("left-Colliding");
    }

    void Left_Acceleration()
    {
        if (leftGrabbing.L_ismoving && !(Left_currentHandState == 3))
        {
            Left_accelerationCount++;
        }
        if (!leftGrabbing.L_ismoving)
        {
            Left_accelerationCount = 0f;
        }
        Left_control_K_Accel = Left_accelerationConstant * Left_accelerationCount;
    }
    /// <summary>
    ///  Exhaust장치
    /// </summary>
    void LeftHandExhaust()
    {
        if (originalHand.name == "LeftHandExact")
        {
            LeftHandExhaustValue = Vector3.Magnitude(transform.position - originalHand.transform.position);
            LeftHandExhaustValue = Mathf.Round(LeftHandExhaustValue * 1000f) / 1000f;
        }
    }
}
