using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour {
    /// <summary>
    /// ////////////////////////////////////////오브젝트의 슬롯에 저장된 아이템을 꺼내는 스크립트
    /// </summary>
    public Image icon;
    public Inventory inventory;
    public string rightGrabButtonName;
    public string leftGrabButtonName;
    Item item;

    void Start()
    {
        inventory = Inventory.instance;
    }

    public void Additem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (other.name == "IMGLeftHand")
            {
                LeftGetItem();
            }

            if (other.name == "IMGRightHand")
            {
                RightGetItem();
            }

            //////////////////////////왼손전용
        }
    }

    void LeftGetItem()
    {
        if (!leftGrabbing.Left_isGrabbed && Input.GetAxis(leftGrabButtonName) == 1) ///////////그랩해서 꺼내기
        {
            if (item != null)
            {
                if (item.gameObject != null)
                {

                    //GameObject instantiatedItem = Instantiate(item.gameObject, Vector3.zero, Quaternion.identity, FindObjectOfType<leftGrabbing>().transform);
                    GameObject instantiatedItem = Instantiate(item.gameObject);

                    leftGrabbing.Left_grabbedObject = instantiatedItem;
                    leftGrabbing.Left_isGrabbed = true;
                    leftGrabbing.Left_grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    leftGrabbing.Left_grabbedObject.transform.position = FindObjectOfType<leftGrabbing>().transform.position;
                    leftGrabbing.Left_grabbedObject.transform.parent = FindObjectOfType<leftGrabbing>().transform;


                    /*
                    leftGrabbing.Left_grabbedObject = item.gameObject;
                    leftGrabbing.Left_isGrabbed = true;
                    leftGrabbing.Left_grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    leftGrabbing.Left_grabbedObject.transform.position = FindObjectOfType<leftGrabbing>().transform.position;
                    leftGrabbing.Left_grabbedObject.transform.parent = FindObjectOfType<leftGrabbing>().transform;
                    */

                    instantiatedItem.GetComponent<AudioSource>().PlayOneShot(item.pickUpClip); ////////////꺼낼때 소리

                    inventory.Remove(item);

                    if (BagControler.onItemChangedCallback != null)
                    {
                        BagControler.onItemChangedCallback.Invoke();///////////UI업데이트 콜백
                    }
                }
            }
        }
    }

    void RightGetItem()
    {
        if (!grabbing.rightIsGrabbed && Input.GetAxis(rightGrabButtonName) == 1)
        {
            if (item != null)
            {
                if (item.gameObject != null)
                {
                    /*
                    item.gameObject.SetActive(true);
                    grabbing.Right_grabbedObject = item.gameObject;

                    grabbing.rightIsGrabbed = true;
                    grabbing.Right_grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbing.Right_grabbedObject.transform.position = FindObjectOfType<grabbing>().transform.position;
                    grabbing.Right_grabbedObject.transform.parent = FindObjectOfType<grabbing>().transform;

                    item.gameObject.GetComponent<AudioSource>().PlayOneShot(item.pickUpClip);
                    */

                    GameObject instantiatedItem = Instantiate(item.gameObject);

                    grabbing.Right_grabbedObject = instantiatedItem;
                    grabbing.rightIsGrabbed = true;
                    grabbing.Right_grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbing.Right_grabbedObject.transform.position = FindObjectOfType<grabbing>().transform.position;
                    grabbing.Right_grabbedObject.transform.parent = FindObjectOfType<grabbing>().transform;

                    instantiatedItem.GetComponent<AudioSource>().PlayOneShot(item.pickUpClip); ////////////꺼낼때 소리

                    inventory.Remove(item);

                    if (BagControler.onItemChangedCallback != null)
                    {
                        BagControler.onItemChangedCallback.Invoke();
                    }
                }
            }
        }
    }
}
