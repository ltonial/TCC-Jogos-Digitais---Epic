using UnityEngine;
using System.Collections;

/// <summary>
/// Bullet script.
/// Adicionado ao prefab da bala
/// </summary>
public class BulletScript : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// The _damage. Esta setando aqui pois preciso saber quanto de dano tira antes do Start.
    /// </summary>
    private int _damage = 1;
    /// <summary>
    /// The _velocity.
    /// </summary>
    private float _velocity;
    /// <summary>
    /// Tempo de vida da bala
    /// </summary>
    private float _timeToDestroy;
    /// <summary>
    /// The _hit.
    /// </summary>
    private RaycastHit _hit;
    /// <summary>
    /// The _player.
    /// </summary>
    private GameObject _player;
    /// <summary>
    /// No start seta o script do GameObject camera.
    /// </summary>
    private GameObject _cameraMain;
	/// <summary>
	/// The _menu pause.
	/// </summary>
    private MenuPause _menuPause;
    /// <summary>
    /// Camera do bullet time
    /// </summary>
    private GameObject _cameraBullet;
    #endregion
    #region Properties
    public int Damage
    {
        get { return this._damage; }
    }
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        this._cameraBullet = transform.FindChild("CameraBullet").gameObject;
        this._cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
        this._menuPause = this._cameraMain.GetComponent<MenuPause>();

        this._player = GameObject.FindGameObjectWithTag("Player");
        this._velocity = 15f;
        this._timeToDestroy = 15f;

        StartCoroutine(this.DestroyByTime());
    }
    void Update()
    {
        if (!MenuPause.Paused)
        {
            if (WeaponPlayer.BulletTimeActived)
                this.ActiveBulletTime();

            if (Physics.Raycast(this.transform.position, this.transform.forward, out this._hit, 1f))
            {
                if (this._hit.transform.tag == "Turret")
                    this._hit.transform.GetComponent<TurretManager>().Health.UpdateHealth(-this._damage);
				else if (this._hit.transform.tag == "HumanoidCollider")
					this._hit.transform.parent.GetComponent<HumanoidManager>().Health.UpdateHealth(-this._damage);

                Destroy(gameObject);
            }
            transform.Translate(Vector3.forward * this._velocity * Time.deltaTime);
        }
    }
    #endregion
    #region Methods (Class)
    public void ActiveBulletTime()
    {
        this._player.GetComponent<PlayerMovement>().enabled = false;
        this._player.GetComponent<PlayerInput>().enabled = false;
        this._player.GetComponent<AimScript>().enabled = false;
        this._player.GetComponent<PlayerManager>().enabled = false;
        this._player.GetComponent<CharacterController>().enabled = false;
        this._cameraMain.camera.enabled = false;
        this._cameraBullet.camera.enabled = true;
        Time.timeScale = 0.3f;
    }
    public void DesactiveBulletTime()
    {
        WeaponPlayer.BulletTimeActived = false;
        this._cameraBullet.camera.enabled = false;
        this._cameraMain.camera.enabled = true;
        this._player.GetComponent<CharacterController>().enabled = true;
        this._player.GetComponent<PlayerMovement>().enabled = true;
        this._player.GetComponent<PlayerInput>().enabled = true;
        this._player.GetComponent<AimScript>().enabled = true;
        this._player.GetComponent<PlayerManager>().enabled = true;
        Time.timeScale = 1.0f;
    }
    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        this.DesactiveBulletTime();
    }
    #endregion
}