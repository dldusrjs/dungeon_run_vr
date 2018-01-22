using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour {

    [Header("Unity Stuff")]
    public Image playerHealthValue;

	// Update is called once per frame
	void Update () {
        playerHealthValue.fillAmount = PlayerStatusInstance.playerHealth / 100f;
    }
}
