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
    /// M�quina de estados finita.
    /// </summary>
    private TurretStateMachine _fsm;
	/// <summary>
	/// The _terminal spawn.
	/// </summary>
	private ComputerManager _terminalSpawn;
    #endregion
    #region Properties
    /// <summary>
    /// Recupera a m�quina de estados.
    /// </summary>
    public TurretStateMachine FSM { get { return this._fsm; } }
    /// <summary>
    /// Recupera a energia.
    /// </summary>
    public TurretHealth Health { get { return this._health; } }
    #endregion
    #region Methods (Inherit)
    /// <summary>
    /// Iniciando as vari�veis no in�cio do script.
    /// </summary>
    void Start()
    {
        this._myTransform = transform;
        this._health = new TurretHealth();
        this._fsm = new TurretStateMachine(this._myTransform);
    }
    /// <summary>
    /// Atualizando o turret.
    /// </summary>
    void Update()
    {
		if (!MenuPause.Paused)
		{
        this._health.Update();
        if (this._health.IsDead)
        {
			FreeTerminalSpawn();
			
            this._fsm = null;
            Destroy(gameObject);
        }
        if (this._fsm != null) this._fsm.Update(Time.deltaTime);
		}
    }
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 100), "T.HEALTH: " + this._health.CurrentHealth);
	}
    /// <summary>
    /// Desenha Gizmos em volta do Turret.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TurretStateMachine.DISTANCELIMITTOCHASE);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TurretStateMachine.DISTANCELIMITTOEVADE);
    }
    #endregion
	#region Methods (Class)
	public void SetTerminalSpawn(ComputerManager pTerminal)
	{
		this._terminalSpawn = pTerminal;
	}
	public void FreeTerminalSpawn()
	{
		Debug.Log("<< " + this._terminalSpawn.ActiveSpawn);
		if (this._terminalSpawn != null) this._terminalSpawn.ActiveSpawn = false;
		Debug.Log(">> " + this._terminalSpawn.ActiveSpawn);
	}
	#endregion
}