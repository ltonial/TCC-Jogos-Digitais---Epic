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
	private CombatCombo puch = new CombatCombo (new string[] { PUNCH });
	private CombatCombo doublePunch = new CombatCombo (new string[] { PUNCH, PUNCH });
	private CombatCombo godsHand = new CombatCombo (new string[] { PUNCH, PUNCH, PUNCH });
    private const string METHOD_COMBAT_ANIMATIONS = "CombatAnimations";
	public static List<HumanoidManager> HumanoidsList;
    #endregion
    #region Methods (Inherit)
	void Start ()
	{
		HumanoidsList = new List<HumanoidManager>();
		
		GameObject[] auxHumanoidObjects = GameObject.FindGameObjectsWithTag("Humanoid");
		if (auxHumanoidObjects != null) {
			foreach (GameObject item in auxHumanoidObjects) {
				HumanoidsList.Add(item.GetComponent<HumanoidManager> ());
			}
		}
		else Debug.LogError("NAO HÁ PEAO NO CENARIO!");
	}
	void Update ()
	{
		AnalizeDistance();
		CheckAttack();
	}
	void OnDrawGizmos ()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, DISTANCETOFIGHT);
	}
	#endregion
	#region Methods (Class)
    private void AnalizeDistance()
	{
		if (PlayerManager.CurrentStatePlayer != StatePlayerType.AIM) 
		{
			bool nearOfPlayer = false;
			StatePlayerType lastState = PlayerManager.CurrentStatePlayer;
			foreach (HumanoidManager humanoid in HumanoidsList.Where(h => h.name != "HumanoidRed")) 
			{
				float distance = Vector3.Distance (humanoid.transform.position, transform.position);
				if (distance <= DISTANCETOFIGHT) { 
					nearOfPlayer = true; 
					break;
				}
			}
			PlayerManager.CurrentStatePlayer = nearOfPlayer ? StatePlayerType.FIGHT : lastState;
		}
	}
	private void CheckAttack()
	{
		if (PlayerManager.CurrentStatePlayer != StatePlayerType.AIM)
		{
            if (puch.Check())
                Attack(ComboType.PUNCH);
			if (doublePunch.Check ())
				Attack (ComboType.DOUBLEPUNCH);
			if (godsHand.Check ())
				Attack (ComboType.GODSHAND);
		}
	}
	private void Attack (ComboType combo)
	{
        if (combo == ComboType.PUNCH)
            animation.CrossFade("punch");
        else if (combo == ComboType.DOUBLEPUNCH)
            animation.CrossFade("double_punch");
        else if (combo == ComboType.GODSHAND)
            animation.CrossFade("gods_hand");
        //SendMessage(METHOD_COMBAT_ANIMATIONS, combo);

		foreach (HumanoidManager humanoid in HumanoidsList) {
			float distance = Vector3.Distance (humanoid.MyTransform.position, transform.position);
			Vector3 dir = (humanoid.MyTransform.position - transform.position).normalized;

			//Verifica se o player está de frente para o inimigo ou de costas: 
			//Vector3.Dot retorna um valor > 0 se o player está de frente para o inimigo.
			float direction = Vector3.Dot (dir, transform.forward);
			
			//Se satisfizer a distância e direção, desconta a vida do inimigo.
			if (distance < 0.8f && direction > 0f)
				humanoid.Health.UpdateHealth (-1 * (int)combo);
			
		}
	}
    #endregion
}