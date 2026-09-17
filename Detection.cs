using OpenTK.Graphics.ES11;
using OpenTK.Graphics.ES20;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

class Coll_Object : Component
{
    public string Tag;
    public float Left;
    public float Right;
    public float Up;
    public float Down;
    Detection dect;
    public BoxCollider Box;
    public bool Rcol;
    public bool Lcol;
    public bool Ucol;
    public bool Dcol;
    public bool xint;
    public bool yint;
    public bool zint;
    public bool alrc;
    public float Range = 0.2f;
    public bool Tuch;
    public bool nowtuch;
    public bool pastuch;


    public Coll_Object(string T, float L, float R, float U, float D, Detection dec, BoxCollider box)
    {
        Left = L;
        Right = R;
        Up = U;
        Down = D;
        Tag = T;
        dect = dec;
        Box = box;
        dect.objects.Add(this);
    }

    public override void Update(float dt, KeyboardState input)
    {
        
        pastuch = nowtuch;
        nowtuch = Tuch;

    }

    public string CollisionCheck(Coll_Object other)
    {
        Rcol = Right > other.Left && Right < other.Right;
        Lcol = Left < other.Right && Left > other.Left;
        Ucol = Up > other.Down && Up < other.Up;
        Dcol = Down < other.Up && Down > other.Down;
        xint = Rcol || Lcol;
        yint = Ucol || Dcol;
        zint = Up > other.Up && Down < other.Down;


        Tuch = (xint && (yint || zint));



        

        if (Tuch)
        {
            if (!Box.TouchingObjects.Contains(other))
            {
                Box.TouchingObjects.Add(other);
            }

            if (!other.Box.TouchingObjects.Contains(this))
            {
                other.Box.TouchingObjects.Add(this);
            }

            Box.obj.otherobj = other.Box.obj;

            return other.Tag;
        }
        else
        {
            Box.TouchingObjects.Remove(other);
            other.Box.TouchingObjects.Remove(this);

            if (Box.obj.otherobj == other.Box.obj)
            {
                Box.obj.otherobj = null;
            }

            return null;
        }
    }

    public bool WithinRange(Coll_Object other)
    {
        if (other.Box.obj.X > Box.obj.X-Range && other.Box.obj.X < Box.obj.X+Range)
        {
            if (other.Box.obj.Y > Box.obj.Y - Range && other.Box.obj.Y < Box.obj.Y + Range)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
        
}
class Detection : Component 
{
    public List<Coll_Object> objects = new();
    public List<Coll_Object> fobject = new();
    public float Up;
    public float Down;
    public float Left;
    public float Right;

    public bool downy = true;
    public float gravvy;
    public int CPF;
    public int trigy;
    public List<bool> Ups = new();



    public override void Update(float dt, KeyboardState input)
    {

        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i].Box.ongame == false)
            {
                objects.RemoveAt(i); 
            }
        }
        Ups.Clear();
        ZZYY();
        //Console.WriteLine(trigy);
        trigy = 0;


    }




    

    
    void ZZYY()
    {
        CPF = 0;

        
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].Box.Check_Others == true)
            {
                objects[i].Box.PE_UP = false;
                objects[i].Box.PE_DOWN = false;
                objects[i].Box.PE_LEFT = false;
                objects[i].Box.PE_RIGHT = false;

                for (int y = 0; y < objects.Count; y++)
                {
                    if (objects[i] != objects[y] && (objects[i].Box.onlylist.Count > 0 && objects[i].Box.onlylist.Contains(objects[y].Tag) || (objects[i].Box.onlylist.Count == 0)))
                    {
                        objects[i].CollisionCheck(objects[y]);

                        

                        float overlapX = Math.Min(objects[i].Right, objects[y].Right) - Math.Max(objects[i].Left, objects[y].Left);
                        float overlapY = Math.Min(objects[i].Up, objects[y].Up) - Math.Max(objects[i].Down, objects[y].Down);
                        if ((objects[i].Box.Collider == true && objects[i].Box.rb != null && objects[i].Box.statc == true) && (objects[y].Box.Collider == true && objects[y].Box.rb != null) && objects[i].WithinRange(objects[y]))
                        {

                            Coll_Object uplap = objects.FirstOrDefault(x => x.Box.obj.Y == (objects[y].Box.obj.Y + (0.065f / 2)) && objects[y].Box.obj.X == x.Box.obj.X && x.Box.statc == false);
                            
                            Coll_Object downlap = objects.FirstOrDefault(x => x.Box.obj.Y == (objects[y].Box.obj.Y - (0.065f / 2)) && objects[y].Box.obj.X == x.Box.obj.X && x.Box.statc == false);
                            

                            

                            if (overlapX > overlapY)
                            {
                                if (objects[i].Box.rb.YVelocity < 0 && objects[i].Dcol == true && objects[i].xint && uplap == null)
                                {
                                    
                                    objects[i].Box.obj.Y = objects[y].Up + (objects[i].Box.Height / 10);
                                    objects[i].Box.PE_DOWN = true;
                                    Ups.Add(false);

                                }
                                else if (objects[i].Box.rb.YVelocity > 0 && objects[i].Ucol == true && objects[i].xint && downlap == null)
                                {
                                    objects[i].Box.obj.Y = objects[y].Down - (objects[i].Box.Height / 10);
                                    objects[i].Box.obj.GetComponent<Rigidbody>().YVelocity = 0;
                                    
                                    Ups.Add(true);
                                   
                                }
                                else
                                {
                                    Ups.Add(false);
                                }
                                
                                
                                




                            }
                            else
                            {
                                if (objects[i].Box.rb.XVelocity > 0 && objects[i].Rcol == true && (objects[i].yint || objects[i].zint))
                                {
                                    objects[i].Box.obj.X = objects[y].Left - (objects[i].Box.Width / 9f);
                                    objects[i].Box.PE_RIGHT = true;
                                    Ups.Add(false);
                                }
                                else if (objects[i].Box.rb.XVelocity < 0 && objects[i].Lcol == true && (objects[i].yint || objects[i].zint))
                                {
                                    objects[i].Box.obj.X = objects[y].Right + (objects[i].Box.Width / 9f);
                                    objects[i].Box.PE_LEFT = true;
                                    Ups.Add(false);
                                }
                                

                            }

                        }

                    }
                }
            }
            objects[i].Box.PE_UP = Ups.Contains(true);
            

        }
        

    }




}

