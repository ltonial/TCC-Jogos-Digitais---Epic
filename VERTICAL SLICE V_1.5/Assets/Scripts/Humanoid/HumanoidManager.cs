using UnityEngine;
using System.Collections;
using System.Linq;

public class HumanoidManager : MonoBehaviour 
{
	#region Attributres
	/// <summary>
    /// Objeto transform do turret.
    /// </summary>
    private GameObject _myObject;
    /// <summary>
    /// Objeto transform do turret.
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// Energia.
    /// </summary>
    private HumanoidHealth _health;
    /// <summary>
    /// Finite State Machine.
    /// </summary>
    private HumanoidStateMachine _fsm;
    /// <summary>
    /// Comportamento do pe�o.
    /// </summary>
    public BehaviourType _behaviour;
    /// <summary>
    /// Tipo de pe�o.
    /// </summary>
    public HumanoidType _humanoidType;
	/// <summary>
	/// The ragdoll.
	/// </summary>
	private GameObject _ragdoll;
	/// <summary>
	/// The _item energy.
	/// </summary>
	private GameObject _energyItem;
	#endregion
    #region Properties
    public HumanoidHealth Health { get { return this._health; } }
	public GameObject MyObject { get { return this._myObject; } }
	public Transform MyTransform { get { return this._myTransform; } }
    #endregion
    #region Methods (Inherit)
    /// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		this._myObject = gameObject;
        this._myTransform = transform;
        
        this._health = new HumanoidHealth();
        this._fsm = new HumanoidStateMachine(this._myTransform, this._humanoidType, this._behaviour, this._health.CurrentHealth);
		
		this._ragdoll = (GameObject)Resources.Load("Ragdolls/RagdollHumanoidRed");
		this._energyItem = (GameObject)Resources.Load("Items/EnergyItem");
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
        this._health.Update();
        if (this._health.IsDead)
        {
			//TODO:Instancia o item de energia.
			//Instantiate(this._energyItem, this._myTransform.position, Quaternion.identity);
			//Instancia o ragdoll.
			Instantiate(this._ragdoll, this._myTransform.position, this._myTransform.rotation);
			
			this._fsm = null;
            PlayerCombat.HumanoidsList.Remove(this);
			Destroy(gameObject);
        }
		if (this._fsm != null) this._fsm.Update(Time.deltaTime, this._health.CurrentHealth);
	}
	#endregion	
}
