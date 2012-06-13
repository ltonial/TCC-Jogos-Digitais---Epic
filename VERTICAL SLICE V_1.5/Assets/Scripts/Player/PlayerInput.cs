using UnityEngine;

/// <summary>
/// Controla todas os Inputs feitos pelo jogador.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    #region Attributes
    private const string MOVEVERTICAL = "Vertical";
    private const string METHOD_MOVEMENTVERTICAL = "MovementVertical";
    private const string MOVEHORIZONTAL = "Horizontal";
    private const string METHOD_MOVEMENTHORIZONTAL = "MovementHorizontal";
    private const string RUN = "Run";
    private const string TOGGLERUN = "ToggleRun";
    private const string RIGHTBUTTONMOUSE = "RightButtonMouse";
    private const string TOGGLEAIMBUTTONDOWN = "ToggleAimButtonDown";
    private const string JUMP = "Jump";
    private const string METHOD_JUMP = "JumpMe";
    private Transform cameraTransform;
    private CameraAction scriptCamera;
    #endregion
    #region Methods (Inherit)
    void Awake()
    {
        cameraTransform = Camera.main.transform;
        scriptCamera = Camera.main.GetComponent<CameraAction>();
    }
    #endregion
    #region Methods (Class)
    /// <summary>
    /// Update.
    /// </summary>
    void Update()
    {

        if (Input.GetButtonDown(JUMP))
            SendMessage(METHOD_JUMP);

        /// <summary>
        /// Movimento Vertical do player, teclas W e S
        /// </summary>
        if (Input.GetButton(MOVEVERTICAL))
        {
            if (Input.GetAxis(MOVEVERTICAL) > 0)
                SendMessage(METHOD_MOVEMENTVERTICAL, DirectionType.FORWARD);
            else
                SendMessage(METHOD_MOVEMENTVERTICAL, DirectionType.BACK);
        }
        if (Input.GetButtonUp(MOVEVERTICAL))
            SendMessage(METHOD_MOVEMENTVERTICAL, DirectionType.NONE);

        /// <summary>
        /// Movimento Horizontal do player, teclas A e D
        /// </summary>
        if (Input.GetButton(MOVEHORIZONTAL))
        {
            if (Input.GetAxis(MOVEHORIZONTAL) > 0)
                SendMessage(METHOD_MOVEMENTHORIZONTAL, TurnType.RIGHT);
            else
                SendMessage(METHOD_MOVEMENTHORIZONTAL, TurnType.LEFT);
        }
        if (Input.GetButtonUp(MOVEHORIZONTAL))
            SendMessage(METHOD_MOVEMENTHORIZONTAL, TurnType.NONE);

        //Correndo.
        if (Input.GetButtonDown(RUN))
            SendMessage(TOGGLERUN);

        //Ativar/desativar a mira (botão direito do mouse).
        if (Input.GetButtonDown(RIGHTBUTTONMOUSE))
            scriptCamera.SendMessage(TOGGLEAIMBUTTONDOWN, true);
        if (Input.GetButtonUp(RIGHTBUTTONMOUSE))
            scriptCamera.SendMessage(TOGGLEAIMBUTTONDOWN, false);
    }
    #endregion
}