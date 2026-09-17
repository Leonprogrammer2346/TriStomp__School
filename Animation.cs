using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Animation : Component
{

    public int frames;
    public float animatespeed;
    public float frametimer;
    public int currentframe;
    public int column = 1;
    public int row = 1;
    public float Width;
    public float Height;
    public float fixwidth;
    public float fixheight;

    public Animation(int f, float a, float ft, int ct, float w, float h)
    {
        frames = f;
        animatespeed = a;
        frametimer = ft;
        currentframe = ct;
    }

    public override Component Clone(GameObject newObj)
    {

        return new Animation(frames, animatespeed, frametimer, currentframe, Width, Height)
        {
            column = 1,
            row = 1,
            
        };

    }

    public override void Update(float dt, KeyboardState input)
    {
        
        frametimer += dt;
        if (frametimer > animatespeed)
        {
            currentframe += 1;

            if (currentframe >= frames)
            {
                currentframe = 0;
            }
            frametimer = 0;
        }
    }
}
