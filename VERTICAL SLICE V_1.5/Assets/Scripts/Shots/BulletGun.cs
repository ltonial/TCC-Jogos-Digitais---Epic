using UnityEngine;
using System.Collections;

public class BulletGun : MonoBehaviour
{
    #region Attributes
    private const int BULLETDAMAGE = 1;
	private const float VELOCITYSHOT = 10f;
    private const float TIMETODESTROY = 2f;
    private float _velocity;
    #endregion
    #region Methods
    void Start()
    {
        this._velocity = VELOCITYSHOT;
    }
    void Update()
    {
        transform.Translate(Vector3.forward * this._velocity * Time.deltaTime);
        Destroy(gameObject, TIMETODESTROY);
    }
	void OnTriggerEnter(Collider collider) 
	{
		Debug.Log("Trigger: " + collider.tag);
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerManager>().Tank.EnergyLife -= BULLETDAMAGE;
            Destroy(gameObject);
        }
    }
    #endregion
}