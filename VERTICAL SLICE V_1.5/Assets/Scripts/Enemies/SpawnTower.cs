using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnTower : MonoBehaviour {

    #region Attributes
    private List<GameObject> _listaChallengeFinal = new List<GameObject>();
    private GameObject _enemyObject;
    #endregion
    #region Methods
	void Start ()
    {
        this._enemyObject = (GameObject)Resources.Load("Enemies/TowerFinal");

        this._listaChallengeFinal.Add(GameObject.Find("Disco_1"));
        this._listaChallengeFinal.Add(GameObject.Find("Disco_2"));
        this._listaChallengeFinal.Add(GameObject.Find("Disco_3"));
        Debug.Log(this._listaChallengeFinal.Count);
	}
	void Update ()
    {
        if (!MenuPause.Paused)
        {
            foreach (GameObject item in this._listaChallengeFinal)
            {
                Vector3 world = transform.TransformPoint(transform.position);
                //Debug.Log(">> " + item.transform.tag + " :: " + world.y);

                if (item.transform.position.y == 0)
                {
                    foreach (Transform child in item.transform)
                    {
                        child.renderer.material.color = Color.blue;
                    }
                }
                else
                {
                    foreach (Transform child in item.transform)
                    {
                        child.renderer.material.color = Color.gray;
                    }
                }
            }

            bool ok = this._listaChallengeFinal.All(c => c.transform.position.y == 0);
            if (ok)
            {
                GameObject auxSpawn = (GameObject)Instantiate(this._enemyObject, transform.position, Quaternion.identity);
            }
        }
	}
    #endregion
}
