using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Attributes
	/// <summary>
	/// The _my transform.
	/// </summary>
    private Transform _myTransform;
	/// <summary>
	/// The _controller.
	/// </summary>
    private CharacterController _controller;
    /// <summary>
    /// Flags de colis�o que temos a partir do ultimo frame
    /// </summary>
    private CollisionFlags _collision;
    /// <summary>
    /// A dire��o que o jogador est� se movendo
    /// </summary>
    private Vector3 _moveDirection = Vector3.zero;
    /// <summary>
    /// A velocidade que o personagem rotaciona
    /// </summary>
    private float _rotatePlayerSpeed = 250f;
    /// A velocidade que o jogador tem quando est� mirando
    /// </summary>
    private float _aimSpeed = 2f;
    /// <summary>
    /// A velocidade que o jogador caminha
    /// </summary>
    private float _walkSpeed = 4f;
    /// <summary>
    /// A velocidade que o jogador corre
    /// </summary>
    private float _runSpeed = 7f;
    /// <summary>
    /// Valor da gravidade aplicada ao avatar do jogador
    /// </summary>
    private float _gravity = 9.8f;
    /// <summary>
    /// O per�odo de tempo de ca�da que n�s temos antes do sistema saber que � uma ca�da
    /// </summary>
    private float _fallTime = .5f;
    /// <summary>
    /// Verifica se o jogador est� correndo ou n�o
    /// </summary>
    private bool _run;
    /// <summary>
    /// Verifica se o jogador est� se movendo
    /// </summary>
    private bool _isMoving = false;
    /// <summary>
    /// per�odo para travar a camera
    /// </summary>
    private float _lockCameraTimer = 0.0f;
    /// <summary>
    /// Velocidade do movimento do jogador (usada para fazer compara��es em condicionais e as transi��es de velocidade, 
    /// al�m de ser a velocidade final aplicada ao movimento)
    /// </summary>
    private float _moveSpeed = 0.0f;
    /// <summary>
    /// Suaviza a velocidade
    /// </summary>
    private float _speedSmoothing = 10.0f;
    /// <summary>
    /// In�cio do tempo de caminhada
    /// </summary>
    private float _walkTimeStart = 0.0f;
    /// <summary>
    /// Constante que multiplica cada eixo do vetor inAirVelocity para definir a velocidade
    /// </summary>
    private float _inAirControlAcceleration = 3.0f;
    /// <summary>
    /// Enum com a dire��o vertical do jogador
    /// </summary>
    private DirectionType _currentDirection;
    /// <summary>
    /// Enum com a dire��o horizontal do jogador
    /// </summary>
    private TurnType _currentTurn;
    /// <summary>
    /// Estado do jogador
    /// </summary>
    private State _currentState;
    /// <summary>
    /// Verifica se o jogador est� pulando ou n�o
    /// </summary>
    private bool _jumping;
    /// <summary>
    /// O player � control�vel? (para usar na aplica��o da gravidade)
    /// </summary>
    private bool _isControllable = true;
    /// <summary>
    /// Limite do pulo foi alcan�ado?
    /// </summary>
    private bool _jumpingReachedApex = false;
    /// <summary>
    /// Velocidade vertical que � aplicada ao pulo
    /// </summary>
    private float _verticalSpeed = 0.0f;
    /// <summary>
    /// O ultimo tempo registrado ap�s ter pulado
    /// </summary>
    private float _lastJumpTime = -1.0f;
    /// <summary>
    /// Tempo auxiliar para adequar uma condi��o de entrada em Apply Jumping
    /// </summary>
    private float _jumpDumpingTime = 0.05f;
    /// <summary>
    /// O tempo registrado na ultima vez que foi pressionado o bot�o de pulo
    /// </summary>
    private float _lastJumpButtonTime = -10.0f;
    /// <summary>
    /// Tempo limite para o pulo
    /// </summary>
    private float _jumpTimeout = 0.15f;
    /// <summary>
    /// A ultima altura ao iniciar o pulo
    /// </summary>
    private float _lastJumpStartHeight = 0.0f;
    /// <summary>
    /// O ultimo tempo ao estar no ch�o
    /// </summary>
    private float _lastGroundedTime = 0.0f;
    /// <summary>
    /// Velocidade no ar
    /// </summary>
    private Vector3 _inAirVelocity = Vector3.zero;
    /// <summary>
    /// A altura alcan�ada no pulo
    /// </summary>
    private float _jumpHeight = 0.8f;
    /// <summary>
    /// Som do pulo.
    /// </summary>
    private AudioClip _jumpSound;
    /// <summary>
    /// Som de caminhar.
    /// </summary>
    private AudioClip _walkSound;
    #endregion
    #region Methods (Inherit)
    void Awake()
    {
        _moveDirection = transform.TransformDirection(Vector3.forward);
        _myTransform = transform;
        _controller = GetComponent<CharacterController>();
        _currentState = State.INIT;
        this._jumpSound = (AudioClip)Resources.Load ("Sounds/Player/Jump");
        this._walkSound = (AudioClip)Resources.Load ("Sounds/Player/Walk");
    }
    void Update()
    {
        switch (_currentState)
        {
            case State.INIT:
                Initizalize();
                break;
            case State.SETUP:
                Setup();
                break;
            case State.RUN:
                SelectAction();
                break;
        }
    }
    #endregion
    #region Methods (Class)
    /// <summary>
    /// Muda o estado do jogador para ir no m�todo Setup setar algumas variaveis
    /// e sai do m�todo se n�o encontrar os componentes CharacterController e Animation
    /// </summary>
    private void Initizalize()
    {
        if (!GetComponent<CharacterController>()) return;
        if (!GetComponent<Animation>()) return;
        this._currentState = State.SETUP;
    }
    /// <summary>
    /// Alimenta as vari�veis de dire��o para movimentar, pular e as vari�veis de anima��o
    /// </summary>
    private void Setup()
    {
        //animation.wrapMode = WrapMode.Loop;
        //animation["AxeSlash"].layer = 1;
        //animation["AxeSlash"].wrapMode = WrapMode.Once;
        //animation.Play("CombatStance");

        this._currentDirection = DirectionType.NONE;
        this._jumping = false;
        this._run = false;
        this._currentTurn = TurnType.NONE;
        this._currentState = State.RUN;
    }
    /// <summary>
    /// Altera a vari�vel informa a dire��o do movimento vertical, 
    /// � chamada pelo PlayerInput.cs atrav�s de um evento do teclado
    /// </summary>
    /// <param name="z"></param>
    public void MovementVertical(DirectionType z)
    {
        _currentDirection = z;
    }
    /// <summary>
    /// Altera a vari�vel informa a dire��o do movimento horizontal, 
    /// � chamada pelo PlayerInput.cs atrav�s de um evento do teclado
    /// </summary>
    /// <param name="x"></param>
    public void MovementHorizontal(TurnType x)
    {
        _currentTurn = x;
    }
    /// <summary>
    /// Habilita o pulo do jogador 
    /// � chamada pelo PlayerInput.cs atrav�s de um evento do teclado
    /// </summary>
    public void JumpMe()
    {
        _jumping = true;
        _lastJumpButtonTime = Time.time;
    }
    /// <summary>
    /// Muda o movimento do jogador (correr ou caminhar) 
    /// � chamada pelo PlayerInput.cs atrav�s de um evento do teclado
    /// </summary>
    public void ToggleRun()
    {
        _run = !_run;
    }
    /// <summary>
    /// Fun��o que retorna true se o jogador est� se movendo
    /// </summary>
    /// <returns></returns>
    private bool IsMoving()
    {
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
        return isMoving;
    }
    /// <summary>
    /// Realiza toda a movimenta��o do jogador
    /// </summary>
    public void MovePlayer()
    {
        Transform cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        //retorna true se o jogador est� no ch�o e false sen�o est�
        bool grounded = _controller.isGrounded;

        // Vetor direcionado para a frente relativo � c�mera ao longo do plano x-z
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward = new Vector3(forward.x, 0f, forward.z); //forward.y = 0;
        forward = forward.normalized;

        // Vetor apontado para a direita relacionado a camera
        // � sempre ortogonal(perpendicular) ao vetor forward
        Vector3 right = new Vector3(forward.z, 0f, -forward.x);

        //Recebe os valores do movimento vertical e horizontal conforme as teclas s�o pressionadas
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //quarda o estado do movimento atual na vari�vel wasMoving
        bool wasMoving = _isMoving;
        //se foi apertado alguma tecla de movimenta��o isMoving � true
        _isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;

        // Dire��o de destino relativo a c�mera
        Vector3 targetDirection = h * right + v * forward;

        if (grounded)
        {
            // Bloquea c�mera por um curto per�odo quando a ocorre a transi��o de se movendo e parado
            _lockCameraTimer += Time.deltaTime;
            if (_isMoving != wasMoving)
                _lockCameraTimer = 0.0f;

            //N�s armazenamos velocidade e dire��o separadamente,
            //de modo que quando o personagem fica parado ainda temos um sentido v�lido para a frente
            //moveDirection sempre � normalizada, e n�s s� atualizamos se houver a entrada do usu�rio.
            if (targetDirection != Vector3.zero)
            {
                // Se o jogador est� realmente lento, apenas encaixa para a dire��o alvo
                if (_moveSpeed < _walkSpeed * 0.9 && grounded)
                {
                    _moveDirection = targetDirection.normalized;
                }
                // Caso contr�rio, suavemente volta para ele
                else
                {
                    _moveDirection = Vector3.RotateTowards(_moveDirection, targetDirection, _rotatePlayerSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000f);
                    _moveDirection = _moveDirection.normalized;
                }
            }

            //Suaviza a velocidade baseado na dire��o atual do alvo
            float curSmooth = _speedSmoothing * Time.deltaTime;
            //Escolha a velocidade alvo
            //Queremos apoiar a entrada anal�gica, mas certifique-se voc� n�o pode andar mais r�pido na diagonal do que apenas pra frente ou pra lateral
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1f);

            // Escolhe o modificador de velocidade
            // Escolhe o modificador de velocidade
            if (PlayerManager.CurrentStatePlayer == StatePlayerType.AIM)
                targetSpeed *= _aimSpeed;
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                targetSpeed *= _runSpeed;
            else
                targetSpeed *= _walkSpeed;

            _moveSpeed = Mathf.Lerp(_moveSpeed, targetSpeed, curSmooth);
            // Reinicia o in�cio do tempo de caminhada quando desaceleramos
            if (_moveSpeed < _walkSpeed * 0.3f)
                _walkTimeStart = Time.time;

            // Velocidade no ar zerada.
            this._inAirVelocity = Vector3.zero;
        }
        // Controles no ar
        else
        {
            // Bloqueia a camera enquanto estiver no ar
            if (_jumping)
                _lockCameraTimer = 0f;

            if (_isMoving)
                _inAirVelocity += targetDirection.normalized * Time.deltaTime * _inAirControlAcceleration;
        }

        //O vetor final de movimenta��o considera a gravidade, e finalmente realiza a movimenta��o
        //_moveDirection.y -= _gravity * Time.deltaTime;
        Vector3 movement = _moveDirection * _moveSpeed + new Vector3(0f, _verticalSpeed, 0f) + _inAirVelocity;
        // collision recebe todas as colis�es do jogador durante a movimenta��o
        _collision = _controller.Move(movement * Time.deltaTime);


        //Se o jogador est� no ch�o rotaciona o jogador de acordo com o movimento dele
        //Se o jogador est� no ar rotaciona ele verifica se a magnitude do quadrado do vetor que vai orientar a rota��o � maior que a constante .0001f
        //Para comparar a magnitude do vetor com alguma coisa � mais r�pido usar o sqrMAgnitude(magnitude quadrada do vetor) = (x*x+y*y+z*z)
        if (IsGrounded())
        {
            Vector3 auxMove = movement;
            auxMove.y = 0.0f;
            if (auxMove.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(auxMove);
            }
        }
        else
        {
            Vector3 auxMove2 = movement;
            auxMove2.y = 0.0f;
            if (auxMove2.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(auxMove2);
            }
        }

        // Est� em modo de pulo apenas quando esta no ch�o
        if (IsGrounded())
        {
            _lastGroundedTime = Time.time;
            _inAirVelocity = Vector3.zero;
            if (_jumping)
            {
                _jumping = false;

                //toca som do pulo - nao sei se seria o melhor lugar para o audio
                Camera.main.audio.clip = this._jumpSound;
                Camera.main.audio.Play();
            }
        }
    }
    /// <summary>
    /// Seleciona a a��o do jogador
    /// </summary>
    private void SelectAction()
    {
        //Controla o pulo do jogador e chama a fun��o da anima��o correspondente.
        if (_controller.isGrounded)
        {
            //Chama as fun��es das anima��es conforme a dire��o vertical e horizontal
            if (_currentDirection != DirectionType.NONE)
            {
                if (_run)
                {
                    Run();
                }
                else
                {
                    Walk();
                }
            }
            else if (_currentTurn != TurnType.NONE)
            {
                Walk();
            }
            else
            {
                Idle();
            }
        }

        //Aplica a gravidade
        ApplyGravity();

        //Aplica a Pulo
        ApplyJumping();
        //Chama a movimenta��o do jogador
        MovePlayer();
    }
    private void ApplyJumping()
    {
        // Se o tempo do �ltimo salto for maior que o tempo do jogo sai da fun��o
        if (_lastJumpTime + _jumpDumpingTime > Time.time)
            return;

        if (IsGrounded())
        {
            // Jump
            // Incrementa a velocidade vertical (aplicada no pulo)		
            if (Time.time < _lastJumpButtonTime + _jumpTimeout)
            {
                _verticalSpeed = CalculateJumpVerticalSpeed(_jumpHeight);
            }
        }
    }
    private void ApplyGravity()
    {
        if (_isControllable)
        {
            if (_jumping && !_jumpingReachedApex && _verticalSpeed <= 0.0f)
            {
                _jumpingReachedApex = true;
            }

            if (IsGrounded())
                _verticalSpeed = 0.0f;
            else
                _verticalSpeed -= _gravity * Time.deltaTime;
        }
    }
    private void DidJump()
    {
        _jumping = true;
        _jumpingReachedApex = false;
        _lastJumpTime = Time.time;
        _lastJumpStartHeight = transform.position.y;
        _lastJumpButtonTime = -10;
    }
    private bool IsGrounded()
    {
        return (_collision & CollisionFlags.CollidedBelow) != 0;
    }
    private float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * targetJumpHeight * _gravity);
    }
    /// <summary>
    /// Fun��es que apenas chamam as anima��es do movimento do jogador
    /// </summary>
    #endregion
    #region Methods (Animations)
    public void Fall()
    {
        //Debug.Log("Fall animation here");
        //animation.CrossFade("fall");
    }
    public void Jump()
    {

        //animation.CrossFade("AxeSlash");
    }
    public void Walk()
    {
        //som da caminhada
//        Camera.main.audio.clip = this._walkSound;
//        Camera.main.audio.Play();
        //animation.CrossFade("GoodWalk");
    }
    public void Run()
    {
        //Debug.Log("Run animation here");
        //animation.CrossFade("GoodWalk");
    }
    public void Idle()
    {
        //Debug.Log("Idle animation here");
        //animation.CrossFade("CombatStance");     //anima��o idle aqui
    }
    #endregion
}