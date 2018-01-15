using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFollowForce : MonoBehaviour {

    public GameObject originalHand;

    private Vector3 vector3HandToHand;
    private Vector3 imgHandVelocity;
    private Vector3 eulerAngleHandToHand;
    private Vector3 imgHandAngularVelocity;

    private static float Left_accelerationCount = 0;
    private float Left_accelerationConstant = 0.1f;

    public static float Left_control_K = 100.0f;
    public static float Left_control_K_Accel = 0f; // Acceleration 보너스 증가치를 의미
    public static float Left_control_C = 10.0f;
    public static float Left_angularMomentumInverse = 0.5f; 
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
        transform.rotation = Quaternion.Slerp(transform.rotation, originalHand.transform.rotation, Left_angularMomentumInverse);
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
        Left_angularMomentumInverse = 0.5f;
        //Debug.Log("left-idle");
    }

    void SwordGrabMotion()
    {
        Left_control_K = 30.0f;
        Left_control_C = 8.0f;
        Left_angularMomentumInverse = 0.1f;
        //Debug.Log("left-sword");
    }

    void ShieldGrabMotion()
    {
        Left_control_K = 15.0f;
        Left_control_C = 8.0f;
        Left_angularMomentumInverse = 0.01f;
    }

    void CollidingMotion()
    {
        Left_control_K = 0.4f;
        Left_control_K_Accel = 0f;
        Left_control_C = 5.0f;
        Left_angularMomentumInverse = 0.005f;
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
        if (originalHand.name == "LeftHandAnchor")
        {
            LeftHandExhaustValue = Vector3.Magnitude(transform.position - originalHand.transform.position);
            LeftHandExhaustValue = Mathf.Round(LeftHandExhaustValue * 1000f) / 1000f;
        }
    }
}
