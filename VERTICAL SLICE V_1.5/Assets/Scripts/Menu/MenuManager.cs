using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class MenuManager : MonoBehaviour
{
    #region Attributes
	/// <summary>
	/// The _array menu items.
	/// </summary>
    public MenuItem[] _arrayMenuItems;
	/// <summary>
	/// The _current menu item.
	/// </summary>
    private int _currentMenuItem = 0;
	/// <summary>
	/// The _key delay.
	/// </summary>
    private float _keyDelay = .25f;
    #endregion
    #region Methods
	void Awake()
	{
		
	}
	/// <summary>
	/// Start this instance.
	/// </summary>
    IEnumerator Start()
    {
		this._arrayMenuItems[this._currentMenuItem].OnSelected(true);
		int lastCurrentMenuItem = this._currentMenuItem;
		
        while (true)
        {
            if (Input.GetAxisRaw("Vertical") > 0.9f)
            {
                lastCurrentMenuItem = this._currentMenuItem;

                this._currentMenuItem--;
                if (this._currentMenuItem < 0) this._currentMenuItem = 0;

                if (lastCurrentMenuItem != this._currentMenuItem)
                {
                    this._arrayMenuItems[lastCurrentMenuItem].OnSelected(false);
                    this._arrayMenuItems[this._currentMenuItem].OnSelected(true);
                }

                yield return new WaitForSeconds(this._keyDelay);
            }
            else if (Input.GetAxisRaw("Vertical") < -0.9f)
            {
                lastCurrentMenuItem = this._currentMenuItem;

                this._currentMenuItem++;
                if (this._currentMenuItem >= this._arrayMenuItems.Length) this._currentMenuItem = this._arrayMenuItems.Length - 1;

                if (lastCurrentMenuItem != this._currentMenuItem)
                {
                    this._arrayMenuItems[lastCurrentMenuItem].OnSelected(false);
                    this._arrayMenuItems[this._currentMenuItem].OnSelected(true);
                }

                yield return new WaitForSeconds(this._keyDelay);
            }
            yield return 0;
			
			// Pressionando o space sobre uma das opções, será executado seu evento onclick relativo.
            if (Input.GetButtonDown("Jump"))
            {
                if (this._currentMenuItem == 0)
                    this._arrayMenuItems[this._currentMenuItem].OnClick();
            }
        }
    }
    #endregion
}