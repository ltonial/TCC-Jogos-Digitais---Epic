using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Challenge4 : MonoBehaviour
{
    #region Constants
    private const float MAXANGLE = 45f;
    #endregion
    #region Attributes
    private GameObject _challenge;
    /// <summary>
    /// Ativa a rotação do anel?
    /// </summary>
    private bool _activeRotate;
    /// <summary>
    /// Angulo atual, se for maior que 90º o anel para de girar
    /// </summary>
    private float _currentAngle;
    /// <summary>
    /// Velocidade da rotação do anel
    /// </summary>
    private float _velocityRotate;
    /// <summary>
    /// The name of the ring.
    /// </summary>
    public string RingName;
    /// <summary>
    /// The _list computer.
    /// </summary>
    private List<ComputerManager> _listComputer = new List<ComputerManager>();
    /// <summary>
    /// The arrows.
    /// </summary>
    private List<GameObject> arrows = new List<GameObject>();
    #endregion
    #region Methods (Inherit)
    void Start ()
    {
        this._velocityRotate = 10f;
        this._currentAngle = 0f;

        this._challenge = GameObject.FindGameObjectWithTag(this.RingName);

        //GameObject[] aux = gameObject.transform.fi
    }
    void Update ()
    {
        if (!MenuPause.Paused)
        {
            if (this._activeRotate)
            {
                this._challenge.transform.Rotate(Vector3.up * Time.deltaTime * _velocityRotate, Space.World);
                this._currentAngle += Time.deltaTime * _velocityRotate;

                if (this._currentAngle > MAXANGLE)
                {
                    this._currentAngle = 0f;
                    this._activeRotate = false;
                    this._listComputer.ForEach(c => c.OnUnhackedBehaviour());
                }
            }
        }
    }
    /// <summary>
    /// Actives the rotate.
    /// </summary>
    /// <param name='pComputer'>
    /// P computer.
    /// </param>
    public void ActiveRotate(ComputerManager pComputer)
    {
        this._activeRotate = true;
        this._listComputer.Add(pComputer);
    }
    #endregion
}
