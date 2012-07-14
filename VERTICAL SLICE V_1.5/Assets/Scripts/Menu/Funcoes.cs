using UnityEngine;
using System.Collections;

public static class Funcoes {

    public static int _raioCanto=8;
    public static Material _matUi=new Material(
        "Shader \"Transparent/Diffuse\" {"
       +" SubShader { Pass {"
       +"  Blend SrcAlpha OneMinusSrcAlpha"
       +"  ZWrite Off Cull Off Fog { Mode Off }"
       +"  BindChannels { Bind \"vertex\", vertex Bind \"color\", color }"
       +" } }"
       +"}");

    public static ImgManager[] VetImg {
        get {
            return new ImgManager[] {
                new ImgManager("Menu/Iniciar",500,500)
            };
        }
    }

    public static Color[] VetCores {
        get {
            return new Color[] {
                new Color(0.5117f,0.535f,0.527f,1f),
                new Color(0.789f,0.796f,0.777f,1f),
                new Color(0f,0.671f,0.929f,1f),
                new Color(0f,0f,0f,0.2f)
            };
        }
    }

    public static void DesenhaArco(float x, float y, float raio, float ang_min, float ang_max, float inc, bool cheio) {
        float ang = 0f;
        float angr = ang_min*Mathf.Deg2Rad;
        GL.Begin(cheio?GL.TRIANGLES:GL.LINES);
        for(ang=ang_min; ang<ang_max; ang+=inc) {
            if(cheio) GL.Vertex3(x, y, 0);
            GL.Vertex3(x+raio*Mathf.Cos(angr), y+raio*Mathf.Sin(angr), 0);
            angr=(ang+inc)*Mathf.Deg2Rad;
            GL.Vertex3(x+raio*Mathf.Cos(angr), y+raio*Mathf.Sin(angr), 0);
        }
        GL.End();
    }
    public static void DesenhaQuad(float x, float y, float w, float h) {
        Funcoes.DesenhaArco(x+w-Funcoes._raioCanto,y+h-Funcoes._raioCanto,Funcoes._raioCanto,0,90,5,true); // direito inferior
        Funcoes.DesenhaArco(x+Funcoes._raioCanto,y+h-Funcoes._raioCanto, Funcoes._raioCanto,90,180,5,true); // esquerdo inferior
        Funcoes.DesenhaArco(x+Funcoes._raioCanto,y+Funcoes._raioCanto,Funcoes._raioCanto,180,270,5,true); // esquerdo superior
        Funcoes.DesenhaArco(x+w-Funcoes._raioCanto,y+Funcoes._raioCanto,Funcoes._raioCanto,270,360,5,true); // direito superior
        GL.Begin(GL.QUADS);
        // centro
        GL.Vertex3(x+Funcoes._raioCanto,y,0);
        GL.Vertex3(x+w-Funcoes._raioCanto,y,0);
        GL.Vertex3(x+w-Funcoes._raioCanto,y+h,0);
        GL.Vertex3(x+Funcoes._raioCanto,y+h,0);
        // entre cantos da esquerda
        GL.Vertex3(x,y+Funcoes._raioCanto,0);
        GL.Vertex3(x+Funcoes._raioCanto,y+Funcoes._raioCanto,0);
        GL.Vertex3(x+Funcoes._raioCanto,y+h-Funcoes._raioCanto,0);
        GL.Vertex3(x,y+h-Funcoes._raioCanto,0);
        // entre cantos da direita
        GL.Vertex3(x+w-Funcoes._raioCanto,y+Funcoes._raioCanto,0);
        GL.Vertex3(x+w,y+Funcoes._raioCanto,0);
        GL.Vertex3(x+w,y+h-Funcoes._raioCanto,0);
        GL.Vertex3(x+w-Funcoes._raioCanto,y+h-Funcoes._raioCanto,0);
        GL.End();
    }
    public static void DesenhaQuad(float x,float y,float w,float h,int lado,Color cor1,Color cor2) {
        GL.Color(cor2);
        Funcoes.DesenhaArco(x+w-Funcoes._raioCanto,y+h-Funcoes._raioCanto,Funcoes._raioCanto,0,90,5,true); // direito inferior
        Funcoes.DesenhaArco(x+Funcoes._raioCanto,y+h-Funcoes._raioCanto, Funcoes._raioCanto,90,180,5,true); // esquerdo inferior
        GL.Color(cor1);
        Funcoes.DesenhaArco(x+Funcoes._raioCanto,y+Funcoes._raioCanto,Funcoes._raioCanto,180,270,5,true); // esquerdo superior
        Funcoes.DesenhaArco(x+w-Funcoes._raioCanto,y+Funcoes._raioCanto,Funcoes._raioCanto,270,360,5,true); // direito superior
        GL.Begin(GL.QUADS);
    
        if(lado==3) {
          GL.Vertex3(x,y,0);
          GL.Vertex3(x+w,y,0);
          GL.Color(cor2);
          GL.Vertex3(x+w,y+h,0);
          GL.Vertex3(x,y+h,0);
        }else {
          GL.Vertex3(x+Funcoes._raioCanto,y,0);
          GL.Vertex3(x+w-Funcoes._raioCanto,y,0);
          GL.Color(cor2);
          GL.Vertex3(x+w-Funcoes._raioCanto,y+h,0);
          GL.Vertex3(x+Funcoes._raioCanto,y+h,0);
        }
        if(lado==0 || lado==1){
          // entre cantos da esquerda
          GL.Color(cor1);
          GL.Vertex3(x,y+Funcoes._raioCanto,0);
          GL.Vertex3(x+Funcoes._raioCanto,y+Funcoes._raioCanto,0);
          GL.Color(cor2);
          GL.Vertex3(x+Funcoes._raioCanto,y+h-Funcoes._raioCanto,0);
          GL.Vertex3(x,y+h-Funcoes._raioCanto,0);
        }
        if(lado==0 || lado==2){
          // entre cantos da direita
          GL.Color(cor1);
          GL.Vertex3(x+w-Funcoes._raioCanto,y+Funcoes._raioCanto,0);
          GL.Vertex3(x+w,y+Funcoes._raioCanto,0);
          GL.Color(cor2);
          GL.Vertex3(x+w,y+h-Funcoes._raioCanto,0);
          GL.Vertex3(x+w-Funcoes._raioCanto,y+h-Funcoes._raioCanto,0);
        }
        GL.End();
    }
}
