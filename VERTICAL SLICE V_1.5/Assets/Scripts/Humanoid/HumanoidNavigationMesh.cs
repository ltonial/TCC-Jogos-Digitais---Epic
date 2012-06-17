using UnityEngine;
using System.Collections;

public class HumanoidNavigationMesh
{
	#region Constants
	private const float STOPPINGDISTANCETOSHOOT = 4f;
	private const float STOPPINGDISTANCETOFIGHT = 1f;
	private const float SPEEDDEFAULT = 2f;
	private const float ACCELERATIONDEFAULT = 3f;
	private const float SPEEDIMPROVED = 3f;
	private const float ACCELERATIONIMPROVED = 4f;
	private const float SPEEDSLOW = 1f;
	private const float ACCELERATIONSLOW = 2f;
	private const float SPEEDGROUP = 2f;
	private const float ACCELERATIONGROUP = 3f;
	#endregion
    #region Attributres
	/// <summary>
	/// The _my transform.
	/// </summary>
	private Transform _myTransform;
	/// <summary>
	/// The _target transform.
	/// </summary>
	private Transform _targetTransform;
	/// <summary>
	/// The _last target position.
	/// </summary>
	private Vector3 _lastTargetPosition;
	/// <summary>
	/// The _navigation agent.
	/// </summary>
	private NavMeshAgent _navigationAgent;
	/// <summary>
	/// The _behaviour.
	/// </summary>
	private BehaviourType _behaviour;
    #endregion
    #region Constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="NavigationMesh"/> class.
	/// </summary>
	public HumanoidNavigationMesh(Transform pMyTransform, Transform pTarget, NavMeshAgent pNavigationAgent, BehaviourType behaviour)
	{
		this._myTransform = pMyTransform;
    
		this._targetTransform = pTarget;
		this._lastTargetPosition = this._targetTransform.position;

		this._navigationAgent = pNavigationAgent;
		this._behaviour = behaviour;
	}
    #endregion
    #region Methods
	/// <summary>
	/// Updates the destination.
	/// </summary>
	/// <param name='newBehaviour'>
	/// New behaviour.
	/// </param>
	public void UpdateDestination(BehaviourType newBehaviour)
	{
		Debug.DrawLine (this._targetTransform.position, this._myTransform.position, Color.green);
		
		//Avalia a nova velocidade de acordo com o comportamento do humanóide.
		SetNavigationVelocity (newBehaviour);
		
		if (this._lastTargetPosition != this._targetTransform.position) 
		{
			float distance = Vector3.Distance(this._myTransform.position, this._targetTransform.position);
			if (distance > 1f)
			{
				this._navigationAgent.SetDestination(this._targetTransform.position);
				this._navigationAgent.stoppingDistance = STOPPINGDISTANCETOSHOOT;
			}
			this._lastTargetPosition = this._targetTransform.position;
		}
	}
	/// <summary>
	/// Sets the target navigation.
	/// </summary>
	public void SetTargetNavigation()
	{
		this._navigationAgent.SetDestination (this._targetTransform.position);
	}
	/// <summary>
	/// Sets the navigation velocity.
	/// </summary>
	/// <param name='newBehaviour'>
	/// New behaviour.
	/// </param>
	private void SetNavigationVelocity(BehaviourType newBehaviour)
	{
		if (this._behaviour != newBehaviour) 
		{
			this._behaviour = newBehaviour;
			//Debug.Log ("BEHAVIOUR: " + this._behaviour);
			
			switch (this._behaviour) 
			{
				case BehaviourType.DEFAULT:
					this._navigationAgent.speed = SPEEDDEFAULT;
					this._navigationAgent.acceleration = ACCELERATIONDEFAULT;
					break;
				case BehaviourType.GROUP:
					this._navigationAgent.speed = SPEEDGROUP;
					this._navigationAgent.acceleration = ACCELERATIONGROUP;
					break;
				case BehaviourType.IMPROVED:
					this._navigationAgent.speed = SPEEDIMPROVED;
					this._navigationAgent.acceleration = ACCELERATIONIMPROVED;
					break;
				case BehaviourType.SLOW:
					this._navigationAgent.speed = SPEEDSLOW;
					this._navigationAgent.acceleration = ACCELERATIONSLOW;
					break;
			}
		}
	}
    #endregion
}