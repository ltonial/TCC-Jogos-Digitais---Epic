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
    private GameObject[] _turrets;
    private bool _punchHit = false;
    private bool _doublePunchHit = false;
    private bool _godsHandHit = false;
    private bool _punchHitOnGround = false;
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        this._player = GameObject.FindWithTag("Player");
        this._turrets = GameObject.FindGameObjectsWithTag("Turret");
    }
    void Update()
    {
        CheckAttack();
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Humanoid")
        {
            if (_punchHit)
            {
                collider.gameObject.GetComponent<HumanoidManager>().Health.UpdateHealth(-1 * (int)ComboType.PUNCH);
                _punchHit = false;
            }
            if (_doublePunchHit)
            {
                collider.gameObject.GetComponent<HumanoidManager>().Health.UpdateHealth(-1 * (int)ComboType.DOUBLEPUNCH);
                _doublePunchHit = false;
            }
            if (_godsHandHit)
            {
                collider.gameObject.GetComponent<HumanoidManager>().Health.UpdateHealth(-1 * (int)ComboType.GODSHAND);
                _godsHandHit = false;
            }
        }

        if (collider.tag == "Turret")
        {
            if (_punchHitOnGround)
            {
                collider.gameObject.GetComponent<TurretManager>().Health.UpdateHealth(-1 * (int)ComboType.PUNCH);
                _punchHitOnGround = false;
            }
        }
    }

    private void Attack(ComboType combo)
    {
        foreach (GameObject turret in _turrets)
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
                    this._punchHitOnGround = true;
                    this._punchHit = false;
                    this._doublePunchHit = false;
                    this._godsHandHit = false;
                }
            }

        }

        if (combo == ComboType.PUNCH && !_player.animation.IsPlaying("gods_hand") && !_punchHitOnGround)
        {
            this._punchHit = true;
            this._doublePunchHit = false;
            this._godsHandHit = false;
            _player.animation.CrossFade("punch");
        }
        if (combo == ComboType.DOUBLEPUNCH && !_player.animation.IsPlaying("gods_hand"))
        {
            this._punchHit = false;
            this._doublePunchHit = true;
            this._godsHandHit = false;
            _player.animation.CrossFade("double_punch");
        }
        if (combo == ComboType.GODSHAND)
        {
            this._punchHit = false;
            this._doublePunchHit = false;
            this._godsHandHit = true;
            _player.animation.CrossFade("gods_hand");
        }
    }

    #endregion
}