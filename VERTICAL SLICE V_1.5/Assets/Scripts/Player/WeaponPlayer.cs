using UnityEngine;
using System.Collections;

public class WeaponPlayer
{
    #region Attributes
    /// <summary>
    /// Tempo de recarrega da arma
    /// </summary>
    private float _reloadTimeBullet;
    /// <summary>
    /// Tempo entre disparos
    /// </summary>
    private float _reloadTimeShot;
    /// <summary>
    /// Mostra se disparou ou não, usado para controlar tempo entre disparos
    /// </summary>
    private bool _isShot;
    /// <summary>
    /// Game Object da bala, precisa estar dentro da pasta Resources/prefabs
    /// </summary>
    private GameObject _arrowGo;
    /// <summary>
    /// Transform da arma que está no player
    /// </summary>
    private GameObject _weaponGo;
    /// <summary>
    /// The bullet time actived.
    /// </summary>
    public static bool BulletTimeActived;
    /// <summary>
    /// % de chance para disparar o bullet time
    /// </summary>
    private int _percentBulletTime;
    #endregion
    #region Properties
    public float ReloadTimeBullet
    {
        get { return this._reloadTimeBullet; }
    }
    public bool Reloading
    {
        get;
        set;
    }
    public float ReloadTimeShot
    {
        get { return this._reloadTimeShot; }
    }
    public bool IsShot { get { return this._isShot; } }
    /// <summary>
   /// Gets or sets the tank.
   /// O tank é setado pelo PlayerManager (referencia da instancia no PlayerManager)
   /// </summary>
   /// <value>
   /// The tank.
   /// </value>
    public TankPlayer Tank {
        get;
        set;
    }
    #endregion
    #region Methods
    public void Start()
    {
        this._arrowGo = (GameObject)Resources.Load("Shots/Arrow");
        this._weaponGo = GameObject.FindGameObjectWithTag("ArrowPointer");
        this._reloadTimeShot = .3f;
        this._reloadTimeBullet = 3f;
        this.Reloading = false;
        this._percentBulletTime = 30;
    }
    /// <summary>
    /// Instancia tiros
    /// Faz um RayCast do centro da camera, dispara o tiro da ponta da arma rotacionado para o centro da camera,
    /// em seguida faz um LookAt para o ponto da colisao no momento do click.
    /// </summary>
    public void Shot()
    {
        if (!this._isShot && this.Tank.EnergyShot > 0)
        {
            this._isShot = true;
            this.Tank.EnergyShot--;
            RaycastHit hit;
            GameObject bullet = (GameObject)GameObject.Instantiate(this._arrowGo, this._weaponGo.transform.position, Camera.main.transform.rotation);

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f))
            {
                bullet.transform.LookAt(hit.point);
                if (hit.transform.tag == "Turret")
                {
                    int testHealth = hit.transform.GetComponent<TurretManager>().Health.CurrentHealth - this._arrowGo.GetComponent<BulletScript>().Damage;
                    if (testHealth <= 0)
                    {
                        int random = Random.Range(0, 100);
                        if (random <= this._percentBulletTime)
                        {
                            bullet.transform.LookAt(hit.transform);
                            WeaponPlayer.BulletTimeActived = true;
                        }
                    }
                }
            }
            this._isShot = false;
        }
    }
    #endregion
}