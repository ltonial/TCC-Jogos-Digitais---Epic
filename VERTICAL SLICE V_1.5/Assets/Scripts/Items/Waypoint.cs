using UnityEngine;

/// <summary>
/// Waypoint.
/// </summary>
public class Waypoint : Item
{
    #region Attributes
    /// <summary>
    /// Tipo de waypoint.
    /// </summary>
    private WaypointType _type;
    #endregion
    #region Constructors
    /// <summary>
    /// Criando o waypoint.
    /// </summary>
    public Waypoint(int pId, GameObject pObject, WaypointType pType)
        : base(pId, pObject)
    {
        this._type = pType;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Recupera o tipo.
    /// </summary>
    public WaypointType Type { get { return this._type; } }
    #endregion
}