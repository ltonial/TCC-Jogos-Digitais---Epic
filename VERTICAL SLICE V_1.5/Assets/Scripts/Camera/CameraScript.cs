using UnityEngine;

/// <summary>
/// Script que controla o movimento da camera
/// </summary>
public class CameraScript : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Cont�m o GameObject do jogador
    /// </summary>
    private GameObject _objectPlayer;
    /// <summary>
    /// Cont�m o pr�prio transform, ou seja, da camera
    /// </summary>
    private Transform _myTransform;
    /// <summary>
    /// Cont�m o GameObject com o target default da camera
    /// </summary>
    private GameObject _targetCamera;
    /// <summary>
    /// Cont�m o GameObject do target da mira da camera
    /// </summary>
    private GameObject _targetAim;
    /// <summary>
    /// Armazena o GameObject do target atual da camera
    /// </summary>
    private GameObject _currentTarget;
    /// <summary>
    /// x,y,z que controlam o deslocamento da camera com rela��o ao target
    /// </summary>
    private float _xOffset;
    /// <summary>
    /// Distancia da camera com rela��o ao target
    /// </summary>
    private float _currentDistanceCamera;
    /// <summary>
    /// Altura da camera com rela��o ao target
    /// </summary>
    private float _currentHeightCamera;
    /// <summary>
    /// Vari�veis que controlam a distancia e altura da camera com rela��o ao estado do jogador
    /// </summary>
    private float _distCamPlayerWalk;
    private float _distCamPlayerFight;
    private float _distCamAim;
    private float _heightCamPlayerWalk;
    private float _heightCamPlayerFight;
    private float _xOffsetCamPlayerFight;
    private float _xOffsetPlayerWalk;
    /// <summary>
    /// x e y que recebem os valores de movimenta��o do mouse
    /// </summary>
    private float _xMouse;
    private float _yMouse;
    /// <summary>
    /// velocidade x e y da movimenta��o do mouse
    /// </summary>
    private float _xSpeedMouseMov;
    private float _ySpeedMouseMov;
    /// <summary>
    /// Vari�vel que verifica se o bot�o da mira est� pressionado
    /// </summary>
    private bool _aimButtonDown;
    /// <summary>
    /// Suavidade da rota��o da camera
    /// </summary>
    private float _rotationDamping;
    /// <summary>
    /// Suavidade da altera��o da altura da camera
    /// </summary>
    private float _heightDamping;
    /// <summary>
    /// Atingiu o limite de movimenta��o da camera?
    /// </summary>
    private bool _reachedLimitPosMaxCamera;
    /// <summary>
    /// Atingiu o limite de movimenta��o da camera?
    /// </summary>
    private bool _reachedLimitPosMinCamera;
    /// <summary>
    /// Valor inicial para _yMouse, para a camera iniciar com o valor adequado
    /// </summary>
    private float _inicialValorYMouse;
    /// <summary>
    /// Distancia da camera quando o player est� mirando
    /// </summary>
    private float _distCamPlayerAim;
    /// <summary>
    /// Vector com a posi��o m�nima que camera pode se movimentar
    /// </summary>
    private Vector3 _vectorPosMinCamera;
    /// <summary>
    /// Limite m�ximo que o Y do mouse vai at� a camera ficar no limite m�ximo em rela��o ao personagem 
    /// </summary>
    private const float LIMIT_MAX_YMOUSE = 85.0f;
    /// <summary>
    /// Armazena o valor de Y do mouse quando a ele ultrapassa o limite  m�ximo de movimenta��o da camera (LIMIT_MAX_YMOUSE)
    /// </summary>
    float _bufferYMaxMouse;
    /// <summary>
    /// Armazena o valor de Y do mouse quando a ele ultrapassa o limite m�nimo de movimenta��o da camera (LIMIT_MAX_YMOUSE)
    /// </summary>
    float _bufferYMinMouse;
    #endregion
    #region Methods (Inherit)
    void Awake()
    {
        this.Initialize();

        if (_currentTarget.transform == null)
            Debug.Log("We do not have a target");
        else
        {
            this._currentTarget = this._targetCamera;
            CameraSetUp();
        }
    }
    void Update()
    {
        if (_aimButtonDown)
            PlayerManager.CurrentStatePlayer = StatePlayerType.AIM;
    }
    void LateUpdate()
    {
        if (_currentTarget.transform != null)
        {
            //Se o jogador est� em estado AIM a camera muda o comportamento e o target
            if (PlayerManager.CurrentStatePlayer == StatePlayerType.AIM)
            {
                _currentTarget = _targetAim;
                AimingBehaviourCamera();
                _currentDistanceCamera = Mathf.Lerp(_currentDistanceCamera, _distCamPlayerAim, Time.deltaTime * 3);
            }
            //Sen�o volta para o estado inicial que � o WALK
            else
            {
                RotateCameraWithoutButton();
                _currentDistanceCamera = Mathf.Lerp(_currentDistanceCamera, _distCamPlayerWalk, Time.deltaTime * 3);
            }

            //Atualiza os parametros da camera de acordo com a ultima altera��o dos mesmos
            ChangeParametersCamera();
        }
    }
    #endregion
    #region Methods (Class)
    /// <summary>
    /// Recebe a entrada do mouse em PlayerInput e altera a vari�vel que controla se o bot��o da mira est� pressionado
    /// </summary>
    public void ToggleAimButtonDown(bool isButttomDown)
    {
        _aimButtonDown = isButttomDown;
    }
    /// <summary>
    /// Inicializa as vari�veis da c�mera.
    /// </summary>
    private void Initialize()
    {
        this._xOffset = 0f;
        
        this._distCamPlayerWalk = 2f;
        this._distCamPlayerFight = 15f;
        this._distCamPlayerAim = .5f;
        this._currentDistanceCamera = _distCamPlayerWalk;

        this._heightCamPlayerWalk = 10f;
        this._heightCamPlayerFight = 15f;
        this._currentHeightCamera = _heightCamPlayerWalk;

        this._xOffsetPlayerWalk = this._xOffset;
        this._xOffsetCamPlayerFight = 0;

        this._xSpeedMouseMov = 150f;
        this._ySpeedMouseMov = 120f;
        this._aimButtonDown = false;
        this._rotationDamping = 3f;
        this._heightDamping = 2f;
        this._objectPlayer = GameObject.FindGameObjectWithTag("Player");
        this._targetCamera = this._objectPlayer.transform.FindChild("TargetCamera").gameObject;
        this._targetAim = this._objectPlayer.transform.FindChild("AimCamera").gameObject;
        this._myTransform = transform;
        this._currentTarget = this._targetCamera;
        this._reachedLimitPosMaxCamera = false;
        this._vectorPosMinCamera = new Vector3(0,-12.3f,12.5f);
        //Inicia a camera com o angulo adequado que � alterado na fun��o RotateCamera();
        this._inicialValorYMouse = 10f;
        this._yMouse = _inicialValorYMouse;
    }
    /// <summary>
    /// Rotaciona a camera sem o bot�o do mouse, ou seja, quando n�o est� no estado AIM
    /// </summary>
    private void RotateCameraWithoutButton()
    {
        _xMouse += Input.GetAxis("Mouse X") * _xSpeedMouseMov * 0.02f;            //Use the Input Manager to make this a user selected button
        _yMouse -= Input.GetAxis("Mouse Y") * _ySpeedMouseMov * 0.02f;            //Use the Input Manager to make this a user selected button

        if ((!_reachedLimitPosMaxCamera && !_reachedLimitPosMinCamera) ||
            (_yMouse < _bufferYMaxMouse && _yMouse > _bufferYMinMouse))
        {
            _reachedLimitPosMaxCamera = false;
            _reachedLimitPosMinCamera = false;
            RotateCamera();
        }
        else if (_reachedLimitPosMaxCamera)
        {
            _currentDistanceCamera = Mathf.Lerp(_currentDistanceCamera, _distCamPlayerWalk + 3, Time.deltaTime * 3);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (_reachedLimitPosMinCamera)
        {
            _currentDistanceCamera = Mathf.Lerp(_currentDistanceCamera, _distCamPlayerWalk, Time.deltaTime * 3);
            transform.position = new Vector3(transform.position.x, transform.position.y, _vectorPosMinCamera.z);
        }
    }
    /// <summary>
    /// M�todo que faz a movimenta��o da camera quando o jogador est� mirando,
    /// e quando o jogador para de mirar
    /// </summary>
    private void AimingBehaviourCamera()
    {
        if (PlayerManager.CurrentStatePlayer == StatePlayerType.AIM)
        {
            if (_aimButtonDown)
            {
                _xMouse += Input.GetAxis("Mouse X") * _xSpeedMouseMov * 0.02f;
                _yMouse -= Input.GetAxis("Mouse Y") * _ySpeedMouseMov * 0.02f;

                RotatePlayer();
                RotateCamera();
            }
            else
            {
                _yMouse = _inicialValorYMouse;

                float wantedRotationAngle = _currentTarget.transform.eulerAngles.y;
                float wantedHeight = _currentTarget.transform.position.y + _currentHeightCamera;

                float currentRotationAngle = _myTransform.eulerAngles.y;
                float currentHeight = _myTransform.position.y;

                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

                Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

                _myTransform.position = _currentTarget.transform.position;
                _myTransform.position -= currentRotation * Vector3.forward * _currentDistanceCamera;

                _myTransform.position = new Vector3(_myTransform.position.x, currentHeight, _myTransform.position.z);

                //Volta para o estado WALK e seta a camera para o target default
                PlayerManager.CurrentStatePlayer = StatePlayerType.WALK;
                _currentTarget = _targetCamera;
            }
        }
    }
    /// <summary>
    /// Rotaciona a camera em rela��o ao target (pivo)
    /// </summary>
    private void RotateCamera()
    {
     
        Quaternion rotation = Quaternion.Euler(_yMouse, _xMouse, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -_currentDistanceCamera) + _currentTarget.transform.position;
        transform.rotation = rotation;
        float newY = Mathf.Lerp(_myTransform.position.y, position.y, Time.deltaTime);
        transform.position = new Vector3(position.x, newY, position.z);

        if (_yMouse > LIMIT_MAX_YMOUSE)
        {
            _bufferYMaxMouse = _yMouse;
             _reachedLimitPosMaxCamera = true;
            
        }

        //ARRUMAR BUG AQUI
        if (_myTransform.position.y < _vectorPosMinCamera.y &&
            _myTransform.position.z >= _vectorPosMinCamera.z)
        {
            _bufferYMinMouse = _yMouse;
            _reachedLimitPosMinCamera = true;
            
        }

        _myTransform.LookAt(_currentTarget.transform);
    }
    /// <summary>
    /// Rotaciona o player e o target quando o jogador est� em estado AIM (mirando)
    /// </summary>
    private void RotatePlayer()
    {
        Quaternion rotation = Quaternion.Euler(0, _xMouse, 0);
        _objectPlayer.transform.rotation = rotation;
    }
    /// <summary>
    /// Define a c�mera para a posi��o padr�o por tr�s do jogador e de frente para ele (LookAt)
    /// </summary>
    private void CameraSetUp()
    {
        _myTransform.position = new Vector3(_currentTarget.transform.position.x, _currentTarget.transform.position.y + _currentHeightCamera, _currentTarget.transform.position.z - _currentDistanceCamera);
        _myTransform.LookAt(_currentTarget.transform);
        _reachedLimitPosMaxCamera = false;
    }
    /// <summary>
    /// Altera os par�metros da camera de acordo com o estado do jogador
    /// </summary>
    private void ChangeParametersCamera()
    {
        if (PlayerManager.CurrentStatePlayer == StatePlayerType.FIGHT)
        {
            _currentDistanceCamera = Mathf.Lerp(_currentDistanceCamera, _distCamPlayerFight, Time.deltaTime);
            _currentHeightCamera = Mathf.Lerp(_currentHeightCamera, _heightCamPlayerFight, Time.deltaTime);
            _xOffset = Mathf.Lerp(_xOffset, _xOffsetCamPlayerFight, Time.deltaTime);
        }
        else if (PlayerManager.CurrentStatePlayer == StatePlayerType.WALK)
        {
            _currentDistanceCamera = Mathf.Lerp(_currentDistanceCamera, _distCamPlayerWalk, Time.deltaTime);
            _currentHeightCamera = Mathf.Lerp(_currentHeightCamera, _heightCamPlayerWalk, Time.deltaTime);
            _xOffset = Mathf.Lerp(_xOffset, _xOffsetPlayerWalk, Time.deltaTime);
        }
    }
    #endregion
}