using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controla o combate do Player
/// </summary>
public class PlayerCombat : MonoBehaviour
{
    #region Constants
    private const float DISTANCETOFIGHT = 5f;
    #endregion
    #region Attributes
    private const string PUNCH = "Fire1";
    private CombatCombo puch = new CombatCombo(new string[] { PUNCH });
    private CombatCombo doublePunch = new CombatCombo(new string[] { PUNCH, PUNCH });
    private CombatCombo godsHand = new CombatCombo(new string[] { PUNCH, PUNCH, PUNCH });
    private GameObject _player;
    private GameObject[] auxTurrets;
    private List<GameObject> _turrets;
    public static List<GameObject> _humanoids;
    private bool _punchHit = false;
    private bool _doublePunchHit = false;
    private bool _godsHandHit = false;
    private bool _punchHitOnGround = false;
    private GameObject _particlePunchGround;
    private GameObject _particleGodsHand;
    private int _numberHit;
    #endregion
    #region Methods (Inherit)
    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        this._particlePunchGround = (GameObject)Resources.Load("Items/LightningPunchGround");
        this._particleGodsHand = (GameObject)Resources.Load("Items/LightningGodsHand");
        this._player = GameObject.FindWithTag("Player");
        _humanoids = new List<GameObject>();
        _turrets = new List<GameObject>();
        auxTurrets = GameObject.FindGameObjectsWithTag("Turret");
        for (int i = 0; i < auxTurrets.Length; i++)
        {
            _turrets.Add(auxTurrets[i]);
        }
    }
    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        CheckDistanceHumanoids();

        UpdateListTurrets();

        CheckAttack();
    }

    private void CheckDistanceHumanoids()
    {
        GameObject[] auxHumanoids = GameObject.FindGameObjectsWithTag("Humanoid");

        foreach (GameObject humanoid in auxHumanoids)
        {
            if (!_humanoids.Contains(humanoid))
                _humanoids.Add(humanoid);
        }

        StatePlayerType stateBeforeChecking = PlayerManager.CurrentStatePlayer;

        foreach (GameObject humanoid in _humanoids)
        {
            if (humanoid != null)
            {
                float distance = Vector3.Distance(humanoid.transform.position, _player.transform.position);
                if (distance < 3f)
                    PlayerManager.CurrentStatePlayer = StatePlayerType.FIGHT;
                else
                    PlayerManager.CurrentStatePlayer = stateBeforeChecking;
            }
        }
    }

    private void UpdateListTurrets()
    {
        auxTurrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject turret in auxTurrets)
        {
            if (!_turrets.Contains(turret))
                _turrets.Add(turret);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DISTANCETOFIGHT);
    }
    #endregion
    #region Methods (Class)

    private void CheckAttack()
    {
        if (PlayerManager.CurrentStatePlayer != StatePlayerType.AIM)
        {
            if (puch.Check())
                Attack(ComboType.PUNCH);
            if (doublePunch.Check())
                Attack(ComboType.DOUBLEPUNCH);
            if (godsHand.Check())
                Attack(ComboType.GODSHAND);
        }
    }
    /// <summary>
    /// Raises the trigger enter event.
    /// </summary>
    /// <param name='collider'>
    /// Collider.
    /// </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Humanoid")
        {
            _numberHit++;
            collider.gameObject.GetComponent<HumanoidManager>().Health.UpdateHealth(-1 * (int)ComboType.PUNCH);

            if (_player.animation.IsPlaying("gods_hand") && _numberHit == 2 && transform.name == "RightHandCombat")
            {
                GameObject.Instantiate(this._particleGodsHand, this.transform.position, Quaternion.identity);
            }
            if (collider.gameObject.GetComponent<HumanoidManager>().Health.CurrentHealth <= 0)
            {
                _player.animation.Stop("punch");
                _player.animation.Stop("double_punch");
                _player.animation.Stop("gods_hand");
                _humanoids.Remove(collider.gameObject);
            }

        }

        if (collider.tag == "Turret")
        {
            if (_punchHitOnGround)
            {
                collider.gameObject.GetComponent<TurretManager>().Health.UpdateHealth(-1 * (int)ComboType.PUNCH);
                _punchHitOnGround = false;

                if ( collider.gameObject.GetComponent<TurretManager>().Health.CurrentHealth <= 0)
                    _turrets.Remove(collider.gameObject);
            }
        }
    }
    /// <summary>
    /// Attack the specified combo.
    /// </summary>
    /// <param name='combo'>
    /// Combo.
    /// </param>
    private void Attack(ComboType combo)
    {
        
        foreach (GameObject turret in _turrets)
        {
            if (turret != null)
            {
                float distance = Vector3.Distance(turret.transform.position, _player.transform.position);
                Vector3 dir = (turret.transform.position - _player.transform.position).normalized;

                //Verifica se o player está de frente para o inimigo ou de costas: 
                //Vector3.Dot retorna um valor > 0 se o player está de frente para o inimigo.
                float direction = Vector3.Dot(dir, _player.transform.forward);

                //Se satisfizer a distância e direção, desconta a vida do inimigo.
                if (distance < 3.0f && direction > 0f)
                {
                    if (combo == ComboType.PUNCH && !_player.animation.IsPlaying("gods_hand"))
                    {
                        _player.animation.CrossFade("punch_on_the_ground");
                        GameObject.Instantiate(this._particlePunchGround, _player.transform.position, Quaternion.identity);
                        this._punchHitOnGround = true;
                        turret.gameObject.GetComponent<TurretManager>().Health.UpdateHealth(-1 * (int)ComboType.PUNCH);

                        if (turret.gameObject.GetComponent<TurretManager>().Health.CurrentHealth <= 0)
                        {
                            _turrets.Remove(turret.gameObject);
                            break;
                        }
                    }
                }

            }
        }

        if (combo == ComboType.PUNCH && !_player.animation.IsPlaying("gods_hand") && !_punchHitOnGround)
        {
            _player.animation.CrossFade("punch");
            _numberHit = 0;
        }
        if (combo == ComboType.DOUBLEPUNCH && !_player.animation.IsPlaying("gods_hand"))
        {
            _player.animation.CrossFade("double_punch");
            _numberHit = 0;
        }
        if (combo == ComboType.GODSHAND)
        {
            _player.animation.CrossFade("gods_hand");
            _numberHit = 0;
        }
        
    }
    #endregion
}