using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftExhaustSystemUI : MonoBehaviour {
    public Text LeftExhaustText;
    [Header("Unity Stuff")]
    public Image LeftExhaustGaugeImage;
    public static float LeftHandEnergyValue;
    //public float ClampedLeftHandEnergy = Mathf.Clamp(LeftHandEnergyValue, 0f, 100f);
    private float LeftHandEnergyRegeneration;
    private float LeftRegenarationSpeed = 25f;
    private float LeftExhaustSpeed = 200f;
    private float ExhaustedDurationTime = 3f;
    private float lowerLimitValue = 0f;
    private bool isLeftHandRegen = true;
    private static bool isLeftExhaustedAtAll = false;

    // Use this for initialization
    void Start () {
        LeftHandEnergyValue = 100f;
        
    }
	
	// Update is called once per frame
	void Update () {

        LeftHandRegen();

        LeftExhaust();

        LeftExhaustText.text = "Exhaust: " + Mathf.Round(LeftHandEnergyValue).ToString() + " %";

        LeftExhaustGaugeImage.fillAmount = LeftHandEnergyValue / 100f;

    }

    void LeftHandRegen()
    {
        if (isLeftHandRegen && LeftHandEnergyValue < 100 && !isLeftExhaustedAtAll)
        {
            LeftHandEnergyValue += Time.deltaTime * LeftRegenarationSpeed;
        }
    }

    void LeftExhaust()
    {
        if (!isLeftExhaustedAtAll)
        {
            float exhaustDistance = LeftFollowForce.LeftHandExhaustValue; //가상손과 앵커와의 거리

            if (LeftHandEnergyValue > lowerLimitValue)
            {
                LeftHandEnergyValue -= Time.deltaTime * exhaustDistance * LeftExhaustSpeed;
            }
            else
            {
                StartCoroutine("Exhausted");
            }
        }
    }

    IEnumerator Exhausted()
    {
        LeftHandEnergyValue = 0f;
        isLeftExhaustedAtAll = true;
        lowerLimitValue = 50f;
        yield return new WaitForSeconds(5);
        isLeftExhaustedAtAll = false;
        lowerLimitValue = 0f;
        LeftHandEnergyValue = 100f;
    }
}
