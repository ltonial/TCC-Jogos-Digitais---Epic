using UnityEngine;
using System.Collections;

public class HumanoidStateMachine
{
    #region Constants
    private const float TIMETOCHANGE = 2f;
    private const float FIGHTERAGENTESPEED = 1f;
    private const float SHOOTERAGENTESPEED = .25f;
    #endregion
    #region Attributres
    /// <summary>
    /// Objeto transform do turret.
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// The _target.
    /// </summary>
    private Transform _target;
    /// <summary>
    /// The _navigation.
    /// </summary>
    private HumanoidNavigationMesh _navigationMesh;
    /// <summary>
    /// Enumeração da máquina de estados finita.
    /// </summary>
    private FiniteStateMachineType _currentState;
    private FiniteStateMachineType _lastCurrentState;
    /// <summary>
    /// Comportamento do peão.
    /// </summary>
    private BehaviourType _behaviour;
    /// <summary>
    /// Tipo de peão.
    /// </summary>
    public HumanoidType _humanoidType;
    /// <summary>
    /// The _gun object.
    /// </summary>
    private GameObject _gunObject;
    /// <summary>
    /// Arma/Tiro.
    /// </summary>
    private HumanoidGun _gun;
    /// <summary>
    /// Tempos.
    /// </summary>
    private float _timeToChange;
	/// <summary>
	/// The _start heath.
	/// </summary>
	private float _startHeath;
    #endregion
    #region Constructors
    public HumanoidStateMachine(Transform pTransform, HumanoidType pHumanoidType, BehaviourType pBehaviour, float startHealth)
    {
        this._myTransform = pTransform;
        this._timeToChange = TIMETOCHANGE;
		this._startHeath=startHealth;

        this._humanoidType = pHumanoidType;
        this._behaviour = pBehaviour;
        if (this._behaviour == BehaviourType.DEFAULT) this._currentState = FiniteStateMachineType.IDLE;
        else this._currentState = FiniteStateMachineType.PATROL;

        this._target = GameObject.FindGameObjectWithTag("Player").transform;
        this._navigationMesh = new HumanoidNavigationMesh(this._myTransform, this._target, this._myTransform.GetComponent<NavMeshAgent>(), this._behaviour);

        this._gunObject = this._myTransform.FindChild("Gun").gameObject;
        this._gun = new HumanoidGun(this._gunObject);
    }
    #endregion
    #region Methods
    /// <summary>
    /// Atualiza o estado do peão.
    /// </summary>
    /// <param name="pDeltaTime"></param>
    public void Update(float pDeltaTime, float currentHealth)
    {
		//Atualiza o comportamento (para alterar a velocidade do humanóide).
		this.UpdateBehaviour(currentHealth);
		
        //Debug.Log("HUMANOID STATE: " + this._currentState);
        switch (this._currentState)
        {
            case FiniteStateMachineType.IDLE:
                this.ChangeState(pDeltaTime, FiniteStateMachineType.BUILD);
                break;
            case FiniteStateMachineType.BUILD:
                if (this.ChangeState(pDeltaTime, FiniteStateMachineType.PATROL)) this._navigationMesh.SetTargetNavigation();
                break;
            case FiniteStateMachineType.PATROL:
                if (this._humanoidType == HumanoidType.SHOOTER) this.ChangeState(pDeltaTime, FiniteStateMachineType.SHOT);
                else if (this._humanoidType == HumanoidType.FIGHTER) this.ChangeState(pDeltaTime, FiniteStateMachineType.FIGHT);
                break;
            case FiniteStateMachineType.CHASE:
                break;
            case FiniteStateMachineType.ALERT:
            case FiniteStateMachineType.EVADE:
                Debug.LogWarning(">> [ALERT|EVADE] FSM incompatível com o personagem Peão! <<");
                break;
            case FiniteStateMachineType.FIGHT:
                //TODO:É necessário as animações do peão.
                //if (this._humanoidType == HumanoidType.SHOOTER && this._navigationSystem.SetDestination()) this._currentState = FiniteStateMachineType.SHOT;
                break;
            case FiniteStateMachineType.SHOT:
				//TODO:Se está próximo, atacar com a arma.
                this.UpdateShot(pDeltaTime);
                break;
        }

        //Último estado do peão.
        this._lastCurrentState = _currentState;
    }
	private void UpdateBehaviour(float currentHealth)
	{
		float percentage = currentHealth * 100 / this._startHeath;	
		if (percentage < 50)
			this._behaviour = BehaviourType.IMPROVED;
		else if (percentage < 20)
			this._behaviour = BehaviourType.SLOW;
		
	}
    /// <summary>
    /// Atualiza açãode tiro.
    /// </summary>
    private void UpdateShot(float pDeltaTime)
    {
        this._navigationMesh.UpdateDestination(this._behaviour);
        this._gun.UpdateShot(pDeltaTime);
    }
    /// <summary>
    /// Altera o estado de acordo com o tempo.
    /// </summary>
    private bool ChangeState(float pDeltaTime, FiniteStateMachineType pNextState)
    {
        this._timeToChange -= pDeltaTime;
        if (this._timeToChange <= 0)
        {
            this._currentState = pNextState;
            this._timeToChange = TIMETOCHANGE;
            return true;
        }
        return false;
    }
    #endregion
}
