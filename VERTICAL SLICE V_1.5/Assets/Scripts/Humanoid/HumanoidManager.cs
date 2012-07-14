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
    /// Comportamento do peão.
    /// </summary>
    public BehaviourType _behaviour;
    /// <summary>
    /// Tipo de peão.
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
    /// <summary>
    /// The _computer.
    /// </summary>
    private ComputerManager _terminalCreation;

    /// <summary>
    /// Enemy
    /// </summary>

    #endregion
    #region Properties
    public HumanoidHealth Health { get { return this._health; } }

    public GameObject MyObject { get { return this._myObject; } }

    public Transform MyTransform { get { return this._myTransform; } }

    public BehaviourType BehaviourType
    {
        set { this._behaviour = value; }
    }
    #endregion
    #region Methods (Inherit)
    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {

        this._myObject = gameObject;
        this._myTransform = transform;

        this._health = new HumanoidHealth(this._myTransform.FindChild("HealthText").GetComponent<TextMesh>());
        this._fsm = new HumanoidStateMachine(this._myTransform, this._humanoidType, this._behaviour, this._health.CurrentHealth);

        this._ragdoll = (GameObject)Resources.Load("Ragdolls/RagdollHumanoidRed");
        this._energyItem = (GameObject)Resources.Load("Items/EnergyItem");
        
        if (_myTransform.animation.GetClipCount() == 0)
        {
            _myTransform.animation.AddClip((AnimationClip)Resources.Load("Animations/run"), "run");

        }

    }
    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (!MenuPause.Paused)
        {
            this._health.Update();
            if (this._health.IsDead)
            {
                //TODO:Instancia o item de energia.
                //Instantiate(this._energyItem, this._myTransform.FindChild("Controlador").FindChild("Pelvis").FindChild("Cervical03").FindChild("Cervical02").
                //    FindChild("Cervical01").FindChild("ClaviculaE").FindChild("BracoE").FindChild("AntebracoE").FindChild("MaoE").position, Quaternion.identity);
                Instantiate(this._energyItem, this._myTransform.FindChild("Gun").position, Quaternion.identity);
                //Instancia o ragdoll.
                Instantiate(this._ragdoll, this._myTransform.position, this._myTransform.rotation);

                this._fsm = null;
                PlayerCombat._humanoids.Remove(gameObject);
                Destroy(gameObject);
            }
            if (this._fsm != null)
                this._fsm.Update(Time.deltaTime, this._health.CurrentHealth);
        }

        if (_myTransform.GetComponent<NavMeshAgent>().velocity != Vector3.zero)
        {
            _myTransform.animation.wrapMode = WrapMode.Default;
            _myTransform.animation.CrossFade("run");
        }
        else
        {
            //AnimationState newIdle = _myTransform.animation.CrossFadeQueued("idle", 0f, QueueMode.CompleteOthers);
            //newIdle.wrapMode = WrapMode.PingPong;
            //newIdle.speed = 0.5f;
            _myTransform.animation.Stop();
        }

    }
    #endregion
    #region Methods (Class)
    public void SetComputer(ComputerManager pTerminal)
    {
        this._terminalCreation = pTerminal;
    }
    public void FreeTerminalCreation()
    {
        if (this._terminalCreation != null)
            this._terminalCreation.TotalSpawn--;
    }
    public void RemoveTerminal()
    {
        this._terminalCreation = null;
    }
    #endregion
}