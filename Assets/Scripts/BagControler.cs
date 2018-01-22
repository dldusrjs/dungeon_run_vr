using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagControler : MonoBehaviour {
    /// <summary>
    /// ////////////////////////////////////////////////오브젝트 인벤토리에 저장 스크립트
    /// </summary>
    Inventory inventory;

    public delegate void OnItemChanged();
    public static OnItemChanged onItemChangedCallback;
    public AudioClip openBagClip;
    public AudioClip closeBagClip;

    public Transform itemsParents;

    InventorySlot[] slots;

    public GameObject bagCanvas;
    private bool isBagCalled;
    private bool isStoring = false;
    private AudioSource audioSource;

    private BoxCollider boxCollider;
    private Vector3 initialBoxSize = new Vector3(0.02f, 0.02f, 0.013f);
    private Vector3 initialBoxLocation = Vector3.zero;

    private Vector3 handEnterPosition;
    private Vector3 handSearchingVector;

    private Vector3 currentCanvasLocalPosition;
    private Quaternion currentCanvasLocalRotation;
    private Vector3 currentCanvasPosition;
    private Quaternion currentCanvasRotation;
    public GameObject currentParentOb;

    // Use this for initialization
    void Start () {
        inventory = Inventory.instance;
        onItemChangedCallback += UpdateUI;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = initialBoxSize;
        //Debug.Log(boxCollider.size); 000이 뜨는데 왜이러는건지?

        bagCanvas.SetActive(false);
        isBagCalled = false;

        slots = itemsParents.GetComponentsInChildren<InventorySlot>();
        audioSource = GetComponent<AudioSource>();

        currentCanvasLocalPosition = transform.localPosition;
        currentCanvasLocalRotation = transform.localRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {

            boxCollider.size = new Vector3(0.24f, 0.08f, 0.034f);
            boxCollider.center = new Vector3(-0.02f, 0, -0.01f);
            handEnterPosition = other.transform.position;
            CallCanvas();

            if (!isStoring && other.name == "IMGLeftHand")
            {
                Left_Hand_StoreItem();
            }

            if (!isStoring && other.name == "IMGRightHand")
            {
                Right_Hand_StoreItem();
            }
            StartCoroutine(storingCooldown());
        }
    }

    ////캔버스 닫기
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            //boxCollider.size = initialBoxSize;
            //boxCollider.center = initialBoxLocation;
            handEnterPosition = Vector3.zero;
            RemoveCanvas();
        }
    }

    public void CallCanvas()
    {
        bagCanvas.SetActive(true);

        
        currentCanvasPosition = transform.position;
        currentCanvasRotation = transform.rotation;

        //transform.parent = null;

        isBagCalled = true;
        audioSource.PlayOneShot(openBagClip, 0.5f);
        
    }

    public void RemoveCanvas()
    {
        bagCanvas.SetActive(false);

        //transform.parent = currentParentOb.transform;
        transform.localPosition = currentCanvasLocalPosition;
        transform.localRotation = currentCanvasLocalRotation;

        isBagCalled = false;
        audioSource.PlayOneShot(closeBagClip, 0.5f);
    }

    public void Left_Hand_StoreItem()
    {
        if (leftGrabbing.Left_isGrabbed && leftGrabbing.Left_grabbedObject.CompareTag("Pickable"))
        {
            if (leftGrabbing.Left_grabbedObject != null)
            {
                if (leftGrabbing.Left_grabbedObject.GetComponent<ItemStatus>().item != null)
                {
                    bool wasPickedUp = Inventory.instance.Add(leftGrabbing.Left_grabbedObject.GetComponent<ItemStatus>().item);

                    if (wasPickedUp)
                    {
                        Destroy(leftGrabbing.Left_grabbedObject);

                        if (onItemChangedCallback != null)
                        {
                            onItemChangedCallback.Invoke();
                        }

                        //StartCoroutine(storingCooldown()); // 콜라이더가 2번 호출되어 아이템이 2번 저장되는것을 막기 위함
                    }
                }
            }
        }
    }

    public void Right_Hand_StoreItem()
    {
        if (grabbing.rightIsGrabbed && grabbing.Right_grabbedObject.CompareTag("Pickable"))
        {
            if (grabbing.Right_grabbedObject != null)
            {
                if (grabbing.Right_grabbedObject.GetComponent<ItemStatus>().item != null)
                {
                    bool wasPickedUp = Inventory.instance.Add(grabbing.Right_grabbedObject.GetComponent<ItemStatus>().item);

                    if (wasPickedUp)
                    {
                        Destroy(grabbing.Right_grabbedObject);

                        if (onItemChangedCallback != null)
                        {
                            onItemChangedCallback.Invoke();
                        }

                        //StartCoroutine(storingCooldown()); // 콜라이더가 2번 호출되어 아이템이 2번 저장되는것을 막기 위함
                    }
                }
            }
        }
    }

    IEnumerator storingCooldown()
    {
        isStoring = true;
        yield return new WaitForSeconds(0.00001f); ///아무리 작아도 가방은 망가진다

        isStoring = false;
        
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].Additem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    void Update()
    {
        if(isBagCalled)
        {
            transform.rotation = currentCanvasRotation;  //////////////가방 터치시 캔버스 고정을 위한 기능
        }
    }
}
