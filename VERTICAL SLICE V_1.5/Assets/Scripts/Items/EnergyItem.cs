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
    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Player")
        {
            hit.gameObject.GetComponent<PlayerManager>().Tank.ItemEnergy = QUANTITY;
            Destroy(gameObject);
        }
    }
    #endregion
}

