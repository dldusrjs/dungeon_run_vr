using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightExhaustSystemUI : MonoBehaviour {
    public Text RightExhaustText;
    [Header("Unity Stuff")]
    public Image RightExhaustGaugeImage;
    public static float RightHandEnergyValue;
    private float RightHandEnergyRegeneration;
    private float RightRegenarationSpeed = 25f;
    private float RightExhaustSpeed = 200f;
    private float ExhaustedDurationTime = 3f;
    private float lowerLimitValue = 0f;
    private bool isRightHandRegen = true;
    private static bool isRightExhaustedAtAll = false;

    // Use this for initialization
    void Start()
    {
        RightHandEnergyValue = 100f;

    }

    // Update is called once per frame
    void Update()
    {

        RightHandRegen();

        RightExhaust();

        RightExhaustText.text = "Exhaust: " + Mathf.Round(RightHandEnergyValue).ToString() + " %";

        RightExhaustGaugeImage.fillAmount = RightHandEnergyValue / 100f;

    }

    void RightHandRegen()
    {
        if (isRightHandRegen && RightHandEnergyValue < 100 && !isRightExhaustedAtAll)
        {
            RightHandEnergyValue += Time.deltaTime * RightRegenarationSpeed;
        }
    }

    void RightExhaust()
    {
        if (!isRightExhaustedAtAll)
        {
            float exhaustDistance = followForce.RightHandExhaustValue; //가상손과 앵커와의 거리

            if (RightHandEnergyValue > lowerLimitValue)
            {
                RightHandEnergyValue -= Time.deltaTime * exhaustDistance * RightExhaustSpeed;
            }
            else
            {
                StartCoroutine("Exhausted");
            }
        }
    }

    IEnumerator Exhausted()
    {
        RightHandEnergyValue = 0f;
        isRightExhaustedAtAll = true;
        lowerLimitValue = 50f;
        yield return new WaitForSeconds(5);
        isRightExhaustedAtAll = false;
        lowerLimitValue = 0f;
        RightHandEnergyValue = 100f;
    }
}
