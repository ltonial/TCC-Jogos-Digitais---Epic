using UnityEngine;
using System.Collections;

public class ImgManager : MonoBehaviour {

    #region Attributes
    private Texture2D _tex;
    private string _resName;
    private int _w;
    private int _h;
    #endregion

    #region Properties
    public Texture2D GetTex {
     get {return this._tex;}
    }
    public string GetResName {
     get {return this._resName;}
    }
    public int GetW {
     get {return this._w;}
    }
    public int GetH {
     get {return this._h;}
    }
    #endregion

    #region Constructors
    public ImgManager (string res_name, int w, int h) {
        this._resName=res_name;
        this._w=w;
        this._h=h;
        this._tex=(Texture2D)Resources.Load(this._resName);
    }
    #endregion

    #region Methods
    public void Draw(int pos_x, int pos_y) {
        GUI.DrawTexture(new Rect(pos_x,pos_y,this._w,this._h),this._tex);
    }
    #endregion

}
