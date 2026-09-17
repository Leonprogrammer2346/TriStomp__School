using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Rigidbody : Component
{
    float fallweight;
    public float XVelocity;
    public float YVelocity;
    GameObject obj;
    public float Gravity;
    public bool OnFall = true;

    public Rigidbody(GameObject ojb, float fw, float X, float Y)
    {
        fallweight = fw;
        obj = ojb;
        Console.WriteLine("Rigid work");
        foreach (var sip in obj.Comp)
        {
            Console.WriteLine(obj + " is in me!");
        }
    }

    public override Component Clone(GameObject newObj)
    {
        return new Rigidbody(newObj, 1, newObj.X, newObj.Y)
        {
            Gravity = this.Gravity
        };
    }

    public override void Update(float dt, KeyboardState input)
    {
        dt = MathF.Min(dt, 1f / 30f);

        if (dt > 0.04f)
        {
            Console.WriteLine($"LAG FRAME: {dt}");
        }
        if (OnFall == true)
        {
            YVelocity -= Gravity * dt;
        }
        else
        {
            YVelocity = 0;
        }






        obj.X += XVelocity * dt;
        obj.Y += YVelocity * dt;
    }
}

