using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatus : MonoBehaviour {

    public Item item;

    void Start()
    {
        //item.gameObject = this.gameObject; //gameobject타입을 item정보에 추가하여 나중에 인벤토리에서 꺼낼 수 있게 함(프리팹에서 등록해놓도록바꿈)->등록안해도작동함.왜..?

        if (item.gameObject.GetComponent<AudioSource>() == null) ////////////오디오 소스 미리 추가해놓기
        {
            item.gameObject.AddComponent<AudioSource>();
        }
    }
}
