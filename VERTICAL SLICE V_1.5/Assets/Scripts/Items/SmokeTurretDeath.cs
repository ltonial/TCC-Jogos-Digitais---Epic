using UnityEngine;
using System.Collections;

public class SmokeTurretDeath : MonoBehaviour 
{
    /// <summary>
    /// The _item turret.
    /// </summary>
    private GameObject _energyItem;
    void Start()
	{
        this._energyItem = (GameObject)Resources.Load("Items/EnergyItem");
        StartCoroutine(this.InstanciarItem());
    }
    IEnumerator InstanciarItem() 
	{
        yield return new WaitForSeconds(3f);
        Instantiate(this._energyItem,this.transform.position,Quaternion.identity);
        this.GetComponent<ParticleEmitter>().minSize=0f;
        this.GetComponent<ParticleEmitter>().maxSize=0f;
        Destroy(this.gameObject, 10f);
    }
}
