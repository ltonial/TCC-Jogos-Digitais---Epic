using UnityEngine;
using System.Collections;

public class CameraAction : MonoBehaviour
{
    #region Attributes
    private GameObject _target;
    private const string CAMERA_TAG_NAME = "TargetCamera";
    private const string PLAYER_TAG_NAME = "Player";
    private const float REACHED_MIN_Y_MOUSE = -70.0f;
    private const float REACHED_MAX_Y_MOUSE = 60.0f;
    private float _currentDistance;
    private float _walkDistance;
    private float _fightDistance;
    private float _currentHeight;
    private float _walkHeight;
    private float _fightHeight;
    private float _xSpeedMouseMov;
    private float _ySpeedMouseMov;
    private float _heightDamping;
    private float _rotationDamping;
    private Transform _myTransform;
    private GameObject _gameObjectPlayer;
    private float _xMouse;
    private float _yMouse;
    private float _xOffsetTarget;
    private float _yOffsetTarget;
    private bool _camButtonRotatePressed;
    private bool _aimButtonDown;
    private bool _canMoveCamera = true;
    private StatePlayerType _previousState;
    #endregion
    #region Methods (Inherit)
    void Start()
    {
        this._xOffsetTarget = 0.5f;
        this._yOffsetTarget = 1f;
        this._walkDistance = 1f;
        this._fightDistance = 3f;
        this._walkHeight = 0.3f;
        this._fightHeight = 2.5f;
        this._xSpeedMouseMov = 250.0f;
        this._ySpeedMouseMov = 120.0f;
        this._heightDamping = 2.0f;
        this._rotationDamping = 3.0f;
        this._camButtonRotatePressed = false;
        this._aimButtonDown = false;
        this._gameObjectPlayer = GameObject.FindGameObjectWithTag(PLAYER_TAG_NAME);
        this._myTransform = transform;
        this._currentDistance = _walkDistance;
        this._currentHeight = _walkHeight;
        this._target = GameObject.FindGameObjectWithTag(CAMERA_TAG_NAME);
        this._target.transform.position = new Vector3(_target.transform.position.x + _xOffsetTarget,
            _target.transform.position.y + this._yOffsetTarget,
            _target.transform.position.z);

        if (_target == null)
            Debug.LogError("NÃO HÁ UM TARGET PARA A CAMERA");
        else
        {
            _myTransform.position = new Vector3(_target.transform.position.x, _target.transform.position.y + _currentHeight, _target.transform.position.z - _currentDistance);
            _myTransform.LookAt(_target.transform);
        }
    }
    void Update()
    {
        this.ChangeParametersCamera();

        if (PlayerManager.CurrentStatePlayer != StatePlayerType.AIM) this._previousState = PlayerManager.CurrentStatePlayer;
        PlayerManager.CurrentStatePlayer = this._aimButtonDown ? StatePlayerType.AIM : this._previousState;
    }
    void LateUpdate()
    {
        this._xMouse += Input.GetAxis("Mouse X") * this._xSpeedMouseMov * .02f;
        this._yMouse -= Input.GetAxis("Mouse Y") * this._ySpeedMouseMov * .02f;

        if (this._yMouse < REACHED_MIN_Y_MOUSE)
            this._yMouse = REACHED_MIN_Y_MOUSE;
        if (this._yMouse > REACHED_MAX_Y_MOUSE)
            this._yMouse = REACHED_MAX_Y_MOUSE;

        RotatePlayer();
        RotateCamera();
    }
    #endregion
    #region Methods (Class)
    private void RotateCamera()
    {
        Quaternion rotation = Quaternion.Euler(_yMouse, _xMouse, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -_currentDistance) + _target.transform.position;
        this._myTransform.rotation = rotation;
        this._myTransform.position = new Vector3(position.x, position.y + _currentHeight, position.z);
        this._myTransform.LookAt(_target.transform);
    }
    private void RotatePlayer()
    {
        Quaternion rotation = Quaternion.Euler(0, _xMouse, 0);
        this._gameObjectPlayer.transform.rotation = rotation;
    }
    public void ToggleAimButtonDown(bool isButttomDown)
    {
        this._aimButtonDown = isButttomDown;
    }
    /// <summary>
    /// Altera os parâmetros da camera de acordo com o estado do jogador
    /// </summary>
    private void ChangeParametersCamera()
    {
        switch (PlayerManager.CurrentStatePlayer)
        {
            case StatePlayerType.FIGHT:
                _currentDistance = Mathf.Lerp(_currentDistance, _fightDistance, Time.deltaTime);
                _currentHeight = Mathf.Lerp(_currentHeight, _fightHeight, Time.deltaTime);
                break;
            default:
                _currentDistance = Mathf.Lerp(_currentDistance, _walkDistance, Time.deltaTime);
                _currentHeight = Mathf.Lerp(_currentHeight, _walkHeight, Time.deltaTime);
                break;
        }
    }
    #endregion
}
