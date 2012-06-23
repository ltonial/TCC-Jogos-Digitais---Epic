using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Challenge3 : MonoBehaviour
{
    #region Constants
    private const int COMPUTERS_NEED_HACK = 2;
    private const float MAXANGLE = 90f;
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
    #endregion
    #region Methods (Inherit)
    void Start ()
    {
        this._activeRotate = true;
        this._velocityRotate = 10f;
        this._currentAngle = 0f;

        this._challenge = GameObject.FindGameObjectWithTag("Challenge3");

        GameObject[] auxTerminals = GameObject.FindGameObjectsWithTag("ComputerTurnDoor");
        foreach (GameObject item in auxTerminals) {
            this._terminalList.Add (item.GetComponent<ComputerManager>());
        }
    }
    void Update ()
    {
        if (!MenuPause.Paused)
        {
            int computersHacked = this._terminalList.Count(t => t.WasHacked);
            if (computersHacked == COMPUTERS_NEED_HACK)
                this._activeRotate = true;

            if (this._activeRotate)
            {
                this._challenge.transform.Rotate(Vector3.up * Time.deltaTime * _velocityRotate, Space.World);
                this._currentAngle += Time.deltaTime * _velocityRotate;

                if (this._currentAngle > MAXANGLE)
                {
                    this._currentAngle = 0f;
                    this._activeRotate = false;
                }
            }
        }
    }
    #endregion
}
