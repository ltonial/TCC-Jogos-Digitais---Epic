using UnityEngine;
using System.Collections;

public class MenuStart : MonoBehaviour 
{
	#region Attributes
	/// <summary>
	/// The _text mesh.
	/// </summary>
	public TextMesh _textMesh;
	/// <summary>
	/// The _can draw.
	/// </summary>
	private bool _canDraw;
	/// <summary>
	/// The _time.
	/// </summary>
	private float _time;
	/// <summary>
	/// Constant TIMETODRAW.
	/// </summary>
	private const float TIMETODRAW = .5f;
	#endregion	
	#region Methods
	void Start () 
	{
		this._canDraw = false;
		this._time = TIMETODRAW;
	}
	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			AutoFade.LoadLevel("Menu", 1.5f, 1.5f, Color.black);
		}
	}
	void FixedUpdate () 
	{
		this._time -= Time.deltaTime;
		if (this._time <= 0f)
		{
			this._time = TIMETODRAW;
			this._canDraw = !this._canDraw;
			this._textMesh.characterSize = this._canDraw ? 1f : 0f;
		}
	}
	#endregion
}
