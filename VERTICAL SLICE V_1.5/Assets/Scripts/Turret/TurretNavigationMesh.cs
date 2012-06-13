using UnityEngine;
using System.Collections;

public class TurretNavigationMesh
{
	#region Attributres
    /// <summary>
    /// The _my transform.
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// The _navigation agent.
    /// </summary>
    private NavMeshAgent _navigationAgent;
    /// <summary>
    /// The _target position.
    /// </summary>
    private Vector3 _targetPosition;
    #endregion
    #region Constructors
    public TurretNavigationMesh(Transform pMyTransform, NavMeshAgent pNavigationAgent)
    {
        this._myTransform = pMyTransform;
        this._navigationAgent = pNavigationAgent;
    }
    #endregion
    #region Methods
    public void Update()
    {
        Debug.DrawLine(this._targetPosition, this._myTransform.position, Color.green);
    }
    public void SetDestination(Vector3 pNewPosition, float speed, float acceleration, float angular, float stoppingDistance)
    {
        if (this._targetPosition != pNewPosition)
        {
			this._targetPosition = pNewPosition;

            this._navigationAgent.updatePosition = true;
			this._navigationAgent.speed = speed;
			this._navigationAgent.acceleration= acceleration;
			this._navigationAgent.angularSpeed = angular;
			this._navigationAgent.stoppingDistance = stoppingDistance;
			
            this._navigationAgent.SetDestination(this._targetPosition);
        }
    }
    public void StopDestination()
    {
        this._navigationAgent.updatePosition = false;
    }
    #endregion
}