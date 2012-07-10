using UnityEngine;
using System.Collections;

public class TurretGun : MonoBehaviour {

	#region Constants
    private const int MAXSHOTQUANTITY = 5;
    private const float TIMETORELOAD_CLIP = 3f;
    private const float TIMETORELOAD_SHOT = 1f;
    #endregion
    #region Attributes
    /// <summary>
    /// Arma.
    /// </summary>
    private GameObject _gunObject;
    /// <summary>
    /// Ponteiro da arma.
    /// </summary>
    private Transform _gunPointerTransform;
    /// <summary>
    /// Pode atirar?
    /// </summary>
    private bool _canShot;
    /// <summary>
    /// Quantidade de munições.
    /// </summary>
    private int _shotsQuantity;
    /// <summary>
    /// Tempo p/ recarregar.
    /// </summary>
    private float _timeToReloadClip;
    /// <summary>
    /// Prefab da munição.
    /// </summary>
    private GameObject _bulletGunObject;
    /// <summary>
    /// Pronto p/ atirar.
    /// </summary>
    private bool _isReadyToShot;
    /// <summary>
    /// Tempo p/ atirar.
    /// </summary>
    private float _timeToReloadShot;
    #endregion
    #region Constructors
    public TurretGun(GameObject pGunTransform)
    {
        this._shotsQuantity = MAXSHOTQUANTITY;
        this._timeToReloadClip = TIMETORELOAD_CLIP;
        this._timeToReloadShot = TIMETORELOAD_SHOT;
        this._canShot = true;
        this._isReadyToShot = true;

        this._gunObject = pGunTransform;
        this._gunPointerTransform = this._gunObject.transform.FindChild("GunPointer");
        this._bulletGunObject = (GameObject)Resources.Load("Shots/BulletGun");
    }
    #endregion
    #region Methods
    public void UpdateShot(float pDeltaTime)
    {
        if (this._canShot)
        {
            if (this._isReadyToShot)
            {
                Debug.Log("TIRO TURRET!");
                GameObject bullet = (GameObject)GameObject.Instantiate(this._bulletGunObject, this._gunPointerTransform.position, this._gunPointerTransform.rotation);
                this._shotsQuantity--;
                this._canShot = this._shotsQuantity > 0;
                this._isReadyToShot = false;
            }
            else
            {
                this._timeToReloadShot -= pDeltaTime;
                if (this._timeToReloadShot <= 0)
                {
                    this._isReadyToShot = true;
                    this._timeToReloadShot = TIMETORELOAD_SHOT;
                }
            }
        }
        else
        {
            this._timeToReloadClip -= pDeltaTime;
            if (this._timeToReloadClip <= 0)
            {
                this._canShot = true;
                this._shotsQuantity = MAXSHOTQUANTITY;
                this._timeToReloadClip = TIMETORELOAD_CLIP;
            }
        }
    }
    #endregion

}
