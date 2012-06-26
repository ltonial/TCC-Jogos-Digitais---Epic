using UnityEngine;
using System.Collections;

public class TankPlayer
{
    #region Constants
    private const int MAXENERGYLIFE = 5;
    private const int MAXENERGYSHOT = 100;
    private const int MAXENERGYSUMP = 10;
    #endregion
    #region Atributes
    /// <summary>
    /// Maxima vida do player
    /// </summary>
    private int _maxEnergyLife;
    /// <summary>
    /// Vida atual do player
    /// </summary>
    private int _currentEnergyLife;
    /// <summary>
    /// Maxima energia de tiro do player
    /// </summary>
    private int _maxEnergyShot;
    /// <summary>
    ///Energia de tiro atual do player
    /// </summary>
    private int _currentEnergyShot;
    /// <summary>
    /// Maxima energia do reservatorio
    /// </summary>
    private int _maxEnergySump;
    /// <summary>
    /// Energia atual do reservatorio
    /// </summary>
    private int _currentEnergySump;
    /// <summary>
    /// The _time.
    /// </summary>
    private float _time;
    /// <summary>
    /// The _time to reload.
    /// </summary>
    private float _timeToReload;
    /// <summary>
    /// The _energy shot. Tubo de energia de tiro filho do Player.
    /// </summary>
    private GameObject _tankEnergyShot;
    /// <summary>
    /// The _energy life. Tubo de energia de vida filho do Player.
    /// </summary>
    private GameObject _tankEnergyLife;
    /// <summary>
    /// The _energy sump. Tubo de energia reserva filho do Player.
    /// </summary>
    private GameObject _tankEnergySump;
    #endregion
    #region Constructors
    /// <summary>
    /// Construtor TankPlayer.
    /// </summary>
    public TankPlayer()
    {
        this._maxEnergyLife = MAXENERGYLIFE;
        this._currentEnergyLife = this._maxEnergyLife;

        this._maxEnergyShot = MAXENERGYSHOT;
        this._currentEnergyShot = this._maxEnergyShot;

        this._maxEnergySump = MAXENERGYSUMP;
        this._currentEnergySump = 0;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Gets or sets the energy life.
    /// </summary>
    public int EnergyLife
    {
        get { return this._currentEnergyLife; }
        set { this._currentEnergyLife = value; }
    }
    /// <summary>
    /// Gets or sets the energy shot.
    /// </summary>
    public int EnergyShot
    {
        get { return this._currentEnergyShot; }
        set { this._currentEnergyShot = value; }
    }
    public int EnergySump
    {
        get { return this._currentEnergySump; }
    }
    /// <summary>
    /// Pega energia do cenario e joga no tanque reserva
    /// </summary>
    public int ItemEnergy
    {
        get;
        set;
    }
    /// <summary>
    ///Coloca a energia do tanque reserva para um dos outros tanques
    /// </summary>
    public TankType ChoiceSideEnergy
    {
        get;
        set;
    }
    #endregion
    #region Methods
    public void Start()
    {
        this.ItemEnergy = 0;
        this.ChoiceSideEnergy = TankType.NONE;
        this._time = Time.deltaTime;
        this._timeToReload = .01f;
        this._tankEnergyShot = GameObject.Find("TankEnergyShot");
        this._tankEnergyShot.transform.localScale = new Vector3(1f,this._currentEnergyShot/MAXENERGYSHOT,1f);
        this._tankEnergyLife = GameObject.Find("TankEnergyLife");
        this._tankEnergyLife.transform.localScale = new Vector3(1f,this._currentEnergyLife/MAXENERGYLIFE,1f);
        this._tankEnergySump = GameObject.Find("TankEnergySump");
        this._tankEnergySump.transform.localScale = new Vector3(1f,this._currentEnergySump/MAXENERGYSUMP,1f);
    }
    public void Update()
    {
        if (this.ItemEnergy > 0)
        {
            if (Time.deltaTime > (this._time * this._timeToReload))
            {
                this._time = Time.deltaTime;
                if (this._currentEnergySump < this._maxEnergySump)
                {
                    this._currentEnergySump++;
                    this.ItemEnergy--;
                }
                else
                {
                    this.ItemEnergy = 0;
                }
            }
        }
        if (this.ChoiceSideEnergy == TankType.SHOT)
        {
            if (Time.deltaTime > (this._time * this._timeToReload))
            {
                this._time = Time.deltaTime;
                if (this._currentEnergyShot < this._maxEnergyShot && this._currentEnergySump > 0)
                {
                    this._currentEnergyShot++;
                    this._currentEnergySump--;
                }
                else
                {
                    this.ChoiceSideEnergy = TankType.NONE;
                }
            }

        }
        else if (this.ChoiceSideEnergy == TankType.LIFE)
        {
            if (Time.deltaTime > (this._time * this._timeToReload))
            {
                this._time = Time.deltaTime;
                if (this._currentEnergyLife < this._maxEnergyLife && this._currentEnergySump > 0)
                {
                    this._currentEnergyLife++;
                    this._currentEnergySump--;
                }
                else
                {
                    this.ChoiceSideEnergy = TankType.NONE;
                }
            }
        }
        this._tankEnergyLife.transform.localScale = new Vector3(1f,((float)this._currentEnergyLife/(float)MAXENERGYLIFE),1f);
        this._tankEnergyShot.transform.localScale = new Vector3(1f,((float)this._currentEnergyShot/(float)MAXENERGYSHOT),1f);
        this._tankEnergySump.transform.localScale = new Vector3(1f,((float)this._currentEnergySump/(float)MAXENERGYSUMP),1f);
    }
    #endregion
}