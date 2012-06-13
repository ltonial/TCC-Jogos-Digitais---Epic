using UnityEngine;

/// <summary>
/// Item genérico do cenário.
/// </summary>
public abstract class Item
{
    #region Attributes
    /// <summary>
    /// Identificador único.
    /// </summary>
    protected int _id;
    /// <summary>
    /// Objeto onde está o item.
    /// </summary>
    protected GameObject _myObject;
    #endregion
    #region Constructors
    /// <summary>
    /// Criando o item.
    /// </summary>
    public Item(int pId, GameObject pObject)
    {
        this._id = pId;
        this._myObject = pObject;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Recupera o identificador único.
    /// </summary>
    public int ID { get { return this._id; } }
    /// <summary>
    /// Recupera o objeto.
    /// </summary>
    public GameObject GameObject { get { return this._myObject; } }
    #endregion
}