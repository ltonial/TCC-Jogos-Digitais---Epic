using UnityEngine;

public class HumanoidHealth
{
    #region Constants
    private const int HEALTH = 5;
    #endregion
    #region Attributes
    /// <summary>
    /// TODO: Testando energia.
    /// </summary>
    public float HealthBarLenght;
    /// <summary>
    /// Máximo de energia do turret.
    /// </summary>
    private int _maxHealth;
    /// <summary>
    /// Quantidade atual de energia.
    /// </summary>
    private int _currentHealth;
	/// <summary>
	/// The _text health.
	/// </summary>
	private TextMesh _textHealth;
    #endregion
    #region Constructors
    /// <summary>
    /// Inicializando a energia do turret.
    /// </summary>
    public HumanoidHealth(TextMesh pText)
    {
        // Setando a energia inicial.
        this._maxHealth = HEALTH;
        this._currentHealth = _maxHealth;
        
		// TODO: Simulando barra de energia.
        this.HealthBarLenght = Screen.width / 2 - 20;
		
		this._textHealth = pText;
		this._textHealth.renderer.material.color = Color.yellow;
		
    }
    #endregion
    #region Properties
    /// <summary>
    /// Se o turret está morto (sem energia).
    /// </summary>
    public bool IsDead { get { return this._currentHealth <= 0; } }
    /// <summary>
    /// Retorna a energia corrente.
    /// </summary>
    public int CurrentHealth { get { return this._currentHealth; } }
    /// <summary>
    /// Retorna o máximo de energia.
    /// </summary>
    public int MaxHealth { get { return this._maxHealth; } }
    #endregion
    #region Methods
    /// <summary>
    /// Atualizando o turret.
    /// </summary>
    public void Update()
    {
        this.UpdateHealth(0);
		this._textHealth.text = this._currentHealth.ToString();
    }
    /// <summary>
    /// Ajusta a energia do inimigo.
    /// </summary>
    public void UpdateHealth(int pHit)
    {
        // Se ainda tem energia.
        //if (this._currentHealth > 0) this._currentHealth += hit;
        this._currentHealth += pHit;

        // TODO: Testando interface 2D
        if (this._currentHealth < 0)
            this._currentHealth = 0;
        if (this._currentHealth > this._maxHealth)
            this._currentHealth = this._maxHealth;
        if (this._maxHealth < 1)
            this._maxHealth = 1;
        this.HealthBarLenght = (Screen.width / 2 - 20) * (this._maxHealth / (float)this._maxHealth);
		
		//Debug.Log ("Humanoid Health: " + this._currentHealth);
    }
    #endregion
}
