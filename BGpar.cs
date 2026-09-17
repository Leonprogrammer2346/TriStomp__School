using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BGpar : Component
{

    public GameObject obj;
    public float mystartx;
    public float startx;
    public float currx;
    public float newx;
    public float movespeed;
    public float rebooX;
    public bool animgot = false;
    public BGpar(GameObject Obj, float speed)
    {
        obj = Obj;
        mystartx = obj.X;
        movespeed = speed;      
    }

    public override Component Clone(GameObject newObj)
    {
        obj = newObj;
        
        return new BGpar(newObj, movespeed);


    }

    public override void Update(float dt, KeyboardState input)
    {
        if (animgot == false)
        {
            startx = obj.game.CameraPos.X;
            rebooX = obj.game.CameraPos.X - mystartx;
            animgot = true;
        }
        currx = obj.game.CameraPos.X;
        newx = startx - currx;
        obj.X = (obj.game.CameraPos.X + rebooX) + (newx * movespeed);
    }

}
