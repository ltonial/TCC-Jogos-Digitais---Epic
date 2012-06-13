using UnityEngine;
using System.Collections;

public class PlayItem : MenuItem
{
	public override void OnClick()
	{
        AutoFade.LoadLevel("Prototype", 1.5f, 1.5f, Color.black);
	}
}
