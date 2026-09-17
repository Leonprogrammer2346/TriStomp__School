using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Pause : Component
{
    GameObject obj;
    public GameObject backbutt;
    public bool firsty;
    public static bool firstis;
    public static int backsprite;

    public Pause(GameObject Obj)
    {
        obj = Obj;
    }

    public override Component Clone(GameObject newObj)
    {
        obj = newObj;

        return new Pause(newObj);


    }

    public override void Update(float dt, KeyboardState input)
    {
        if (firstis == false)
        {
            backsprite = obj.game.LoadTexture("back_butt.png");
            firstis = true;
        }

        if (firsty == false)
        {
            backbutt = new GameObject { X = obj.X, Y = obj.Y, Width = 1.8f, Height = 0.9f, Texture = backsprite, textname = "back_butt.png", ObjectName = "bock" };
            backbutt.Comp.Add(new GameButton(backbutt.Width, backbutt.Height, obj.game, backbutt));
            obj.game.IOUjects.Add(backbutt);
            firsty = true;
        }
        
        
        backbutt.X = obj.X;
        backbutt.Y = obj.Y;

        if (backbutt.GetComponent<GameButton>().hovering == false)
        {
            backbutt.Width = 1.8f;
            backbutt.Height = 0.9f;
        }
        else
        {
            backbutt.Width = 1.5f;
            backbutt.Height = 0.6f;
        }

        if (backbutt.GetComponent<GameButton>().clicking == true)
        {
            GameObject Player = obj.game.objects.FirstOrDefault(x => x.ObjectName == "Player");
            obj.game.pause = false;
            Player.GetComponent<Player>().resetcount();
            obj.game.newdirect = "Scenes";
            obj.game.newindex = "LvlSlctr";
            
        }

        if (obj.game.pause == true)
        {
            if (obj.X > obj.game.CameraPos.X)
            {
                obj.X -= 7 * dt;
            }
        }
        else
        {
            if (obj.X < obj.game.CameraPos.X + 2)
            {
                obj.X += 7 * dt;
            }
        }
    }

}
