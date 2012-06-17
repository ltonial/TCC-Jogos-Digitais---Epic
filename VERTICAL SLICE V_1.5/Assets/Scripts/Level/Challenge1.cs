using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Challenge1 : MonoBehaviour
{
    #region Attributes
    private List<GameObject> _challengeList = new List<GameObject>();
    private List<GameObject> _turretsList = new List<GameObject>();
    private HumanoidManager _firstHumanoidManager;
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        GameObject[] auxObjects = GameObject.FindGameObjectsWithTag("Challenge1");
        if (auxObjects != null) this._challengeList = auxObjects.ToList();

        GameObject[] auxTurretObjects = GameObject.FindGameObjectsWithTag("Turret");
	    if (auxTurretObjects != null) this._turretsList = auxTurretObjects.ToList();
        this._turretsList.ForEach(t => t.GetComponent<TurretManager>().enabled = false);

        GameObject auxObject = GameObject.Find("HumanoidRed");
        this._firstHumanoidManager = auxObject.GetComponent<HumanoidManager>();
    }
    void Update()
    {
		if (!MenuPause.Paused)
		{
        if (this._firstHumanoidManager == null || this._firstHumanoidManager.Health.IsDead)
            ActiveTurrets();
		}
    }
    #endregion
    #region Methods (Class)
    private void ActiveTurrets()
    {
        this._challengeList.ForEach(c => c.active = false);
        this._turretsList.ForEach(t => t.GetComponent<TurretManager>().enabled = true);
        this.enabled = false;
    }
    #endregion
}
