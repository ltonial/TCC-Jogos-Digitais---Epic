using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Computador (terminal).
/// </summary>
public class Computer : Item
{
    #region Attributes
    /// <summary>
    /// Tipo de computador.
    /// </summary>
    private ComputerType _type;
    #endregion
    #region Constructors
    /// <summary>
    /// Criando o waypoint.
    /// </summary>
    public Computer(int pId, GameObject pObject, ComputerType pType)
        : base(pId, pObject)
    {
        this._type = pType;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Recupera o tipo.
    /// </summary>
    public ComputerType Type { get { return this._type; } }
    #endregion
}