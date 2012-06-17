using UnityEngine;
using System.Collections;

public class MenuPause : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Se está pausado.
    /// </summary>
    public static bool Paused;
    /// <summary>
    /// O texto escrito 'pause'.
    /// </summary>
    private GUIText _textPause;
    #endregion
    #region Properties
    //public bool IsPaused { get { return this._paused; } }
    #endregion
    #region Methods
    void Awake()
    {
        this._textPause = GameObject.FindGameObjectWithTag("Pause").GetComponent<GUIText>();
        this._textPause.enabled = false;
    }
    void Start()
    {
        MenuPause.Paused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause))
            UpdatePause();
    }
    private void UpdatePause()
    {
        MenuPause.Paused = !MenuPause.Paused;
        this._textPause.enabled = MenuPause.Paused;

        Time.timeScale = 1f - Time.timeScale;
        gameObject.GetComponent<CameraAction>().enabled = !gameObject.GetComponent<CameraAction>().enabled;
    }
    #endregion
}