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
		Debug.Log(hit.transform.tag);
        if (hit.transform.tag == "Player")
        {
            hit.transform.GetComponent<PlayerManager>().Tank.ItemEnergy = QUANTITY;
            Destroy(gameObject);
        }
    }
	#endregion
}

