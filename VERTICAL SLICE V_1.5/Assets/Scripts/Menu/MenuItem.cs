using UnityEngine;
using System.Collections;

public abstract class MenuItem : MonoBehaviour
{
	#region Methods
	/// <summary>
	/// Realiza a transação dos itens do menu quando selecionados.
	/// </summary>
    public void OnSelected(bool pActive)
    {
        if (pActive)
        {
            iTween.MoveTo(gameObject, new Vector3(transform.position.x, transform.position.y, - 3f), .5f);
            iTween.RotateTo(gameObject, new Vector3(transform.position.x, -90f, transform.position.z), .5f);
        }
        else
        {
            iTween.MoveTo(gameObject, new Vector3(transform.position.x, transform.position.y, 0f), .5f);
            iTween.RotateTo(gameObject, new Vector3(transform.position.x, 0f, transform.position.z), .5f);
        }
    }
	/// <summary>
	/// Método abstrato que todos os itens do menu devem implementar.
	/// </summary>
    public abstract void OnClick();
    #endregion
}
