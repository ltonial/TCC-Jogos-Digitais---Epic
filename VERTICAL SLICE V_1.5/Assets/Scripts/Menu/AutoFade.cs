using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour
{
    #region Attributes
    private static AutoFade _instance = null;
    private Material _material = null;
    private string _levelName = "";
    private int _levelIndex = 0;
    private bool _fading = false;
    #endregion
    #region Methods
    private static AutoFade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (new GameObject("AutoFade")).AddComponent<AutoFade>();
            }
            return _instance;
        }
    }
    public static bool Fading
    {
        get { return Instance._fading; }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _instance = this;
        _material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
    }
    private void DrawQuad(Color aColor, float aAlpha)
    {
        aColor.a = aAlpha;
        _material.SetPass(0);
        GL.Color(aColor);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }
    private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, t);
        }
        if (_levelName != "")
            Application.LoadLevel(_levelName);
        else
            Application.LoadLevel(_levelIndex);
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
            DrawQuad(aColor, t);
        }
        _fading = false;
    }
    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        _fading = true;
        StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
    }
    public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance._levelName = aLevelName;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
    public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance._levelName = "";
        Instance._levelIndex = aLevelIndex;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
    #endregion
}