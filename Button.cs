using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

 class Button : Component
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
    public Editor ed;
    public bool hovering;
    public bool clicking;
    public bool hasclicked;
    public bool canclick = true;
    public bool right;
    public string ButtonName;


    public Button(int texture, string textnames, float X, float Y, float Width, float Height, Editor edd)
    {
        x = X;
        y = Y;
        wid = Width;
        hight = Height;
        ed = edd;
        Texture = texture;
        textname = textnames;
    }

    public void Update(KeyboardState input)
    {
        Left = (x - (wid / 10));
        Right = (x + (wid / 10));
        Up = (y + (hight / 10));
        Down = (y - (hight / 10));
        
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


        
    }

    public void OnMouseDown()
    {
        
        if (hovering == true && canclick)
        {
            Console.WriteLine("Cliked");
            hasclicked = !hasclicked;
            clicking = true;
            
        }
        
    }
    public void OnHover()
    {

    }
}

