using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ComputerManager : MonoBehaviour
{
    #region Constants
    private const int LIMITTOSPAWN = 2;
    private const float SPAWNTIME = 30f;
    #endregion
    #region Attributes
    private bool _wasHacked;
    private bool _wasActivatedSpawn;
    private float _timeToSpawn;
    private int _totalSpawn;
    private GameObject _enemyObject;
    private Transform _spawnTransform;
    private List<HumanoidManager> _humanoidSpawnedList = new List<HumanoidManager>();
    private Material _alarmMaterial;
    private Material _hackedMaterial;
    private Material _desactivatedMaterial;
    private Transform _screenTransform;
    private Light _lightObject;
    private Challenge3 _challengeRing;
    private Challenge4 _challengeFinal;
    #endregion
    #region Properties
    public int TotalSpawn
    {
        get { return this._totalSpawn; }
        set { this._totalSpawn = value; }
    }
    public bool WasHacked
    {
        get { return this._wasHacked; }
    }
    public bool WasActivatedSpawn
    {
        get { return this._wasActivatedSpawn; }
        set { this._wasActivatedSpawn = value; }
    }
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        this._wasHacked = false;
        this._wasActivatedSpawn = false;

        this._timeToSpawn = SPAWNTIME;
        this._totalSpawn = 0;

        this._screenTransform = transform.FindChild("Screen");
        this._lightObject = transform.FindChild("Light").GetComponent<Light>();

        this._enemyObject = (GameObject)Resources.Load("Enemies/HumanoidRed");
        this._spawnTransform = transform.FindChild("Spawn");

        this._alarmMaterial = (Material)Resources.Load("Computer/Alarm");
        this._hackedMaterial = (Material)Resources.Load("Computer/Hacked");
        this._desactivatedMaterial = (Material)Resources.Load("Computer/Desactivated");

        GameObject aux = GameObject.Find("Level");
        this._challengeRing = aux.transform.GetComponent<Challenge3>();
        this._challengeFinal = gameObject.transform.GetComponent<Challenge4>();
        //Debug.Log("4: " + this._challengeFinal);

        this.ChangeTextureAndLight(this._desactivatedMaterial, Color.yellow);
    }
    void Update()
    {
        if (this._wasActivatedSpawn)
        {
            this._timeToSpawn -= Time.deltaTime;
            if (this._timeToSpawn <= 0f)
            {
                this._timeToSpawn = SPAWNTIME;
                this.InstantiateEnemy();
            }
        }
        else if (!this._wasActivatedSpawn && !this._wasHacked)
        {
            ChangeTextureAndLight(this._desactivatedMaterial, Color.yellow);
        }
    }
    #endregion
    #region Methods (Class)
    /// <summary>
    /// 
    /// </summary>
    public void OnHackedBehaviour()
    {
        if (!this._wasActivatedSpawn)
        {
            this._wasHacked = true;

            if (gameObject.transform.tag == "ComputerTurnRing")
            {
                Debug.Log("ALELUIA");
                this._challengeRing.ActiveRotate(this);
            }
            else if (gameObject.transform.tag == "ComputerFinal")
                this._challengeFinal.ActiveRotate(this);

            ChangeTextureAndLight(this._hackedMaterial, Color.blue);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void OnUnhackedBehaviour()
    {
        if (this._wasHacked)
        {
            this._wasHacked = false;
            ChangeTextureAndLight(this._desactivatedMaterial, Color.yellow);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collider"></param>
    public void OnSpawnBehaviour()
    {
        if (!this._wasActivatedSpawn)
        {
            this._wasActivatedSpawn = true;
            this._wasHacked = false;

            this.InstantiateEnemy();

            ChangeTextureAndLight(this._alarmMaterial, Color.red);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newColor"></param>
    private void ChangeTextureAndLight(Material newMaterial, Color newColor)
    {
        this._screenTransform.renderer.material = new Material(newMaterial);
        this._lightObject.color = newColor;
    }
    /// <summary>
    /// 
    /// </summary>
    private void InstantiateEnemy()
    {
        if (this._totalSpawn > LIMITTOSPAWN)
        {
            this._wasActivatedSpawn = false;

            this._totalSpawn = 0;
            this._humanoidSpawnedList.ForEach(h => h.RemoveTerminal());
            this._humanoidSpawnedList.Clear();

            this.ChangeTextureAndLight(this._desactivatedMaterial, Color.yellow);
        }
        else
        {
            this._totalSpawn++;
            GameObject auxSpawn = (GameObject)Instantiate(this._enemyObject, this._spawnTransform.position, Quaternion.identity);

            HumanoidManager auxHumanoid = auxSpawn.GetComponent<HumanoidManager>();
            auxHumanoid.SetComputer(this);
            auxHumanoid.BehaviourType = BehaviourType.GROUP;

            this._humanoidSpawnedList.Add(auxHumanoid);
        }
    }
    #endregion
}
