using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    #region Atributes
    private const string SHOT = "Fire1";
    /// <summary>
    /// The current state player.
    /// </summary>
    public static StatePlayerType CurrentStatePlayer = StatePlayerType.WALK;
    /// <summary>
    /// Instancia da classe TankPlayer.
    /// </summary>
    private TankPlayer _tank;
    /// <summary>
    /// Instancia da classe WeaponPlayer.
    /// </summary>
    private WeaponPlayer _weapon;
    /// <summary>
    /// The _mouse y.
    /// </summary>
    private float _mouseY;
    /// <summary>
    /// The _rdy to shot.
    /// </summary>
    private bool _isReadyToShot;
    /// <summary>
    /// Testa se ja tocou o som de armar o arco.
    /// </summary>
    private bool _isPlayedBowAssembleSound;
    /// <summary>
    /// Som de armar arco.
    /// </summary>
    private AudioClip _bowAssembleSound;
    
    #endregion
    #region Properties
    public TankPlayer Tank
    {
        get { return this._tank; }
    }
    public WeaponPlayer Weapon { get { return this._weapon; } }
    #endregion
    #region Methods
    void Start()
    {
        this._bowAssembleSound = (AudioClip)Resources.Load ("Sounds/Player/BowAssemble");
        this._tank = new TankPlayer();
        this._tank.Start();
        this._weapon = new WeaponPlayer();
        this._weapon.Start();

        this._weapon.Tank = this._tank;
        this._isReadyToShot = true;
    }
    void Update()
    {
        this._tank.Update();

        if (!MenuPause.Paused)
        {
            //Se encontrar um computador, tenta hackear.
            TryFindComputer();

            if (CurrentStatePlayer == StatePlayerType.AIM)
            {
                //som de ativacao do arco
                if(!this._isPlayedBowAssembleSound) {
                    AudioSource.PlayClipAtPoint(this._bowAssembleSound, Camera.main.transform.position,1f);
                    this._isPlayedBowAssembleSound = true;
                }
                if(Input.GetButtonDown(SHOT) && this._isReadyToShot) {
                    animation.Stop("aim");
                    animation.CrossFade("shot");
                    this._weapon.Shot();
                    this._isReadyToShot = false;
                    StartCoroutine(this.ReloadShot());
                }
            }else if(this._isPlayedBowAssembleSound) {
                //cancela som
                this._isPlayedBowAssembleSound = false;
                AudioSource.PlayClipAtPoint(this._bowAssembleSound, Camera.main.transform.position,1f);
                animation.Stop("aim");
            }
            
            if (this._tank.EnergySump > 0)
            {
                if (Input.GetKeyDown(KeyCode.Q)) this._tank.ChoiceSideEnergy = TankType.LIFE;
                else if (Input.GetKeyDown(KeyCode.E)) this._tank.ChoiceSideEnergy = TankType.SHOT;
            }

            if (this._tank.EnergyLife <= 0)
            {
                animation.CrossFade("death");
                animation.wrapMode = WrapMode.ClampForever;
            }
        }
    }
    void OnGUI()
    {
        float height = 120f;
        float widthLabel = 50f;
        float leftLabel = 10f;
        float widthBox = 10f;
        float leftBox = 70f;
        float top = Screen.height - 60f;
        float multiplicator = 5f;
        float space = 30f;

        top -= space;
        GUI.Label(new Rect(leftLabel, top, widthLabel, height), "Sump");
        GUILayout.BeginArea(new Rect(leftBox, top, (this._tank.EnergySump + widthBox) * multiplicator, height));
        GUILayout.Box(this._tank.EnergySump.ToString());
        GUILayout.EndArea();

        top -= space;
        GUI.Label(new Rect(leftLabel, top, widthLabel, height), "Bullets");
        GUILayout.BeginArea(new Rect(leftBox, top, (this._tank.EnergyShot + widthBox) * multiplicator, height));
        GUILayout.Box(this._tank.EnergyShot.ToString());
        GUILayout.EndArea();

        top -= space;
        GUI.Label(new Rect(leftLabel, top, widthLabel, height), "Health");
        GUILayout.BeginArea(new Rect(leftBox, top, (this._tank.EnergyLife + widthBox) * multiplicator, height));
        GUILayout.Box(this._tank.EnergyLife.ToString());
        GUILayout.EndArea();
    }
    #endregion
    #region Methods (Class)
    IEnumerator ReloadShot()
    {
        yield return new WaitForSeconds(this._weapon.ReloadTimeShot);
        this._isReadyToShot = true;
    }
    /// <summary>
    /// Tenta encontrar o computador.
    /// </summary>
    private void TryFindComputer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            Debug.Log(">>> "  + hit.transform.tag);
            
            if (hit.transform.tag.Contains("Computer") ||
                hit.transform.name.Contains("Computer") ||
                hit.transform.gameObject.name.Contains("Computer") ||
                hit.transform.gameObject.tag.Contains("Computer"))
            {
                hit.transform.GetComponent<ComputerManager>().OnHackedBehaviour();
            }
        }
    }
    #endregion
}
