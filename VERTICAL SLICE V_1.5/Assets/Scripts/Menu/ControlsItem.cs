using UnityEngine;
using System.Collections;

public class ControlsItem : MenuItem
{
	public override void OnClick()
	{
        AutoFade.LoadLevel("Controls", 1.5f, 1.5f, Color.black);
	}
}