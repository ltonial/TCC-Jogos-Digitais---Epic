using UnityEngine;
using System.Collections;

public class MenuGui : MonoBehaviour {

    void Start() {
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel("Game");

        }

    }

    void OnGUI() {
        GL.PushMatrix();
        GL.LoadPixelMatrix(0,Screen.width,Screen.height,0);
        Funcoes._matUi.SetPass(0);

        Funcoes.DesenhaQuad(0,0,Screen.width,Screen.height,3,
            Funcoes.VetCores[(int)ColorsType.BACK_MENU_1],Funcoes.VetCores[(int)ColorsType.BACK_MENU_2]);

        GL.Color(Funcoes.VetCores[(int)ColorsType.WINDOWS_MENU]);
        Funcoes.DesenhaQuad(Screen.width/2-300,Screen.height/2-150,600,400);

        GL.Color(Funcoes.VetCores[(int)ColorsType.BLUE_MENU]);
        Funcoes.DesenhaQuad(Screen.width/2-300,Screen.height/2-150,600,50);



        GL.PopMatrix();
    }
}
