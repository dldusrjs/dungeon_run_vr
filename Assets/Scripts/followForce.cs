using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followForce : MonoBehaviour {

    public GameObject originalHand;
    public GameObject CenterOfBody;

    private Vector3 vector3HandToHand;
    private Vector3 vector3AngularHandToHand;
    private Vector3 imgHandVelocity;
    private Vector3 eulerAngleHandToHand;
    private Vector3 imgHandAngularVelocity;

    private static float accelerationCount = 0;
    private float accelerationConstant = 0.1f;

    public static float control_K = 100.0f;
    public static float control_K_Accel = 0f; // Acceleration 보너스 증가치를 의미
    public static float control_C = 10.0f;
    public static float controlRotation_K = 20f;
    public static float controlRotation_C = 5f;

    public static float angularMomentumInverse = 0.5f; 
    public static float RightHandExhaustValue;

    public static bool isCollidingObstacle = false;

    public static int Right_currentHandState = 0;
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
        DefaultMotion();
        MotionSwitching();
        Acceleration();

        RightHandExhaust();
    }

    void DefaultMotion()
    {
        vector3HandToHand = originalHand.transform.position - transform.position;
        eulerAngleHandToHand = originalHand.transform.localEulerAngles - transform.localEulerAngles;
        imgHandVelocity = GetComponent<Rigidbody>().velocity;
        imgHandAngularVelocity = GetComponent<Rigidbody>().angularVelocity;
        GetComponent<Rigidbody>().AddForce(vector3HandToHand * (control_K + control_K_Accel) - imgHandVelocity * control_C);

        Vector3 x = Vector3.Cross(originalHand.transform.forward.normalized, -transform.forward.normalized);
        Vector3 y = Vector3.Cross(originalHand.transform.right.normalized, -transform.right.normalized);

        float theta_x = Mathf.Asin(x.magnitude);
        float theta_y = Mathf.Asin(y.magnitude);

        Vector3 w = x.normalized * theta_x + y.normalized * theta_y; /// Time.fixedDeltaTime;

        //Quaternion q = transform.rotation * GetComponent<Rigidbody>().inertiaTensorRotation;
        //vector3AngularHandToHand = q * Vector3.Scale(GetComponent<Rigidbody>().inertiaTensor, (Quaternion.Inverse(q) * w));
        GetComponent<Rigidbody>().AddTorque(w * controlRotation_K - imgHandAngularVelocity * controlRotation_C);
    }

    void MotionSwitching()
    {
        switch (Right_currentHandState)
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
        control_K = 100.0f;
        control_C = 10.0f;
        angularMomentumInverse = 0.5f;
        //Debug.Log("right-idle");
    }

    void SwordGrabMotion()
    {
        control_K = 100.0f;
        control_C = 8.0f;
        angularMomentumInverse = 0.1f;
        //Debug.Log("right-sword");
    }

    void ShieldGrabMotion()
    {
        control_K = 15.0f;
        control_C = 8.0f;
        angularMomentumInverse = 0.01f;
    }

    void CollidingMotion()
    {
        control_K = 0.4f;
        control_K_Accel = 0f;
        control_C = 5.0f;
        angularMomentumInverse = 0.005f;
        //Debug.Log("right-Colliding");
    }

    void Acceleration()
    {
        if (grabbing.rightIsmoving && !(Right_currentHandState == 3))
        {
            accelerationCount++;
        }
        if (!grabbing.rightIsmoving)
        {
            accelerationCount = 0f;
        }
        control_K_Accel = accelerationConstant * accelerationCount;
    }
    /// <summary>
    ///  Exhaust장치
    /// </summary>
    void RightHandExhaust()
    {
        if (originalHand.name == "RightHandAnchor")
        {
            RightHandExhaustValue = Vector3.Magnitude(transform.position - originalHand.transform.position);
            RightHandExhaustValue = Mathf.Round(RightHandExhaustValue * 1000f) / 1000f;
        }
    }
}
