using UnityEngine;
using System.Collections;

/// <summary>
/// Bullet script.
/// Adicionado ao prefab da bala
/// </summary>
public class BulletArrow : MonoBehaviour
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
    /// <summary>
    /// The _decal.
    /// </summary>
    private GameObject _decal;
    /// <summary>
    /// The _sparks.
    /// </summary>
    private ParticleEmitter _sparks;
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
        this._decal = (GameObject)Resources.Load ("Shots/Decal");
        this._cameraBullet = transform.FindChild("CameraBullet").gameObject;
        this._cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
        this._menuPause = this._cameraMain.GetComponent<MenuPause>();
        this._sparks = GetComponentInChildren<ParticleEmitter>();
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

            if (Physics.Raycast(this.transform.position, this.transform.forward, out this._hit, 0.3f))
            {
                if (this._hit.transform.tag == "Turret" && this._hit.transform.GetComponent<TurretManager>() != null)
                    this._hit.transform.GetComponent<TurretManager>().Health.UpdateHealth(-this._damage);
                if (this._hit.transform.tag == "Humanoid" && this._hit.transform.GetComponent<HumanoidManager>() != null)
                    this._hit.transform.GetComponent<HumanoidManager>().Health.UpdateHealth(-this._damage);

                if(this._hit.transform.tag!="Player" && this._hit.transform.tag!="Decal") {
                    GameObject decal = (GameObject)UnityEngine.GameObject.Instantiate(this._decal, this._hit.point, Quaternion.FromToRotation(Vector3.up, this._hit.normal));
                    decal.transform.parent = this._hit.transform;
                    this._sparks.transform.parent = this._hit.transform;
                    this._sparks.transform.position = this._hit.point;
                    this._sparks.transform.rotation = Quaternion.FromToRotation(Vector3.up, this._hit.normal);
                    this._sparks.Emit();
                    Destroy(gameObject);
                }
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
        Time.timeScale = 0.15f;
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