using UnityEngine;
using System.Collections;

public class CreditsItem : MenuItem
{
	public override void OnClick()
	{
        AutoFade.LoadLevel("Credits", 1.5f, 1.5f, Color.black);
	}
}
