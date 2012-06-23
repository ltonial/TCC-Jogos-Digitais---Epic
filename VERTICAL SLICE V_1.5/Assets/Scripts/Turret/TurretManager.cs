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
    #endregion
    #region Methods (Inherit)
    /// <summary>
    /// Iniciando as variáveis no início do script.
    /// </summary>
    void Start()
    {
        this._myTransform = transform;
        this._fsm = new TurretStateMachine(this._myTransform);
        this._health = new TurretHealth(this._myTransform.FindChild("HealthText").GetComponent<TextMesh>());
    }
    /// <summary>
    /// Atualizando o turret.
    /// </summary>
    void Update()
    {
        if (!MenuPause.Paused)
        {
            this._health.Update(this._fsm.CurrentState);
            if (this._health.IsDead)
            {
                this._fsm.FreeComputerSpawn();

                this._fsm = null;
                Destroy(gameObject);
            }
            if (this._fsm != null) this._fsm.Update(Time.deltaTime);
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
}