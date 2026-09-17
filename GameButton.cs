using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

class GameButton : Component
{
    public int Texture;
    public string textname;
    public float x;
    public float y;
    public float wid;
    public float hight;
    public float Left;
    public float Right;
    public float Up;
    public float Down;
    public Vector2 Mousepos;
    public Game ed;
    public bool hovering;
    public bool clicking;
    public bool hasclicked;
    public bool canclick = true;
    public bool right;
    public string ButtonName;
    public GameObject Obj;
    public int clickframes;

    public GameButton(float Width, float Height, Game edd, GameObject obj)
    {
        
        wid = Width;
        hight = Height;
        ed = edd;
        Obj = obj;
    }

    public override Component Clone(GameObject newObj)
    {
        return new GameButton(wid, hight, ed, newObj);
        
    }

    public override void Update(float dt, KeyboardState input)
    {
        Obj.UI = true;
        Left = (Obj.X - (wid / 10));
        Right = (Obj.X + (wid / 10));
        Up = (Obj.Y + (hight / 10));
        Down = (Obj.Y - (hight / 10));

        Mousepos.X = (ed.MouseX / (float)ed.Size.X) * 2f - 1f;
        Mousepos.Y = 0.9f - (ed.MouseY / (float)ed.Size.Y) * 2f;


        if (Mousepos.X > Left && Mousepos.X < Right && canclick)
        {
            if (Mousepos.Y > Down && Mousepos.Y < Up)
            {
                hovering = true;
                OnHover();
            }
            else
            {
                hovering = false;
            }
        }
        else
        {
            hovering = false;
        }


        if (clicking == true && clickframes < 1)
        {
            clickframes++;
        }
        else
        {
            clicking = false;
            clickframes = 0;
        }
    }

    public void OnMouseDown()
    {

        if (hovering == true && canclick)
        {
            clicking = true;
            Console.WriteLine("Cliked");
            hasclicked = !hasclicked;
            

        }

        
    }
    public void OnHover()
    {

    }
}
