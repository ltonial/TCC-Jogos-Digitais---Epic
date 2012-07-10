using System.Collections;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Objeto transform do turret.
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// Energia do turret.
    /// </summary>
    private TurretHealth _health;
    /// <summary>
    /// Máquina de estados finita.
    /// </summary>
    private TurretStateMachine _fsm;
    /// <summary>
    /// The _smoke.
    /// </summary>
    private GameObject _smoke;
    #endregion
    #region Properties
    /// <summary>
    /// Recupera a máquina de estados.
    /// </summary>
    public TurretStateMachine FSM { get { return this._fsm; } }
    /// <summary>
    /// Recupera a energia.
    /// </summary>
    public TurretHealth Health { get { return this._health; } }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is playing walk sound.
    /// Utilizado para gerenciar o som do turret caminhando.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is playing walk sound; otherwise, <c>false</c>.
    /// </value>
    public bool IsPlayingWalkSound {
     get;
     set;
    }
    #endregion
    #region Methods (Inherit)
    /// <summary>
    /// Iniciando as variáveis no início do script.
    /// </summary>
    void Start()
    {
        this._myTransform = transform;
        this._fsm = new TurretStateMachine(this._myTransform);
        //this._health = new TurretHealth(this._myTransform.FindChild("HealthText").GetComponent<TextMesh>());
        this._health = new TurretHealth();

        this._smoke = (GameObject)Resources.Load("Items/Smoke");
    }
    /// <summary>
    /// Atualizando o turret.
    /// </summary>
    void Update()
    {
        if (!MenuPause.Paused)
        {
            if (this._health != null && this._fsm != null)
            {
                this._health.Update(this._fsm.CurrentState);
                if (this._health.IsDead)
                {
                    this._fsm.FreeComputerSpawn();

                    //Respawn
                    GameObject.Instantiate(this._smoke, this._myTransform.position, Quaternion.identity);

                    this._fsm = null;
                    Destroy(gameObject, 15f);

                    animation.wrapMode = WrapMode.ClampForever;
                    animation.Play("TurretDeath");
                }
                if (this._fsm != null) this._fsm.Update(Time.deltaTime);
            }
        }
    }
    /// <summary>
    /// Desenha Gizmos em volta do Turret.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TurretStateMachine.DISTANCELIMITTOCHASE);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TurretStateMachine.DISTANCELIMITTOFIGHT);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, TurretStateMachine.DISTANCELIMITTOEVADE);
    }
    #endregion
    #region Coroutines
    public IEnumerator TimeForWalk () {
        yield return new WaitForSeconds(0.5f);
        this.IsPlayingWalkSound = false;
    }
    #endregion
}