using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Turret finite state machine.
/// </summary>
public class TurretStateMachine
{
    private static int TURRET_ID = 0;
    #region Constants
    private const float SPEEDIDLE = 0f;
    private const float ACCELERATIONIDLE = 0f;
    private const float ANGULARIDLE = 120f;
    private const float STOPPINGDISTANCEIDLE = 0f;

    private const float SPEEDPATROL = 2f;
    private const float ACCELERATIONPATROL = 3f;
    private const float ANGULARPATROL = 120f;
    private const float STOPPINGDISTANCEPATROL = 0f;

    private const float SPEEDCHASE = 3f;
    private const float ACCELERATIONCHASE = 4f;
    private const float ANGULARCHASE = 240f;
    private const float STOPPINGDISTANCECHASE = 0f;

    private const float SPEEDALERT = 4f;
    private const float ACCELERATIONALERT = 5f;
    private const float ANGULARALERT = 120f;
    private const float STOPPINGDISTANCEALERT = 0f;

    private const float SPEEDEVADE = 2.5f;
    private const float ACCELERATIONEVADE = 3.5f;
    private const float ANGULAREVADE = 120f;
    private const float STOPPINGDISTANCEEVADE = 0f;

    public const float DISTANCELIMITTOCHASE = 15f;
    public const float DISTANCELIMITTOEVADE = 10f;
    //TODO:Desenvolver fight do turret (laser saindo da câmera).
    public const float DISTANCELIMITTOFIGHT = 5f;

    private const float WORLDPOSITION_Y = .2f;
    private const float MINVALUEIDLE = 5f;
    private const float MAXVALUEIDLE = 10f;
    private const float MINVALUEPATROL = 15f;
    private const float MAXVALUEPATROL = 25f;
    #endregion
    #region Atrributes
    /// <summary>
    /// Identificador de cada turret.
    /// </summary>
    private long _idTurret;
    /// <summary>
    /// Enumeração da máquina de estados finita.
    /// </summary>
    private FiniteStateMachineType _currentState;
    private FiniteStateMachineType _lastCurrentState;
    /// <summary>
    /// Sistema de waypoints.
    /// </summary>
    private WaypointSystem _waypointSystem;
    private TurretNavigationMesh _navigationMesh;
    /// <summary>
    /// Raytrace da câmera do turret.
    /// </summary>
    private TurretTracer _tracer;
    /// <summary>
    /// Transform do turret.
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// Transform do target.
    /// </summary>
    private Transform _target;
    /// <summary>
    /// Distância em comparação ao target.
    /// </summary>
    private float _distanceLimitToChasing;
    private float _distanceLimitToEvade;
    /// <summary>
    /// Tempo em cada estado.
    /// </summary>
    private float _idleTime;
    private float _patrolTIme;
        /// <summary>
    /// The _terminal spawn.
    /// </summary>
    private List<ComputerManager> _computerSpawnList = new List<ComputerManager>();
    #endregion
    #region Constructors
    /// <summary>
    /// Inicia a máquina de estados.
    /// </summary>
    public TurretStateMachine(Transform pMyTransform)
    {
        this._idTurret = ++TURRET_ID;
        this._currentState = FiniteStateMachineType.IDLE;
        //SetNewState(FiniteStateMachineType.PATROL);

        this._distanceLimitToChasing = DISTANCELIMITTOCHASE;
        this._distanceLimitToEvade = DISTANCELIMITTOEVADE;
        this._idleTime = Random.Range(MINVALUEIDLE, MAXVALUEIDLE);
        this._patrolTIme = Random.Range(MINVALUEPATROL, MAXVALUEPATROL);

        this._myTransform = pMyTransform;
        this._tracer = new TurretTracer(this._myTransform.FindChild("Cabeca").FindChild("Camera"));
        this._target = GameObject.FindGameObjectWithTag("Player").transform;

        this._waypointSystem = new WaypointSystem(this._myTransform, this._target);
        this._navigationMesh = new TurretNavigationMesh(this._myTransform, this._myTransform.GetComponent<NavMeshAgent>());
    }
    #endregion
    #region Properties
    public FiniteStateMachineType CurrentState
    {
        get { return this._currentState; }
    }
    public TurretTracer Tracer { get { return this._tracer; } }
    #endregion
    #region Methods
    /// <summary>
    /// Atualiza o estado do turret.
    /// </summary>
    public void Update(float pDeltaTime)
    {
        // Atualiza o raytrace da câmera.
        this._tracer.Update(this._currentState != FiniteStateMachineType.IDLE);
        // Distância entre o turret e o target.
        float auxDistanceToTarget = Vector3.Distance(this._myTransform.position, this._target.position);
        // Distância entre o turret e o target em Y (no mundo).
        Vector3 worldTurret = this._myTransform.TransformPoint(this._myTransform.position);
        Vector3 worldTarget = this._target.TransformPoint(this._target.position);
        float auxDistance_Y = Mathf.Abs(Mathf.Abs(worldTurret.y) - Mathf.Abs(worldTarget.y));

        //Debug.Log(this._idTurret + " - STATE:" + this._currentState);
        switch (this._currentState)
        {
            case FiniteStateMachineType.IDLE:
                this.UpdateIdle(pDeltaTime, auxDistanceToTarget, auxDistance_Y);
                break;
            case FiniteStateMachineType.PATROL:
                this.UpdatePatrol(pDeltaTime, auxDistanceToTarget, auxDistance_Y);
                break;
            case FiniteStateMachineType.CHASE:
                this.UpdateChase(pDeltaTime);
                break;
            case FiniteStateMachineType.ALERT:
                this.UpdateAlert(pDeltaTime, auxDistanceToTarget, auxDistance_Y);
                break;
            case FiniteStateMachineType.EVADE:
                this.UpdateEvade(pDeltaTime);
                break;
            case FiniteStateMachineType.FIGHT:
            case FiniteStateMachineType.SHOT:
            case FiniteStateMachineType.BUILD:
                Debug.LogWarning(">> [FIGHT|SHOT|BUILD] FSM incompatível com o personagem Turret! <<");
                break;
        }

        // Último estado do turret.
        this._lastCurrentState = this._currentState;
    }
    /// <summary>
    /// Atualiza quando for IDLE.
    /// </summary>
    private void UpdateIdle(float pDeltaTime, float pDistanceToTarget, float pDistanceY)
    {
        // Se a distância for menor que o limite para se defender.
        if (this._tracer.HithingTarget && pDistanceToTarget < this._distanceLimitToEvade)// && pDistanceY < WORLDPOSITION_Y)
            this.SetNewState(FiniteStateMachineType.EVADE);
        // Se enxergar o target.
        else if (this._tracer.HithingTarget) this.SetNewState(FiniteStateMachineType.ALERT);
        // Se a distância for menor que o limite para perseguir.
        else if (pDistanceToTarget < this._distanceLimitToChasing)// && pDistanceY < WORLDPOSITION_Y)
            this.SetNewState(FiniteStateMachineType.CHASE);
        // Senão decrementa o tempo para mudar para patrulhando.
        else
        {
            this._idleTime -= pDeltaTime;
            if (this._idleTime <= 0) this.SetNewState(FiniteStateMachineType.PATROL);
        }
    }
    /// <summary>
    /// Atualiza quando for PATROL.
    /// </summary>
    private void UpdatePatrol(float pDeltaTime, float pDistanceToTarget, float pDistanceY)
    {
        // Se a distância for menor que o limite para se defender.
        if (this._tracer.HithingTarget && pDistanceToTarget < this._distanceLimitToEvade)// && pDistanceY < WORLDPOSITION_Y)
            this.SetNewState(FiniteStateMachineType.EVADE);
        // Se enxergar o target.
        else if (this._tracer.HithingTarget) this.SetNewState(FiniteStateMachineType.ALERT);
        // Se a distância for menor que o limite para perseguir.
        else if (pDistanceToTarget < this._distanceLimitToChasing)// && pDistanceY < WORLDPOSITION_Y)
            this.SetNewState(FiniteStateMachineType.CHASE);
        // Senão decrementa o tempo para mudar para patrulhando.
        else
        {
            this._patrolTIme -= pDeltaTime;
            if (this._patrolTIme <= 0) this.SetNewState(FiniteStateMachineType.IDLE);
        }
    }
    /// <summary>
    /// Atualiza quando for CHASING.
    /// </summary>
    private void UpdateChase(float pDeltaTime)
    {
        // Se enxergar o target.
        if (this._tracer.HithingTarget)
            this.SetNewState(FiniteStateMachineType.ALERT);
        else //Atualiza a navegação.
            this._navigationMesh.SetDestination(this._target.position, SPEEDCHASE, ACCELERATIONCHASE, ANGULARCHASE, STOPPINGDISTANCECHASE);
    }
    /// <summary>
    /// Atualiza quando for ALERT.
    /// </summary>
    private void UpdateAlert(float pDeltaTime, float pDistanceToTarget, float pDistanceY)
    {
        // Se enxergar o player ou se a distância for menor que o limite para se defender.
        if (this._tracer.HithingTarget && pDistanceToTarget < this._distanceLimitToEvade)// && pDistanceY < WORLDPOSITION_Y)
            this.SetNewState(FiniteStateMachineType.EVADE);

        //Se encontrar um computador
        if (this._tracer.TryFindComputer())
            this.SetComputerSpawn(this._tracer.Hit.transform.GetComponent<ComputerManager>());
        else if (this._waypointSystem.NearComputer != null)
        {
            float auxDistance = Vector2.Distance(this._myTransform.position, this._waypointSystem.NearComputer.GameObject.transform.position);
            if (auxDistance <= 2f)
                this.SetComputerSpawn(this._waypointSystem.NearComputer.GameObject.transform.GetComponent<ComputerManager>());
        }
    }
    /// <summary>
    /// Atualiza quando for EVADE.
    /// </summary>
    private void UpdateEvade(float pDeltaTime)
    {
        // Se a distância para o computador for menor que a do player.
        if (Vector3.Distance(this._myTransform.position, this._waypointSystem.NearComputer.GameObject.transform.position) <= Vector3.Distance(this._myTransform.position, this._target.position))
            this.SetNewState(FiniteStateMachineType.ALERT);
    }
    /*/// <summary>
    /// Atualiza a movimentação
    /// </summary>
    private void UpdateMovement(GameObject followObject, float moveSpeed, float rotationSpeed)
    {
        Debug.DrawLine(followObject.transform.position, this._myTransform.position, Color.yellow);

        // Ajusta a rotação para ficar de frente para o target.
        this._myTransform.rotation = Quaternion.Slerp(this._myTransform.rotation,
            Quaternion.LookRotation(followObject.transform.position - this._myTransform.position),
            rotationSpeed * Time.deltaTime);

        // Incrementa a posição para seguir o target.
        this._myTransform.position += this._myTransform.forward * moveSpeed * Time.deltaTime;
    }*/
    /// <summary>
    /// Seta o novo comportamento.
    /// </summary>
    /// <param name="pNewStateMachine"></param>
    public void SetNewState(FiniteStateMachineType pNewStateMachine)
    {
        //Novo estado.
        this._currentState = pNewStateMachine;
        //Dependendo do estado, atualiza o próximo passo do turret.
        switch (this._currentState)
        {
            case FiniteStateMachineType.IDLE:
                this._patrolTIme = Random.Range(MINVALUEPATROL, MAXVALUEPATROL);
                this._navigationMesh.StopDestination();
                break;
            case FiniteStateMachineType.PATROL:
                this._idleTime = Random.Range(MINVALUEIDLE, MAXVALUEIDLE);
                this._waypointSystem.UpdateWaypoint();
                this._navigationMesh.SetDestination(this._waypointSystem.NearWaypoint.GameObject.transform.position,
                    SPEEDPATROL, ACCELERATIONPATROL, ANGULARPATROL, STOPPINGDISTANCEPATROL);
                break;
            case FiniteStateMachineType.CHASE:
                this._navigationMesh.SetDestination(this._target.position,
                    SPEEDCHASE, ACCELERATIONCHASE, ANGULARCHASE, STOPPINGDISTANCECHASE);
                break;
            case FiniteStateMachineType.ALERT:
                this._waypointSystem.UpdateComputer();
                this._navigationMesh.SetDestination(this._waypointSystem.NearComputer.GameObject.transform.position,
                    SPEEDALERT, ACCELERATIONALERT, ANGULARALERT, STOPPINGDISTANCEALERT);
                break;
            case FiniteStateMachineType.EVADE:
                //TODO:Criar sistema de fuga e defesa contra o player!!!
                this.SetNewState(FiniteStateMachineType.ALERT);
                break;
            case FiniteStateMachineType.FIGHT:
            case FiniteStateMachineType.SHOT:
            case FiniteStateMachineType.BUILD:
                Debug.LogWarning(">> [FIGHT|SHOT|BUILD] FSM incompatível com o personagem Turret! <<");
                break;
        }
    }
    /// <summary>
    /// Se encontrou um waypoint.
    /// </summary>
    /// 
    public void FoundWaypoint()
    {
        if (this._currentState == FiniteStateMachineType.PATROL)
        {
            this._waypointSystem.UpdateWaypoint();
            this._navigationMesh.SetDestination(this._waypointSystem.NearWaypoint.GameObject.transform.position,
                SPEEDPATROL, ACCELERATIONPATROL, ANGULARPATROL, STOPPINGDISTANCEPATROL);
        }
    }
    /// <summary>
    /// Sets the computer spawn.
    /// </summary>
    /// <param name='pComputer'>
    /// P computer.
    /// </param>
    public void SetComputerSpawn(ComputerManager pComputer)
    {
        pComputer.OnSpawnBehaviour();
        this._computerSpawnList.Add(pComputer);

        this.SetNewState(FiniteStateMachineType.PATROL);
    }
    /// <summary>
    /// 
    /// </summary>
    public void FreeComputerSpawn()
    {
        this._computerSpawnList.ForEach(c => c.WasActivatedSpawn = false);
    }
    #endregion
}