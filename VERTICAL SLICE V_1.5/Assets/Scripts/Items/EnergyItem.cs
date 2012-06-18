using UnityEngine;
using System.Collections;

public class EnergyItem : MonoBehaviour
{
    #region Constants
    /// <summary>
    /// The _quantity.
    /// </summary>
    private const int QUANTITY = 30;
    #endregion
    #region Methods
    void Start()
    {
        Debug.Log("SPAWN ENERGY");
    }
    void OnTriggerEnter(Collider hit)
    {
        Debug.Log("ITEM: " + hit.tag);
        if (hit.tag == "Player")
        {
            hit.gameObject.GetComponent<PlayerManager>().Tank.ItemEnergy = QUANTITY;
            Destroy(gameObject);
        }
    }
    #endregion
}

