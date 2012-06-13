using UnityEngine;
using System.Collections;

public class ItemIdentificatorManager : MonoBehaviour
{
    void Awake()
    {
		string tagName = LayerMask.LayerToName(gameObject.layer);
		
        GameObject[] auxObjects = GameObject.FindGameObjectsWithTag(tagName);
        int auxId = 0;
        foreach (GameObject item in auxObjects)
        {
            item.AddComponent(typeof(ItemIdentificator));
            item.GetComponent<ItemIdentificator>().ID = ++auxId;
        }
    }
}
