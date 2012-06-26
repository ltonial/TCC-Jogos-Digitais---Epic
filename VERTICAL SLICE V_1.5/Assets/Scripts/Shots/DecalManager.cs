using UnityEngine;
using System.Collections;

public class DecalManager : MonoBehaviour {
    /// <summary>
    /// The _time to destroy.
    /// </summary>
    private float _timeToDestroy;

    void Start () {
        this._timeToDestroy = 5.0f;
    }

    void Update () {
        if(this._timeToDestroy > 0.0F){
            this._timeToDestroy -= Time.deltaTime;
        }else {
            Destroy(gameObject);
        }
    }
}
