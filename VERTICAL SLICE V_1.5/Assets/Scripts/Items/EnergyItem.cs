using UnityEngine;
using System.Collections;

public class EnergyItem : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// The _is ground.
    /// </summary>
    private bool _isOnGround;
    #endregion
    #region Constants
    /// <summary>
    /// The _quantity.
    /// </summary>
    private const int QUANTITY = 30;
    #endregion
    #region Methods
    void Start()
    {
        this._isOnGround = false;
    }
    void Update()
    {
        if(!this._isOnGround) {
            this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-(1.5f*Time.deltaTime),this.transform.position.z);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Level")
        {
            this._isOnGround = true;
        }
        if (hit.tag == "Player")
        {
            hit.gameObject.GetComponent<PlayerManager>().Tank.ItemEnergy = QUANTITY;
            Destroy(gameObject);
        }
    }
    #endregion
}

