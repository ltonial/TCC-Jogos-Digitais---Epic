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
        this._challengeList.ForEach(c => c.active = false);

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
            {
                this._challengeList.ForEach(c => c.active = true);
                this._turretsList.ForEach(t => t.GetComponent<TurretManager>().enabled = true);
                this.enabled = false;
                //Debug.Log("CHALLENGE 1 FINISHED");
                SwitchCameras._activeCameraPit = true;
            }
		}
    }
    #endregion
}
