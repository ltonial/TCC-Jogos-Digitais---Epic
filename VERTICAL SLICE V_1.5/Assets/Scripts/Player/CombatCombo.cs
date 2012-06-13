using UnityEngine;

/// <summary>
/// Controla os combos do personagem
/// </summary>
public class CombatCombo
{
    #region Attributes
    public string[] buttons;
    /// <summary>
    ///  É alterado conforme o os botões são pressionados.
    /// </summary>
    private int currentIndex = 0;
    public float allowedTimeBetweenButtons = .3f; 
    private float timeLastButtonPressed;
    #endregion
    # region Methods (Class)
    public CombatCombo(string[] b)
    {
        buttons = b;
    }
    /// <summary>
    /// É chamado uma vez por frame, quando o combo é finalizado retorna true
    /// </summary>
    /// <returns></returns>
    public bool Check()
    {
        if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
        {
            if (currentIndex < buttons.Length)
            {
                if (Input.GetButtonDown(buttons[currentIndex]))
                {
                    timeLastButtonPressed = Time.time;
                    currentIndex++;
                }
                if (currentIndex >= buttons.Length)
                {
                    currentIndex = 0;
                    return true;
                }
                else return false;
            }
        }
        return false;
    }
    #endregion
}