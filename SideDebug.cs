using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class SideDebug : Component
{
    GameObject obj;
    GameObject P;
    string Side;

    public SideDebug(GameObject oqj, GameObject p, string side)
    {
        obj = oqj;
        P = p;
        Side = side;
    }

    public override void Update(float dt, KeyboardState input)
    {
        if (Side == "Left")
        {
            obj.X = P.GetComponent<BoxCollider>().coll.Left;
        }
        else if (Side == "Right")
        {
            obj.X = P.GetComponent<BoxCollider>().coll.Right;
        }
        else if (Side == "Up")
        {
            obj.Y = P.GetComponent<BoxCollider>().coll.Up;
        }
        else if (Side == "Down")
        {
            obj.Y = P.GetComponent<BoxCollider>().coll.Down;
        }
        else if (Side == "Center")
        {
            obj.X = P.X;
            obj.Y = P.Y;
        }

    }
}

