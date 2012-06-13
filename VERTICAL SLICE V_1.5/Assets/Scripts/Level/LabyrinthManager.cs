using UnityEngine;
using System.Collections;

public class LabyrinthManager : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Angulo de rotação do anel
    /// </summary>
    private const float ANGLE = 90.0f;
    /// <summary>
    /// GameObject do anel que será rotacionado
    /// </summary>
    private GameObject _ringLabyrinth;
    /// <summary>
    /// GameObject do player
    /// </summary>
    private GameObject _player;
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
    /// Distancia mínima para o player conseguir ativar o computador
    /// </summary>
    private float _distMinToActiveRotateOnComputer;
	/// <summary>
	/// The name of the _ring.
	/// </summary>
	public string _ringName;
    #endregion
    #region Mathods (Inherit)
    void Start()
    {
        //Nome do anel a ser rotacionado
        _ringLabyrinth = GameObject.Find(this._ringName);   
        _player = GameObject.Find("Player");
        _activeRotate = false;
        _velocityRotate = 10f;
        _currentAngle = 0;
        _distMinToActiveRotateOnComputer = 0.8f;
    }
    void Update()
    {
        Vector3 dir = (_player.transform.position - this.transform.position).normalized;
        //Verifica se o player está de frente para o computador ou de costas: 
        //Vector3.Dot retorna um valor > 0 se o player está de frente para o computador
        float direction = Vector3.Dot(dir, transform.forward);

        float distance = Vector3.Distance(_player.transform.position, this.transform.position);

        if (distance < _distMinToActiveRotateOnComputer && Input.GetButtonDown("Fire1"))
            _activeRotate = true;

        if (_activeRotate)
        {
            _ringLabyrinth.transform.Rotate(Vector3.up * Time.deltaTime * _velocityRotate, Space.World);
            _currentAngle += Time.deltaTime * _velocityRotate;
        }

        if (_currentAngle > ANGLE)
        {
            _currentAngle = 0;
            _activeRotate = false;
        }
    }
    #endregion
}
