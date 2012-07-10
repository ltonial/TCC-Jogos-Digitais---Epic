using UnityEngine;
using System.Collections;

public class RespawnTurret : MonoBehaviour
{
    /// <summary>
    /// The _time.
    /// </summary>
    private float _time = 0f;
    /// <summary>
    /// Prefab da munição.
    /// </summary>
    private GameObject _turretObject;

    void Start()
    {
        this._turretObject = (GameObject)Resources.Load("Enemies/Turret");
    }

    void Update ()
    {
        this._time += Time.deltaTime;
        if (this._time > 5f)
        {
            GameObject.Instantiate(this._turretObject, transform.position, Quaternion.identity);
        }
    }
}
