using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Challenge1 : MonoBehaviour
{
    #region Attributes
    private List<GameObject> _doorList = new List<GameObject>();
    private List<GameObject> _turretsList = new List<GameObject>();
    private HumanoidManager _firstChallengeScript;
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        GameObject[] auxDoorObjects = GameObject.FindGameObjectsWithTag("Door");
        if (auxDoorObjects != null) this._doorList = auxDoorObjects.ToList();

        GameObject[] auxTurretObjects = GameObject.FindGameObjectsWithTag("Turret");
	    if (auxTurretObjects != null) this._turretsList = auxTurretObjects.ToList();
        this._turretsList.ForEach(t => t.GetComponent<TurretManager>().enabled = false);

        GameObject auxObject = GameObject.Find("HumanoidRed");
        this._firstChallengeScript = auxObject.GetComponent<HumanoidManager>();
    }
    void Update()
    {
        if (this._firstChallengeScript == null || this._firstChallengeScript.Health.IsDead)
            ActiveTurrets();
    }
    #endregion
    #region Methods (Class)
    private void ActiveTurrets()
    {
        this._doorList.ForEach(d => d.active = false);
        this._turretsList.ForEach(t => t.GetComponent<TurretManager>().enabled = true);
        this.enabled = false;
    }
    #endregion
}
