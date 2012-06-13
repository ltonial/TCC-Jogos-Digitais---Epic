using UnityEngine;
using System.Collections;

public class BackItem : MenuItem {

	public override void OnClick ()
	{
		AutoFade.LoadLevel("Menu", 1.5f, 1.5f, Color.black);
	}
}