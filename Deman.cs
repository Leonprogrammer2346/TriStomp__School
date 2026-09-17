using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

internal class Deman : Component
{
    public GameObject obj;
    public float fixeY;
    public float downtween;
    public float uptween;
    public bool aura = true;
    public bool animgot;
    public bool gonup;
    public bool flyaway;
    public float Bottomloc;
    public bool Jeman;
    public static int angryde;
    bool port;
    public Deman(GameObject Obj)
    {
        obj = Obj;
       
    }

    public override Component Clone(GameObject newObj)
    {
        obj = newObj;

        

        return new Deman(obj);
        {
            Jeman = newObj.GetComponent<Deman>().Jeman;
        }


    }

    public override void Update(float dt, KeyboardState input)
    {
        if (animgot == false)
        {
            fixeY = obj.Y;
            downtween = fixeY - 0.04f;
            uptween = fixeY + 0.04f;
            animgot = true;
            Bottomloc = fixeY - 1.3f;
        }

        if (Jeman == false)
        {
            if (obj.game.currentevent == "Demanleave")
            {
                aura = false;
                flyaway = true;
                obj.game.currentevent = "null";
            }

            if (obj.game.currentevent == "Demancome")
            {
                aura = false;
                obj.Y -= 0.01f;
                if (obj.Y <= Bottomloc)
                {
                    fixeY = obj.Y;
                    downtween = fixeY - 0.04f;
                    uptween = fixeY + 0.04f;
                    aura = true;
                    obj.game.currentevent = "null";
                }

            }
        }

        

        if (obj.game.currentevent == "Jemancome")
        {
            if (obj.ObjectName == "Jeman")
            {
                aura = false;
                obj.Y -= 0.01f;
                if (obj.Y <= Bottomloc)
                {
                    fixeY = obj.Y;
                    downtween = fixeY - 0.04f;
                    uptween = fixeY + 0.04f;
                    aura = true;
                    obj.game.currentevent = "null";
                }

            }
            

        }

        if (obj.game.currentevent == "Jemanleave")
        {
            if (obj.ObjectName == "Jeman")
            {
                aura = false;
                obj.X -= 0.001f;
           

            }


        }

        if (obj.game.currentevent == "Jemankidnap")
        {
            if (obj.ObjectName == "Jeman")
            {
                if (port == false)
                {
                    obj.X = -1;
                    port = true;
                }
                obj.Y = -0.59222806f;
                GameObject pastel = obj.game.objects.FirstOrDefault(x => x.ObjectName == "Plastyer");
                aura = false;
                obj.X += 0.005f;
                if (obj.X >= pastel.X)
                {
                    pastel.X = obj.X - 0.00001f;
                }


            }


        }


        if (flyaway == true)
        {
            obj.Y += 0.01f;
        }

        if (aura == true)
        {
            if (gonup == true)
            {
                if (obj.Y < uptween)
                {
                    obj.Y += 0.0001f;
                }
                else
                {
                    gonup = false;
                }
            }
            else
            {
                if (obj.Y > downtween)
                {
                    obj.Y -= 0.0001f;
                }
                else
                {
                    gonup = true;
                }
            }
        }
    }
}
