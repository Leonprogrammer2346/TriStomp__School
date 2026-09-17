using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TextBox : Component
{
    public Game Game;
    public GameObject Obj;
    public static int beanztalk;
    public static int othertalk;
    public static int demantalk;
    public static int pasteltalk;
    public static int mysttalk;
    public static int jemtalk;
    public static bool animgot;
    public int myindx;


    public TextBox(Game game, GameObject obj)
    {
        Game = game;
        Obj = obj;
    }

    public override void Update(float dt, KeyboardState input)
    {
        if (animgot == false)
        {
            beanztalk = Obj.game.LoadTexture("Beantext.png");
            othertalk = Obj.game.LoadTexture("Othertext.png");
            demantalk = Obj.game.LoadTexture("Demantext.png");
            pasteltalk = Obj.game.LoadTexture("Pasttext.png");
            mysttalk = Obj.game.LoadTexture("Mystext.png");
            jemtalk = Obj.game.LoadTexture("Jemantext.png");
            animgot = true;
        }

        if (Game.textndex-1 < Game.AllText.Count && Game.textndex != 0)
        {
            if (Game.Talker[Game.textndex-1] == "Beanz")
            {
                Obj.Texture = beanztalk;
                Obj.GetComponent<Animation>().frames = 2;
                Obj.GetComponent<Animation>().column = 2;
                Obj.GetComponent<Animation>().row = 1;
                Obj.GetComponent<Animation>().Width = 1;
                Obj.GetComponent<Animation>().Height = 1;
            }
            else if (Game.Talker[Game.textndex-1] == "Other")
            {
                Obj.Texture = othertalk;
                Obj.GetComponent<Animation>().frames = 2;
                Obj.GetComponent<Animation>().column = 2;
                Obj.GetComponent<Animation>().row = 1;
                Obj.GetComponent<Animation>().Width = 1;
                Obj.GetComponent<Animation>().Height = 1;
            }
            else if (Game.Talker[Game.textndex - 1] == "Deman")
            {
                Obj.Texture = demantalk;
                Obj.GetComponent<Animation>().frames = 2;
                Obj.GetComponent<Animation>().column = 2;
                Obj.GetComponent<Animation>().row = 1;
                Obj.GetComponent<Animation>().Width = 1;
                Obj.GetComponent<Animation>().Height = 1;
            }
            else if (Game.Talker[Game.textndex - 1] == "Pastel")
            {
                Obj.Texture = pasteltalk;
                Obj.GetComponent<Animation>().frames = 2;
                Obj.GetComponent<Animation>().column = 2;
                Obj.GetComponent<Animation>().row = 1;
                Obj.GetComponent<Animation>().Width = 1;
                Obj.GetComponent<Animation>().Height = 1;
            }
            else if (Game.Talker[Game.textndex - 1] == "Myst")
            {
                Obj.Texture = mysttalk;
                Obj.GetComponent<Animation>().frames = 2;
                Obj.GetComponent<Animation>().column = 2;
                Obj.GetComponent<Animation>().row = 1;
                Obj.GetComponent<Animation>().Width = 1;
                Obj.GetComponent<Animation>().Height = 1;
            }
            else if (Game.Talker[Game.textndex - 1] == "Jeman")
            {
                Obj.Texture = jemtalk;
                Obj.GetComponent<Animation>().frames = 2;
                Obj.GetComponent<Animation>().column = 2;
                Obj.GetComponent<Animation>().row = 1;
                Obj.GetComponent<Animation>().Width = 1;
                Obj.GetComponent<Animation>().Height = 1;
            }
        }
        
        
    }
}
