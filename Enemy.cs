using Microsoft.VisualBasic.FileIO;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

internal class Enemy : Component
{
    public float speed;
    public GameObject obj;
    public static bool animgot;
    public static int animat;
    public bool canflip;
    public int flipint;
    public int flip = 1;
    public float ogwid;
    public float oghig;
    public bool fitr;
    public List<bool> leftss = new();
    public List<bool> vertrang = new();
    public float ogspeed;
    public List<float> turn = new();
    public List<float> turnY = new();
    public float newid;

    public Enemy(GameObject Obj, float sped)
    {
        speed = sped;
        ogspeed = speed;
        obj = Obj;
        
    }

    public override Component Clone(GameObject newObj)
    {
        obj = newObj;

        return new Enemy(obj, speed);
    }

    public override void Update(float dt, KeyboardState input)
    {
        
        flipint += 1;
        if (animgot == false)
        {
            animat = obj.Texture;
            ogwid = obj.Width;
            oghig = obj.Height;
            obj.GetComponent<Animation>().Width = ogwid;
            obj.GetComponent<Animation>().Height = oghig;
            obj.GetComponent<Animation>().frames = 2;
            obj.GetComponent<Animation>().column = 2;
            obj.GetComponent<Animation>().row = 1;
            
            animgot = false;
           
        }

        
        int tuknumb = 0;
        if (fitr == false)
        {
            newid = ogwid;
            foreach (var gyup in obj.game.objects)
            {
                if (gyup.ObjectName.Contains("turnthing"))
                {
                    turn.Add(gyup.X);
                    turnY.Add(gyup.Y);
                }
            }

            foreach (var tutk in turn)
            {

                if (tutk < obj.X)
                {
                    leftss.Add(true);
                }
                else if (tutk > obj.X)
                {
                    leftss.Add(false);
                }

                if (obj.Y - turnY[tuknumb] > -1 && obj.Y - turnY[tuknumb] < 1)
                {
                    vertrang.Add(true);
                }
                else
                {
                    vertrang.Add(false);
                }
                tuknumb += 1;
            }
            fitr = true;
        }

        
        obj.GetComponent<Rigidbody>().XVelocity = speed;
        obj.Texture = animat;
        obj.GetComponent<Animation>().Width = newid;
        obj.GetComponent<Animation>().Height = oghig;
        int tindx = 0;
        
        
            foreach (var tutk in turn)
            {
                if (vertrang[tindx] == true)
                {
                    if (obj.X > tutk && leftss[tindx] == false)
                    {
                        speed = -ogspeed;
                        
                        newid = -ogwid;
                        flipint = 0;
                    
                    }
                    else if (obj.X < tutk && leftss[tindx] == true)
                    {
                        speed = ogspeed;

                        newid = ogwid;
                        flipint = 0;
                    
                    }
                    tindx += 1;
                }
            }
        
        

        

        
        
        
    }
}
