using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Challenge3 : MonoBehaviour
{
    #region Constants
    private const int COMPUTERS_NEED_HACK = 2;
    private const float MAXANGLE = 45f;
    #endregion
    #region Attributes
    private GameObject _challenge;
    private List<ComputerManager> _terminalList = new List<ComputerManager>();
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
    /// The _list computer.
    /// </summary>
    private List<ComputerManager> _listComputer = new List<ComputerManager>();
    /// <summary>
    /// Bool para mostrar a câmera que gira o anel
    /// </summary>
    private bool _showCamera;
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        //this._activeRotate = true;
        this._velocityRotate = 10f;
        this._currentAngle = 0f;
        this._showCamera = true;

        this._challenge = GameObject.FindGameObjectWithTag("Challenge3");
        //this._challenge = GameObject.Find("Anel_3");

        GameObject[] auxTerminals = GameObject.FindGameObjectsWithTag("ComputerTurnRing");
        foreach (GameObject item in auxTerminals)
        {
            this._terminalList.Add(item.GetComponent<ComputerManager>());
        }
    }
    void Update()
    {
        if (!MenuPause.Paused)
        {
            //int computersHacked = this._terminalList.Count(t => t.WasHacked);
            //Debug.Log("HACKED: " + computersHacked);
            //if (computersHacked == COMPUTERS_NEED_HACK)
            //this._activeRotate = true;

            if (this._activeRotate)
            {
                this._challenge.transform.Rotate(Vector3.up * Time.deltaTime * _velocityRotate, Space.World);
                this._currentAngle += Time.deltaTime * _velocityRotate;
                if (_showCamera)
                {
                    SwitchCameras._activeCameraRing = true;
                    _showCamera = false;
                }

                if (this._currentAngle > MAXANGLE)
                {
                    Debug.Log("CHALLENGE 3 FINISHED");
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
