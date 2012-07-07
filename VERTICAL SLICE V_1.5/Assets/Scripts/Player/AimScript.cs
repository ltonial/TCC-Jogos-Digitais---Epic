using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    #region Attributes
    private GameObject _bowObject;
    private List<GameObject> _auxBowObjects;
    private PlayerManager _playerManager;
    #endregion
    #region Methods
    void Start()
    {
        this._playerManager = gameObject.GetComponent<PlayerManager>();
        this._bowObject = transform.FindChild("Bow").gameObject;
        this._bowObject.active = false;
        this._auxBowObjects = GameObject.FindGameObjectsWithTag("AuxBow").ToList();
        this._auxBowObjects.ForEach(b => b.active = false);

        Texture texture = (Texture)Resources.Load("Shots/Aim");
        gameObject.AddComponent(guiTexture.GetType());
        gameObject.guiTexture.texture = texture;
        gameObject.guiTexture.pixelInset = new Rect((Screen.width / 2) - (texture.width / 2), (Screen.height / 2) - (texture.height / 2), texture.width, texture.height);
        gameObject.guiTexture.enabled = false;
    }
    void Update()
    {
        this._bowObject.active = PlayerManager.CurrentStatePlayer == StatePlayerType.AIM;
        this._auxBowObjects.ForEach(b => b.active = PlayerManager.CurrentStatePlayer == StatePlayerType.AIM);
    }
    void OnGUI()
    {
        if (PlayerManager.CurrentStatePlayer == StatePlayerType.AIM && !this._playerManager.Weapon.IsShot)
        {
            animation.CrossFade("aim");
            animation["aim"].wrapMode = WrapMode.ClampForever;
            gameObject.guiTexture.enabled = true;
            GUI.DrawTexture(gameObject.guiTexture.pixelInset, gameObject.guiTexture.texture);
        }
    }
    #endregion
}