using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingManager : MonoBehaviour
{
    #region Attributes
    private List<Transform> _listaRings = new List<Transform>();
    private float _positionY;
    #endregion
    #region Methods
    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start ()
    {
        foreach (Transform child in transform)
        {
            this._listaRings.Add(child);
        }
    }
    void Update ()
    {

    }
    public void ChangeColor(Color newColor)
    {
        this._listaRings.ForEach(r => r.renderer.material.color = newColor);
    }
    public void Turn()
    {
    }
    #endregion
}
