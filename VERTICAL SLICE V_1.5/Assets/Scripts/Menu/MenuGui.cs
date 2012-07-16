using UnityEngine;
using System.Collections;

public class MenuGui : MonoBehaviour {
    private enum _itensGUI {
        NADA,
        ITENS
    }
    private _itensGUI _mouseItem;
    private Font _euphemia;
    private Rect _blueBarRect;
    private Rect _menuRect;
    private int _buttonControl;
    private bool _telaControle;
    private bool _telaAgradecimentos;
    private bool _telaCreditos;
    private int _mouseY;
    private int _wScreenOld;
    private int _hScreenOld;

    void Start() {
        this._mouseItem = _itensGUI.NADA;
        this._buttonControl = 50;
        this._telaControle = false;
        this._telaCreditos = false;
        this._telaAgradecimentos = false;
        this._wScreenOld = Screen.width;
        this._hScreenOld = Screen.height;
        this._euphemia = (Font)Resources.Load("Fonts/euphemia");
        this.UpdateRects();
    }

    void Update() {
        this._mouseY = Screen.height-(int)Input.mousePosition.y;
        if(this._wScreenOld != Screen.width || this._hScreenOld != Screen.height) {
            this._wScreenOld = Screen.width;
            this._hScreenOld = Screen.height;
            this.UpdateRects();
        }
        this._mouseItem = this.CalcMouseItem();

        if(Input.GetMouseButtonDown(0)) {
            if(this._mouseItem == _itensGUI.ITENS) {
                if(this._buttonControl==50) { //novo jogo
                    AutoFade.LoadLevel("Game", 2, 2, Color.black);
                }else if(this._buttonControl==100) { //controles
                    this._telaControle = true;
                }else if(this._buttonControl==150) { //creditos
                    this._telaCreditos = true;
                }else if(this._buttonControl==200) { //agradecimentos
                    this._telaAgradecimentos = true;
                }else if(this._buttonControl==250) { //fechar
                    Application.Quit();
                }
            }else {
                this._telaControle = false;
                this._telaAgradecimentos = false;
                this._telaCreditos = false;
            }

        }

    }

    private void UpdateRects() {
        this._blueBarRect = new Rect(Screen.width/2-300,Screen.height/2-200,550,50);
        this._menuRect = new Rect(Screen.width/2-300,Screen.height/2-200,550,400);
    }

    private _itensGUI CalcMouseItem() {
        if(!this._telaControle && !this._telaCreditos && !this._telaAgradecimentos) {
            if(Input.mousePosition.x >= this._menuRect.x+100 && Input.mousePosition.x <= this._menuRect.x+300) {
                for(int x=0; x<=250; x+=50) {
                    if(this._mouseY >= this._menuRect.y+100 && this._mouseY <= this._menuRect.y+100+x) {
                        this._buttonControl = x;
                        return _itensGUI.ITENS;
                    }
                }
            }
        }
    return _itensGUI.NADA;
  }

    void OnGUI() {
        ImagesType imgCursor = (this._mouseItem==_itensGUI.ITENS?ImagesType.CURSOR_MAO:ImagesType.NADA);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0,Screen.width,Screen.height,0);
        Funcoes._matUi.SetPass(0);
        GUI.skin.label.fontSize=14;
        GUI.skin.font = this._euphemia;
        GUI.skin.label.font = this._euphemia;
        GUI.skin.label.fontStyle=FontStyle.Bold;

        Funcoes.DesenhaQuad(0,0,Screen.width,Screen.height,3,
            Funcoes.VetCores[(int)ColorsType.BACK_MENU_1],Funcoes.VetCores[(int)ColorsType.BACK_MENU_2]);
        GL.Color(Funcoes.VetCores[(int)ColorsType.BLUE_MENU]);
        GL.Begin(GL.QUADS);
        GL.Vertex3(30,Screen.height-25,0);
        GL.Vertex3(Screen.width-30,Screen.height-25,0);
        GL.Vertex3(Screen.width-30,Screen.height-20,0);
        GL.Vertex3(30,Screen.height-20,0);
        GL.End();

        if(!_telaControle)
            Funcoes.VetImg[(int)ImagesType.ITEM_MENU].Draw(Screen.width/2-420,-50);

        Funcoes.VetImg[(int)ImagesType.LOGO_MENU_01].Draw(Screen.width/2-256,-120);

        Funcoes._matUi.SetPass(0);
        if(!_telaControle) {
            GL.Color(Funcoes.VetCores[(int)ColorsType.WINDOWS_MENU]);
            Funcoes.DesenhaQuad(this._menuRect.x,this._menuRect.y,this._menuRect.width,this._menuRect.height);

            GL.Color(Funcoes.VetCores[(int)ColorsType.BLUE_MENU]);
            Funcoes.DesenhaQuad(this._blueBarRect.x,this._blueBarRect.y,this._blueBarRect.width,this._blueBarRect.height);
        }

        if(this._telaAgradecimentos) {
            GUI.Label (new Rect(this._blueBarRect.x+20, this._blueBarRect.y+10, 300, 60), "MENU INICIAL | AGRADECIMENTOS");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+100, 400, 50), "ADRIANA DENARDIN");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+150, 400, 50), "BRUNA MEDEIROS MARQUES | ANIMAÇÃO");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+200, 400, 50), "CARLOS EDUARDO SARTORI FERNANDES | MÚSICA");
        }else if(this._telaCreditos) {
            GUI.Label (new Rect(this._blueBarRect.x+20, this._blueBarRect.y+10, 200, 60), "MENU INICIAL | CRÉDITOS");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+100, 600, 50), "BERNARDO COSTA DE ARAÚJO <3ernardo@gmail.com>");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+150, 600, 50), "CAROLINA BRAVO PILLON <carolinabpilon@gmail.com>");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+200, 600, 50), "DIEGO GONÇALVES SANTOS <diegosanto@gmail.com>");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+250, 600, 50), "FABIANO RICARDO FARIAS <fabiano.r.farias@gmail.com>");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+300, 600, 50), "JOÃO RICARDO FALLEIRO KUCERA <falleirok@gmail.com>");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+350, 600, 50), "LUCAS TONIAL SCORTEGAGNA <ltonial@gmail.com>");
        }else if(this._telaControle) {
            Funcoes.VetImg[(int)ImagesType.CONTROLS_MENU].Draw(0,-20);
        }else {
            Funcoes.VetImg[(int)ImagesType.BUTTON_MENU].Draw((int)this._menuRect.x+80,(int)this._menuRect.y-10+50+this._buttonControl);

            Funcoes._matUi.SetPass(0);
            GUI.Label (new Rect(this._blueBarRect.x+20, this._blueBarRect.y+10, 200, 60), "MENU INICIAL");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+100, 200, 50), "NOVO JOGO");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+150, 200, 50), "CONTROLES");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+200, 200, 50), "CRÉDITOS");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+250, 200, 50), "AGRADECIMENTOS");
            GUI.Label (new Rect(this._menuRect.x+100, this._menuRect.y+300, 200, 50), "FECHAR");
        }






        Screen.showCursor = imgCursor==ImagesType.NADA;
        if(imgCursor!=ImagesType.NADA) {
            Funcoes.VetImg[(int)imgCursor].Draw((int)(Input.mousePosition.x-(imgCursor==ImagesType.CURSOR_MAO?12:0)),
            (int)(Screen.height-Input.mousePosition.y));
        }
        GL.PopMatrix();
    }
}
