using UnityEngine.UI;
using UnityEngine;

public class BagUI : MonoBehaviour {

    public GameObject bagCanvas;
    private bool isBagCalled;

    private void Start()
    {
        bagCanvas.SetActive(false);
        isBagCalled = false;
    }

    public void CallCanvas()
    {
        bagCanvas.SetActive(true);
        isBagCalled = true;
    }

    public void RemoveCanvas()
    {
        bagCanvas.SetActive(false);
        isBagCalled = false;
    }
}
