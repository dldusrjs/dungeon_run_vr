using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<Item> items = new List<Item>();

    public static Inventory instance;

    public int spaceLimit = 10;

# region Singleton

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

# endregion

    public bool Add(Item item)
    {
        if (items.Count >= spaceLimit)
        {
            Debug.Log("No more space in inventory!");
            return false;
        }
        items.Add(item);
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
