using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bladeCutting : MonoBehaviour {

    public Material capMaterial;
    public bool isTargetLockon = false;
    public static bool isCutThrough = false;
    private GameObject victim;
    public float CutCooldown = 5f;

	// Update is called once per frame
	void Update () {
        if (isTargetLockon && isCutThrough)
        {
            TargetCut();
            isCutThrough = false;
            bladeColliding.isCutting = false;
        }

        //iscutthrough가 true로 고정되는 치명적 오류 수정을 위한 항...
        if (!followForce.isCollidingObstacle && isCutThrough)
        {
            isCutThrough = false;
        }
	}


    private void OnTriggerExit(Collider other)    // Targetlockon함수는 이것으로 대체되었다. 왜인지몰라도 Exit호출일때 더 잘된다.
    {
        if (!isTargetLockon && bladeColliding.isCutting)
        {
            if (bladeColliding.isCutting)
            {
                //victim = other.GetComponent<GameObject>();
                victim = other.gameObject;
                if (victim != null) isTargetLockon = true;

                Debug.Log(victim);
            }
        }
    }

    private void TargetCut()
    {
        //앵커포지션과 노멀디렉션 생성
        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, -transform.forward, capMaterial);
        //Destroy(pieces[1], 1); 자른걸 파괴하고 싶다면 이걸 활성화
        //Destroy(pieces[0], 1); 자른걸 파괴하고 싶다면 이걸 활성화
        isTargetLockon = false;
        isCutThrough = false;
        victim = null;
    }


    public static void SureCutThrough()
    {
        if (!isCutThrough)
        {
            isCutThrough = true;
        }
    }

}
