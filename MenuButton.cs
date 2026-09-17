using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class MenuButton : Component
{
    GameButton mybutt;
    public static int nonhov;
    public static int onhov;
    public GameObject Obj;
    static bool animgot = false;
    public MenuButton(GameObject obj)
    {
        mybutt = obj.GetComponent<GameButton>();
        Obj = obj;
    }

    public override Component Clone(GameObject newObj)
    {
        return new MenuButton(newObj);

    }
    public override void Update(float dt, KeyboardState input)
    {
        if (animgot == false)
        {
            onhov = Obj.game.LoadTexture("startbutton_alt.png");
            nonhov = Obj.game.LoadTexture("startbutton.png");
            animgot = true;
        }

        if (mybutt.hovering == false)
        {
            Obj.Width = 1.5f;
            Obj.Height = 0.9f;
        }
        else
        {
            Obj.Width = 1.3f;
            Obj.Height = 0.7f;
        }

        if (mybutt.clicking == true)
        {
           
            Obj.game.newdirect = "Scenes";
            Obj.game.newindex = "Intro";
        }
        
    }
}
