using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Challenge2 : MonoBehaviour
{
	#region Constants
	private const int COMPUTERS_NEED_HACK = 4;
	#endregion
    #region Attributes
	private List<GameObject> _challengeList = new List<GameObject>();
	private List<ComputerManager> _terminalList = new List<ComputerManager>();
    #endregion
    #region Methods (Inherit)
    void Start()
	{
		GameObject[] auxObjects = GameObject.FindGameObjectsWithTag("Challenge2");
        if (auxObjects != null) this._challengeList = auxObjects.ToList();
        this._challengeList.ForEach(c => c.active = false);
		
		GameObject[] auxTerminals = GameObject.FindGameObjectsWithTag("Computer");
		foreach (GameObject item in auxTerminals) 
		{
			this._terminalList.Add(item.GetComponent<ComputerManager>());
		}
	}
	void Update()
	{
		if (!MenuPause.Paused)
		{
			int computersHacked = this._terminalList.Count(t => t.WasHacked);
			if (computersHacked == COMPUTERS_NEED_HACK)
			{
				this._challengeList.ForEach(c => c.active = true);
                //Debug.Log("CHALLENGE 2 FINISHED");
                SwitchCameras._activeCameraStairsLeft = true;
                this.enabled = false;
			}
		}
	}
    #endregion
}
