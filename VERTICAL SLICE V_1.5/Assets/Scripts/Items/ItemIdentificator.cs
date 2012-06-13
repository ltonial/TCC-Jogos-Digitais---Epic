using UnityEngine;
using System.Collections;

public class ItemIdentificator : MonoBehaviour
{
    public int _id;
    public int ID 
    {
        get { return this._id; }
        set { this._id = value; }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Turret" && gameObject.tag == "Waypoint") collider.GetComponent<TurretManager>().FSM.FoundWaypoint();
    }
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Turret" && gameObject.tag == "Computer") collision.transform.GetComponent<TurretManager>().FSM.FoundComputer();
	}
}