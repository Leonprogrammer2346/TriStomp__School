using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;


class BoxCollider : Component
{
    
    public float Left;
    public float Right;
    public float Up;
    public float Down;
    public GameObject obj;
    Detection dect;
    public Coll_Object coll;
    public string Tag;
    public List<string> Othertag = new();
    public float Width;
    public float Height;
    public bool Collider = false;
    public Rigidbody rb;
    public bool statc = true;
    public bool ongame = false;
    public List<bool> Downoff = new();
    public bool PE_UP;
    public bool PE_DOWN;
    public bool PE_LEFT;
    public bool PE_RIGHT;
    public float altWidth;
    public float altHeight;
    public bool AltDimensions = false;
    List<Coll_Object> Unstatic_ob = new();
    List<Coll_Object> Full_X = new();
    List<Coll_Object> Potential_Y = new();
    public float Lef;
    public float Upp;
    public float Rihgt;
    public float Domn;
    public bool first_radio = false;
    public bool first_radio1 = false;
    public bool Check_Others;
    public bool Lasttouched;
    public List<Coll_Object> TouchingObjects = new List<Coll_Object>();
    public List<string> onlylist = new List<string>();
    public bool only;

    public BoxCollider(float W, float H, GameObject ojb, Detection det, string Tg, bool stat, bool check)
    {
        obj = ojb;
        Left = obj.X - (W / 8);
        Right = obj.X + (W / 8);
        Tag = Tg;
        Up = obj.Y + (H / 8);
        Down = obj.Y - (H / 8);
        coll = new Coll_Object(Tg, Left, Right, Up, Down, det, this);
        obj.Comp.Add(coll);
        obj.Comp.Add(det);
        Width = W;
        Height = H;
        dect = det;
        statc = stat;
        Check_Others = check;




        foreach (var sip in obj.Comp)
        {
            Console.WriteLine(obj + " is in me!");
        }

    }

    public override Component Clone(GameObject newObj)
    {
        return new BoxCollider(Width, Height, newObj, dect, Tag, statc, Check_Others);
        {
            Collider = newObj.GetComponent<BoxCollider>().Collider;
            ongame = newObj.GetComponent<BoxCollider>().ongame;
            
            onlylist = newObj.GetComponent<BoxCollider>().onlylist;
        }
        ;
    }
    public bool TagTouch(string other)
    {
        return Othertag.Contains(other);
    }

    public void CombineColl()
    {

        Unstatic_ob = dect.fobject.Where(obj => obj.Box.statc == false).ToList();
        var Isnt_Top = Unstatic_ob.FirstOrDefault(obb => obb.Box.obj.X == obj.X && obb.Box.obj.Y == (obj.Y + 0.065f));
        var Isnt_Left = Unstatic_ob.FirstOrDefault(obb => obb.Box.obj.Y == obj.Y && obb.Box.obj.X == (obj.X - 0.065f));
        int Wid_Length;
        
        if (Isnt_Left == null && Isnt_Top == null && statc == false && dect.objects.Contains(coll))
        {
            
            
            int heightval = 1;
            bool Find_Width = false;
            bool Find_Height = false;
            Lef = coll.Left;
            Upp = coll.Up;
            var Next_X = coll;
            var Under_Y = coll;
            Full_X.Add(coll);
            while (Find_Width == false)
            {
                Next_X = Unstatic_ob.FirstOrDefault(obb => obb.Box.obj.Y == Next_X.Box.obj.Y && obb.Box.obj.X == (Next_X.Box.obj.X + 0.065f));
                
                if (Next_X == null)
                {
                    Find_Width = true;
                }
                else
                {
                  
                    Full_X.Add(Next_X);
                    Console.WriteLine("Ramen");
                    dect.objects.Remove(Next_X);    
                }
                Rihgt = Full_X[Full_X.Count - 1].Right;
            }
            if (first_radio == false)
            {
                Console.WriteLine(Full_X.Count);
                Console.WriteLine("By");
            }
            

            while (Find_Height == false)
            {
                for (int i=0; i < Full_X.Count; i++)
                {
                    Under_Y = Unstatic_ob.FirstOrDefault(obb => obb.Box.obj.X == Full_X[i].Box.obj.X && obb.Box.obj.Y == (Full_X[i].Box.obj.Y - 0.065f));
                    if (Under_Y == null)
                    {
                        Find_Height = true;
                    }
                    else
                    {
                        Potential_Y.Add(Under_Y);
                        
                    }
                }

                if (Potential_Y.Count == Full_X.Count)
                {
                    Domn = Potential_Y[0].Down;
                    Full_X = Potential_Y.ToList();
                    foreach (var pot in Potential_Y)
                    {
                        heightval += 1;
                        dect.objects.Remove(pot);
                    }
                }
                Potential_Y.Clear();
            }
            if (first_radio == false)
            {
                Console.WriteLine(heightval);
                Console.WriteLine("---");
                first_radio = true;
            }
            
            Full_X.Clear();
            AltDimensions = true;
        }
        
    }

    public override void Update(float dt, KeyboardState input)
    {

        



        if (Downoff.Count == 11)
        {
            Downoff.RemoveAt(0);
        }

        bool check = true;
        if (Collider == true && check)
        {
            rb = obj.GetComponent<Rigidbody>();
            check = false;
        }

        if (AltDimensions == false)
        {
            coll.Left = obj.X - (Width / 10);
            coll.Down = obj.Y - (Height / 10);
            coll.Right = obj.X + (Width / 10);
            coll.Up = obj.Y + (Height / 10);
        }
        else
        {
            coll.Left = Lef;
            coll.Down = Domn;
            coll.Right = Rihgt;
            coll.Up = Upp;
        }

    }
}

