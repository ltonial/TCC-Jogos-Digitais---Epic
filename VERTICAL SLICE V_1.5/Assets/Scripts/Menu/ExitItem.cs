using UnityEngine;
using System.Collections;

public class ExitItem : MenuItem
{
	public override void OnClick()
	{
		Application.Quit();
	}
}