using UnityEngine;
using System.Collections;

public class SmokeTurretDeath : MonoBehaviour 
{
    /// <summary>
    /// The _item turret.
    /// </summary>
    //private GameObject _energyItem;
    /// <summary>
    /// Prefab da munição.
    /// </summary>
    private GameObject _turretObject;
    /// <summary>
    /// Variável para acessar o scrip do PlayerCombat
    /// </summary>
    private PlayerCombat _scriptPlayerCombat;

    void Start()
	{
        this._turretObject = (GameObject)Resources.Load("Enemies/Turret");
        //this._energyItem = (GameObject)Resources.Load("Items/EnergyItem");
        StartCoroutine(this.Respawn());
    }
    IEnumerator Respawn()
	{
        yield return new WaitForSeconds(15f);
        Instantiate(this._turretObject,this.transform.position,Quaternion.identity);
        this.GetComponent<ParticleEmitter>().minSize=0f;
        this.GetComponent<ParticleEmitter>().maxSize=0f;
        Destroy(this.gameObject, 10f);
    }
}
