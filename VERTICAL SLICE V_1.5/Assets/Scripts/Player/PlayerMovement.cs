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
    /// Flags de colisão que temos a partir do ultimo frame
    /// </summary>
    private CollisionFlags _collision;
    /// <summary>
    /// A direção que o jogador está se movendo
    /// </summary>
    private Vector3 _moveDirection = Vector3.zero;
    /// <summary>
    /// A velocidade que o personagem rotaciona
    /// </summary>
    private float _rotatePlayerSpeed = 250f;
    /// A velocidade que o jogador tem quando está mirando
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
    /// O período de tempo de caída que nós temos antes do sistema saber que é uma caída
    /// </summary>
    private float _fallTime = .5f;
    /// <summary>
    /// Verifica se o jogador está correndo ou não
    /// </summary>
    private bool _run;
    /// <summary>
    /// Verifica se o jogador está se movendo
    /// </summary>
    private bool _isMoving = false;
    /// <summary>
    /// período para travar a camera
    /// </summary>
    private float _lockCameraTimer = 0.0f;
    /// <summary>
    /// Velocidade do movimento do jogador (usada para fazer comparações em condicionais e as transições de velocidade, 
    /// além de ser a velocidade final aplicada ao movimento)
    /// </summary>
    private float _moveSpeed = 0.0f;
    /// <summary>
    /// Suaviza a velocidade
    /// </summary>
    private float _speedSmoothing = 10.0f;
    /// <summary>
    /// Início do tempo de caminhada
    /// </summary>
    private float _walkTimeStart = 0.0f;
    /// <summary>
    /// Constante que multiplica cada eixo do vetor inAirVelocity para definir a velocidade
    /// </summary>
    private float _inAirControlAcceleration = 3.0f;
    /// <summary>
    /// Enum com a direção vertical do jogador
    /// </summary>
    private DirectionType _currentDirection;
    /// <summary>
    /// Enum com a direção horizontal do jogador
    /// </summary>
    private TurnType _currentTurn;
    /// <summary>
    /// Estado do jogador
    /// </summary>
    private State _currentState;
    /// <summary>
    /// Verifica se o jogador está pulando ou não
    /// </summary>
    private bool _jumping;
    /// <summary>
    /// O player é controlável? (para usar na aplicação da gravidade)
    /// </summary>
    private bool _isControllable = true;
    /// <summary>
    /// Limite do pulo foi alcançado?
    /// </summary>
    private bool _jumpingReachedApex = false;
    /// <summary>
    /// Velocidade vertical que é aplicada ao pulo
    /// </summary>
    private float _verticalSpeed = 0.0f;
    /// <summary>
    /// O ultimo tempo registrado após ter pulado
    /// </summary>
    private float _lastJumpTime = -1.0f;
    /// <summary>
    /// Tempo auxiliar para adequar uma condição de entrada em Apply Jumping
    /// </summary>
    private float _jumpDumpingTime = 0.05f;
    /// <summary>
    /// O tempo registrado na ultima vez que foi pressionado o botão de pulo
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
    /// O ultimo tempo ao estar no chão
    /// </summary>
    private float _lastGroundedTime = 0.0f;
    /// <summary>
    /// Velocidade no ar
    /// </summary>
    private Vector3 _inAirVelocity = Vector3.zero;
    /// <summary>
    /// A altura alcançada no pulo
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
    /// <summary>
    /// The _is playing walk sound. Controla tempo para tocar som da caminhada.
    /// </summary>
    private bool _isPlayingWalkSound;
    /// <summary>
    /// The _jumped to sound. Utilizada para verificar se ja pulou para tocar o som do pulo.
    /// Ver se realmente precisa dela.
    /// </summary>
    private bool _jumpedToSound;
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
        this._isPlayingWalkSound = false;
        this._jumpedToSound = false;
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
    /// Muda o estado do jogador para ir no método Setup setar algumas variaveis
    /// e sai do método se não encontrar os componentes CharacterController e Animation
    /// </summary>
    private void Initizalize()
    {
        if (!GetComponent<CharacterController>()) return;
        if (!GetComponent<Animation>()) return;
        this._currentState = State.SETUP;
    }
    /// <summary>
    /// Alimenta as variáveis de direção para movimentar, pular e as variáveis de animação
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
    /// Altera a variável informa a direção do movimento vertical, 
    /// é chamada pelo PlayerInput.cs através de um evento do teclado
    /// </summary>
    /// <param name="z"></param>
    public void MovementVertical(DirectionType z)
    {
        _currentDirection = z;
    }
    /// <summary>
    /// Altera a variável informa a direção do movimento horizontal, 
    /// é chamada pelo PlayerInput.cs através de um evento do teclado
    /// </summary>
    /// <param name="x"></param>
    public void MovementHorizontal(TurnType x)
    {
        _currentTurn = x;
    }
    /// <summary>
    /// Habilita o pulo do jogador 
    /// é chamada pelo PlayerInput.cs através de um evento do teclado
    /// </summary>
    public void JumpMe()
    {
        _jumping = true;
        _lastJumpButtonTime = Time.time;
    }
    /// <summary>
    /// Muda o movimento do jogador (correr ou caminhar) 
    /// é chamada pelo PlayerInput.cs através de um evento do teclado
    /// </summary>
    public void ToggleRun()
    {
        _run = !_run;
    }
    /// <summary>
    /// Função que retorna true se o jogador está se movendo
    /// </summary>
    /// <returns></returns>
    private bool IsMoving()
    {
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
        return isMoving;
    }
    /// <summary>
    /// Realiza toda a movimentação do jogador
    /// </summary>
    public void MovePlayer()
    {
        Transform cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        //retorna true se o jogador está no chão e false senão está
        bool grounded = _controller.isGrounded;

        // Vetor direcionado para a frente relativo à câmera ao longo do plano x-z
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward = new Vector3(forward.x, 0f, forward.z); //forward.y = 0;
        forward = forward.normalized;

        // Vetor apontado para a direita relacionado a camera
        // É sempre ortogonal(perpendicular) ao vetor forward
        Vector3 right = new Vector3(forward.z, 0f, -forward.x);

        //Recebe os valores do movimento vertical e horizontal conforme as teclas são pressionadas
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //quarda o estado do movimento atual na variável wasMoving
        bool wasMoving = _isMoving;
        //se foi apertado alguma tecla de movimentação isMoving é true
        _isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;

        // Direção de destino relativo a câmera
        Vector3 targetDirection = h * right + v * forward;

        if (grounded)
        {
            // Bloquea câmera por um curto período quando a ocorre a transição de se movendo e parado
            _lockCameraTimer += Time.deltaTime;
            if (_isMoving != wasMoving)
                _lockCameraTimer = 0.0f;

            //Nós armazenamos velocidade e direção separadamente,
            //de modo que quando o personagem fica parado ainda temos um sentido válido para a frente
            //moveDirection sempre é normalizada, e nós só atualizamos se houver a entrada do usuário.
            if (targetDirection != Vector3.zero)
            {
                // Se o jogador está realmente lento, apenas encaixa para a direção alvo
                if (_moveSpeed < _walkSpeed * 0.9 && grounded)
                {
                    _moveDirection = targetDirection.normalized;
                }
                // Caso contrário, suavemente volta para ele
                else
                {
                    _moveDirection = Vector3.RotateTowards(_moveDirection, targetDirection, _rotatePlayerSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000f);
                    _moveDirection = _moveDirection.normalized;
                }
            }

            //Suaviza a velocidade baseado na direção atual do alvo
            float curSmooth = _speedSmoothing * Time.deltaTime;
            //Escolha a velocidade alvo
            //Queremos apoiar a entrada analógica, mas certifique-se você não pode andar mais rápido na diagonal do que apenas pra frente ou pra lateral
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
            // Reinicia o início do tempo de caminhada quando desaceleramos
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

        //O vetor final de movimentação considera a gravidade, e finalmente realiza a movimentação
        //_moveDirection.y -= _gravity * Time.deltaTime;
        Vector3 movement = _moveDirection * _moveSpeed + new Vector3(0f, _verticalSpeed, 0f) + _inAirVelocity;
        // collision recebe todas as colisões do jogador durante a movimentação
        _collision = _controller.Move(movement * Time.deltaTime);


        //Se o jogador está no chão rotaciona o jogador de acordo com o movimento dele
        //Se o jogador está no ar rotaciona ele verifica se a magnitude do quadrado do vetor que vai orientar a rotação é maior que a constante .0001f
        //Para comparar a magnitude do vetor com alguma coisa é mais rápido usar o sqrMAgnitude(magnitude quadrada do vetor) = (x*x+y*y+z*z)
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

        // Está em modo de pulo apenas quando esta no chão
        if (IsGrounded())
        {
            _lastGroundedTime = Time.time;
            _inAirVelocity = Vector3.zero;
            if (_jumping)
            {
                _jumping = false;

            }
        }
    }
    /// <summary>
    /// Seleciona a ação do jogador
    /// </summary>
    private void SelectAction()
    {
        //Controla o pulo do jogador e chama a função da animação correspondente.
        if (_controller.isGrounded)
        {
            //Chama as funções das animações conforme a direção vertical e horizontal
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
        //Chama a movimentação do jogador
        MovePlayer();
    }
    private void ApplyJumping()
    {
        // Se o tempo do último salto for maior que o tempo do jogo sai da função
        if (_lastJumpTime + _jumpDumpingTime > Time.time)
            return;

        if (IsGrounded())
        {
            // Jump
            // Incrementa a velocidade vertical (aplicada no pulo)		
            if (Time.time < _lastJumpButtonTime + _jumpTimeout)
            {
                this._jumpedToSound = true;
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

            if (IsGrounded()) {
                _verticalSpeed = 0.0f;
                if(this._jumpedToSound) {
                    AudioSource.PlayClipAtPoint(this._jumpSound, Camera.main.transform.position,1f);
                    this._jumpedToSound = false;
                }
            }else
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
    /// Funções que apenas chamam as animações do movimento do jogador
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
        if(!this._isPlayingWalkSound) {
            AudioSource.PlayClipAtPoint(this._walkSound, Camera.main.transform.position,1f);
            this._isPlayingWalkSound = true;
            StartCoroutine(TimeForWalk());
        }
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
        //animation.CrossFade("CombatStance");     //animação idle aqui
    }
    #endregion
    #region Coroutines
    /// <summary>
    /// Times for walk. Controla velocidade do som da caminhada.
    /// </summary>
    /// <returns>
    /// The for walk.
    /// </returns>
    IEnumerator TimeForWalk () {
        yield return new WaitForSeconds(0.5f);
        this._isPlayingWalkSound = false;
    }
    #endregion
}