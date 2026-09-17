using OpenTK.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static StbVorbisSharp.StbVorbis;

internal class Player : Component
{
    public float speed;
    public GameObject obj;
    Rigidbody rb;
    float Jump = 1.1f;
    public bool OnGround;
    public float Width;
    
    public Game game;
    static int beanzidleTexture;
    static int beanzwalkTexture;
    static int beanzjumpTexture;
    static int beanzfallTexture;
    static public bool animgot;
    public bool preG;
    public bool thisG;
    public bool checkfall;
    public List<bool> tenground = new();
    public int flip = 1;
    public static int Newfriend;
    public int remainingjumps = 3;
    public bool alive = true;
    public static int tres;
    public static int duo;
    public static int uno;
    public static int zro;
    public static int infy;
    static GameObject Tricount = new GameObject();
    static GameObject Triclock = new GameObject();
    public Vector2 Startpos;
    public bool poset = false;
    public static int animint;
    float overlapX;
    float overlapY;
    public static int trik;
    public float tritime;
    public bool CanStomp;
    public static int deads;
    public bool didjump;
    public int chara;
    static int sarkidleTexture;
    static int sarkwalkTexture;
    static int sarkjumpTexture;
    static int sarkfallTexture;
    public int switchint;
    public Vector2 Prejump;
    public GameObject lockk;
    public static int tril;
    public float finishtime;
    public bool finish;
    public GameObject repdoor;
    public Vector2 endpos;
    public List<string> availchar = new();
    static int pastidleTexture;
    static int pastwalkTexture;
    static int pastjumpTexture;
    static int pastfallTexture;
    static int pound;
    public string path;
    public string[] lines;
    public static int lvlcount;
    public int safeframes;
    public bool addfriends;
    public string moove;
    int prevjump;
    public bool landone;
    public static int Stomp;
    public static int boynce;
    public static int finishy;
    public static int BG;
    public static int dead;
    public float songtime = 0;
    public bool justdead;
    public GameObject torso;
    public static List<int> torsoes = new();
    public float torso_to_leg;
    public float ykeep;
    public float xkeep;
    public bool stomping;
    public float ogspeed;
    public bool boostin;
    public float recap;



    public Player(float sped, GameObject ojb)
    {
        speed = sped;
        ogspeed = sped;
        obj = ojb;
        Console.WriteLine("Player work");
        var bc = obj.GetComponent<BoxCollider>();
        if (bc != null)
            bc.Collider = true;
        Console.WriteLine(obj);
        Width = obj.Width;
        availchar.Add("Beanzy");
        
        



    }
    
    public override Component Clone(GameObject newObj) 
    {
        obj = newObj;
        
        return new Player(speed, obj);

        
    }
    public void resetcount()
    {
        lvlcount = 0;
    }
    public override void Update(float dt, KeyboardState input)
    {
        switchint += 1;
        FrameEventArgs arg = obj.game.Arg;
        

        safeframes += 1;
        if (finish == true)
        {
            songtime = 245;
            GameObject repdoor = obj.game.objects.FirstOrDefault(opc => opc.ObjectName == "repd");
            finishtime += (float)arg.Time;
            obj.X = endpos.X;
            obj.Y = endpos.Y;
        }

        if (justdead == false && alive == false)
        {
            obj.game.Audio.PlaySound(dead);
            justdead = true;
        }

        if (finishtime >= 0.4f)
        {

            repdoor.Width += 0.2f;
            repdoor.Height += 0.2f;
        }

        if (finishtime >= 1.5f)
        {
            obj.game.newdirect = "Scenes";
            obj.game.newindex = "LvlSlctr";

        }

        if (animgot == false)
        {
            Console.WriteLine("s");
            Console.WriteLine(obj.X + ", " + obj.Y);
            Startpos.X = obj.X;
            Startpos.Y = obj.Y;
            //Audio
            Newfriend = obj.game.Audio.LoadOgg("New_Friendly.ogg");
            

            //Animations
            obj.GetComponent<Animation>().Width = Width;
            beanzidleTexture = obj.game.LoadTexture("JustStand.png");
            beanzwalkTexture = obj.game.LoadTexture("Justwalk.png");
            beanzjumpTexture = obj.game.LoadTexture("JustJump.png");
            beanzfallTexture = obj.game.LoadTexture("Justfall.png");
            pound = obj.game.LoadTexture("fallstomp.png");

            sarkidleTexture = obj.game.LoadTexture("Sark_stand.png");
            sarkwalkTexture = obj.game.LoadTexture("Sark_sheet.png");
            sarkjumpTexture = obj.game.LoadTexture("Sark_jump.png");
            sarkfallTexture = obj.game.LoadTexture("Sark_fall.png");

            pastidleTexture = obj.game.LoadTexture("Pastel_standy.png");
            pastwalkTexture = obj.game.LoadTexture("Pastel_walksheet.png");
            pastjumpTexture = obj.game.LoadTexture("Pastel_jumpy.png");
            pastfallTexture = obj.game.LoadTexture("Pastel_fally.png");
            tres = obj.game.LoadTexture("Tri-3.png");
            duo = obj.game.LoadTexture("Tri-2.png");
            uno = obj.game.LoadTexture("Tri-1.png");
            zro = obj.game.LoadTexture("Tri-0.png");
            infy = obj.game.LoadTexture("Tri-inf.png");
            trik = obj.game.LoadTexture("Tri_Clock.png");
            deads = obj.game.LoadTexture("TriDead.png");
            tril = obj.game.LoadTexture("Trilock.png");

            torsoes.Add(obj.game.LoadTexture("Beanzy_torso.png"));
            torsoes.Add(obj.game.LoadTexture("pastel_torso.png"));
            //Other



            animgot = true;
        }

        if (obj.game.currentevent == "addpastel")
        {
            availchar.Add("Pastel");
            obj.game.currentevent = "null";
        }
        else if (obj.game.currentevent == "addsark")
        {
            availchar.Add("Sark");
            obj.game.currentevent = "null";
        }

        
        


        if (obj.GetComponent<BoxCollider>().coll.Up < -1.5f)
        {
            alive = false;
        }

        if (alive == false)
        {
            
           
            obj.GetComponent<Animation>().frames = 1;
            obj.GetComponent<Animation>().column = 1;
            obj.GetComponent<Animation>().row = 1;
            obj.Texture = deads;

        }

        preG = thisG;
        thisG = checkfall;

        if (addfriends == false)
        {
            if (obj.game.pasteladd == true)
            {
                availchar.Add("Pastel");

            }
            if (obj.game.sarkadd == true == true)
            {
                availchar.Add("Sark");

            }
            addfriends = true;
        }

        tenground.Add(OnGround);
        if (tenground.Count > 7)
        {
            tenground.Remove(tenground[0]);
        }

        checkfall = tenground.Contains(true);

        if (obj.GetComponent<BoxCollider>().PE_DOWN == true)
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }

        


        if (input.IsKeyDown(Keys.D) && alive == true && ((obj.game.textrender == false) || (obj.game.Index == "Menu")))
        {
            moove = "Right";
            if (obj.GetComponent<Rigidbody>().XVelocity < speed && checkfall == false)
            {
                obj.GetComponent<Rigidbody>().XVelocity += 0.0035f;
            }
            else
            {
                obj.GetComponent<Rigidbody>().XVelocity = speed;
            }
                
            flip = 1;
            boostin = false;
        }
        else
        {
            if (moove == "Right" && obj.GetComponent<Rigidbody>().XVelocity > 0 && obj.GetComponent<Record>().play == false)
            {
                if (obj.GetComponent<Rigidbody>().XVelocity != 0)
                {
                    obj.GetComponent<Rigidbody>().XVelocity -= recap;
                }
            }
            else if (moove == "Left" && obj.GetComponent<Rigidbody>().XVelocity < 0 && obj.GetComponent<Record>().play == false)
            {
                if (obj.GetComponent<Rigidbody>().XVelocity != 0)
                {
                    obj.GetComponent<Rigidbody>().XVelocity += recap;
                }
            }
            else
            {
                if (boostin == false && obj.GetComponent<Record>().play == false)
                {
                    obj.GetComponent<Rigidbody>().XVelocity = 0;
                }
                
                
            }
        }

        if (checkfall == false)
        {
            recap = 0.00045f;
            speed = ogspeed + 0.11f;
        }
        else
        {
            recap = 0.005f;
            speed = ogspeed;
        }

        if (checkfall == false && input.IsKeyDown(Keys.S) && stomping == false)
        {
            ykeep = obj.GetComponent<Rigidbody>().YVelocity;
            xkeep = obj.GetComponent<Rigidbody>().XVelocity;
            obj.GetComponent<Rigidbody>().XVelocity = 0;
            stomping = true;
        }
        else if (checkfall == true && input.IsKeyDown(Keys.S) && stomping == false)
        {
            obj.GetComponent<Rigidbody>().XVelocity = 0;
        }

        if (stomping == true)
        {
            obj.GetComponent<Rigidbody>().YVelocity = -1.8f;
            obj.GetComponent<Rigidbody>().XVelocity = 0;
        }

        if (checkfall == true && stomping == true)
        {
            obj.GetComponent<Rigidbody>().XVelocity = 0;
            stomping = false;

        }
        

        obj.GetComponent<Rigidbody>().OnFall = (OnGround == false);

        if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Trigger"))
        {
            
            if (obj.otherobj != null)
            {
                obj.otherobj.Y += 10;
            }
            
            obj.game.Trig = true;
        }
        GameObject keysleft = obj.game.objects.FirstOrDefault(opc => opc.ObjectName == "key");
        if (obj.otherobj != null)
        {
            overlapX = Math.Min(obj.GetComponent<BoxCollider>().coll.Right, obj.otherobj.GetComponent<BoxCollider>().coll.Right) - Math.Max(obj.GetComponent<BoxCollider>().coll.Left, obj.otherobj.GetComponent<BoxCollider>().coll.Left);
            overlapY = Math.Min(obj.GetComponent<BoxCollider>().coll.Up, obj.otherobj.GetComponent<BoxCollider>().coll.Up) - Math.Max(obj.GetComponent<BoxCollider>().coll.Down, obj.otherobj.GetComponent<BoxCollider>().coll.Down);

            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Tristart") && obj.otherobj.GetComponent<BoxCollider>().coll.Down < obj.GetComponent<BoxCollider>().coll.Up)
            {
                
                if (overlapX > overlapY)
                {
                    Console.WriteLine(overlapX + ", " + overlapX);
                    remainingjumps = 4;
                }
       
            }

            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "PushP") && obj.GetComponent<BoxCollider>().coll.Down < obj.otherobj.GetComponent<BoxCollider>().coll.Up && safeframes > 5)
            {
                if (obj.otherobj.ObjectName.Contains("purp"))
                {
                    if (overlapX > overlapY)
                    {
                        obj.GetComponent<Rigidbody>().OnFall = true;
                        obj.game.Audio.PlaySound(boynce);
                        obj.GetComponent<Rigidbody>().YVelocity = 1.2f;

                        obj.otherobj.Width = 0.6f;
                        obj.otherobj.Height = 0.6f;

                        didjump = false;
                        CanStomp = true;
                        safeframes = 0;
                    }
                }
                

            }

            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Restart") && obj.GetComponent<BoxCollider>().coll.Down < obj.otherobj.GetComponent<BoxCollider>().coll.Up)
            {

                if (overlapX > overlapY)
                {
                    tritime = obj.game.totaltime;
                }

            }

            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Hurt"))
            {
                alive = false;
            }

            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Key"))
            {
                if (obj.otherobj.ObjectName == "key")
                {
                    obj.game.rejjects.Add(obj.otherobj);
                }
                
                
            }

            
            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Door") && keysleft == null && finish == false)
            {
                lvlcount = 0;
                repdoor = new GameObject { X = obj.game.Door.X, Y = obj.game.Door.Y, Width = 0.8f, Height = 1.2f, Texture = obj.game.Door.Texture, textname = obj.game.Door.textname, ObjectName = "repd" };
                obj.game.IOUjects.Add(repdoor);
                endpos.X = obj.X;
                endpos.Y = obj.Y;
                finish = true;
                obj.game.checkLVL();
                obj.game.Audio.PlaySound(finishy);
            }

            if (obj.GetComponent<BoxCollider>().TouchingObjects.Any(x => x.Tag == "Cloud") && keysleft == null)
            {
                lvlcount += 1;
                obj.game.newdirect = "Scenes/Levels/SideLevels";
                obj.game.newindex = obj.game.Index + "." + lvlcount;
                finish = true;
            }

            obj.GetComponent<BoxCollider>().TouchingObjects.Clear();
        }
        
        

        if (preG == false && thisG == true)
        {
            if (remainingjumps > 0)
            {

                if (CanStomp == false && didjump == true && boostin == false)
                {
                    obj.game.Audio.PlaySound(Stomp);
                    Tricount.Width = 2.2f;
                    Tricount.Height = 2.2f;
                    landone = true;
                    remainingjumps -= 1;
                    didjump = false;


                }
                CanStomp = false;
            }
            else
            {
                if (didjump == true && obj.game.infjump == false)
                {
                    alive = false;
                }

            }
            

        }

        if (obj.GetComponent<BoxCollider>().coll.Left < -1)
        {
            obj.X = -1 + (obj.GetComponent<BoxCollider>().Width / 10);
            boostin = false;
            obj.GetComponent<Rigidbody>().XVelocity = 0.2f;
            moove = "Right";
        }

        if (obj.GetComponent<BoxCollider>().coll.Right > (obj.game.Faright + 1))
        {
            obj.X = (obj.game.Faright+1) - (obj.GetComponent<BoxCollider>().Width / 10);
            boostin = false;
            obj.GetComponent<Rigidbody>().XVelocity = -0.2f;
            moove = "Left";
        }


        if (obj.game.currentevent == "Basic")
        {
            obj.X += 0.35f;
            obj.Y += 0.15f;
            obj.game.currentevent = "null";
        }


        if (input.IsKeyDown(Keys.Z) && alive == true && switchint >= 100)
        {
            switchint = 0;
            if (chara < availchar.Count-1)
            {
                chara += 1;
            }
            else
            {
                chara = 0;
            }
        }

        if (input.IsKeyDown(Keys.T) && checkfall == true && obj.game.Door != null && switchint >= 100)
        {
            if (obj.game.Door.ObjectName == "cloudthing")
            {
                lvlcount += 1;
                obj.game.newdirect = "Scenes/Levels/SideLevels";
                obj.game.newindex = obj.game.Index + "." + lvlcount;
                Console.WriteLine(obj.game.newindex);
                finish = true;
            }
            else if (obj.game.Door.ObjectName == "Doorthing")
            {
                lvlcount = 0;
                repdoor = new GameObject { X = obj.game.Door.X, Y = obj.game.Door.Y, Width = 0.8f, Height = 1.2f, Texture = obj.game.Door.Texture, textname = obj.game.Door.textname, ObjectName = "repd" };
                obj.game.IOUjects.Add(repdoor);
                endpos.X = obj.X;
                endpos.Y = obj.Y;
                finish = true;
            }
        }

        if (input.IsKeyDown(Keys.A) && alive == true && ((obj.game.textrender == false) || (obj.game.Index == "Menu")))
        {
            moove = "Left";
            if (obj.GetComponent<Rigidbody>().XVelocity > -speed && checkfall == false)
            {
                obj.GetComponent<Rigidbody>().XVelocity -= 0.0035f;
            }
            else
            {
                obj.GetComponent<Rigidbody>().XVelocity = -speed;
            }
            
            obj.GetComponent<Animation>().Width = -Width;
            flip = -1;
            boostin = false;
        }

        if (remainingjumps == 0 && alive == true && obj.game.infjump == false)
        {
            obj.game.shake = true;
        }
        else
        {
            if (obj.game.Index != "1Lvl3.1")
            {
                obj.game.shake = false;
            }
            
        }

        

        if (input.IsKeyDown(Keys.M) && finish == false && landone == true)
        {
            obj.GetComponent<Rigidbody>().YVelocity = 0;
            CanStomp = true;
            didjump = false;
            obj.X = Prejump.X;
            obj.Y = Prejump.Y;
            remainingjumps = prevjump;
        }

        if (input.IsKeyDown(Keys.W) && obj.GetComponent<Rigidbody>().OnFall == false && alive == true && ((obj.game.textrender == false) || (obj.game.Index == "Menu")))
        {
            prevjump = remainingjumps;
            Prejump.X = obj.X;
            Prejump.Y = obj.Y;
            didjump = true;
            obj.GetComponent<Rigidbody>().OnFall = true;
           
            obj.GetComponent<Rigidbody>().YVelocity = Jump;

            //obj.game.Audio.PlaySound(Newfriend);
        }

        

        if (input.IsKeyDown(Keys.R) && obj.game.Index != "Menu" && obj.game.Index != "Lvl10")
        {
            obj.game.Reload = true;
        }

        if (obj.GetComponent<Rigidbody>().XVelocity != 0 && checkfall == true && alive == true)
        {
            obj.GetComponent<Animation>().animatespeed = 0.05f;
            obj.GetComponent<Animation>().fixwidth = 0.3f * flip;
            obj.GetComponent<Animation>().Height = 0.8f;
            obj.axisloop = true;
            obj.framelimit = 340;
            obj.Changewid = false;
            obj.Changehgt = false;

            torso_to_leg = 0.015f;
                obj.GetComponent<Animation>().Width = 0.3f * flip;
                obj.GetComponent<Animation>().Height = 0.8f;
                obj.GetComponent<Animation>().column = 5;
                obj.GetComponent<Animation>().row = 2;
                obj.Texture = beanzwalkTexture;
            
            

            obj.GetComponent<Animation>().frames = 10;
            
            
        }
        else if (obj.GetComponent<Rigidbody>().XVelocity == 0 && checkfall == true && alive == true)
        {
            obj.GetComponent<Animation>().fixwidth = 0.3f * flip;
            obj.GetComponent<Animation>().Height = 0.8f;
            obj.axisloop = true;
            obj.framelimit = 340;
            obj.Changewid = true;
            obj.Changehgt = false;
            
            obj.newwidth = 0.22f;
            obj.axispeed = 0.0001f;
              
            obj.Texture = beanzidleTexture;
            torso_to_leg = 0.015f;

            obj.GetComponent<Animation>().frames = 1;
            obj.GetComponent<Animation>().column = 1;
            obj.GetComponent<Animation>().row = 1;
            
        }
        else if (obj.GetComponent<Rigidbody>().YVelocity < 0 && checkfall == false && alive == true && stomping == false)
        {


            torso_to_leg = 0.015f;
            obj.GetComponent<Animation>().Width = 0.3f * flip;
                obj.GetComponent<Animation>().Height = 0.8f;
                obj.Texture = beanzfallTexture;
            
            
            obj.GetComponent<Animation>().frames = 1;
            obj.GetComponent<Animation>().column = 1;
            obj.GetComponent<Animation>().row = 1;
            
        }
        else if (obj.GetComponent<Rigidbody>().YVelocity > 0 && checkfall == false && alive == true)
        {
            obj.axisloop = false;
            obj.Changewid = false;
            obj.Changehgt = true;
            obj.newheight = 1.9f;
            obj.axispeed = 0.015f;
            obj.framelimit = 100;

            obj.GetComponent<Animation>().Width = 0.41f * flip;
            obj.GetComponent<Animation>().fixwidth = 0.7f * flip;
            obj.GetComponent<Animation>().Height = 0.75f;
            obj.Texture = beanzjumpTexture;

            torso_to_leg = 0.018f;
            obj.GetComponent<Animation>().frames = 1;
            obj.GetComponent<Animation>().column = 1;
            obj.GetComponent<Animation>().row = 1;
           
        }
        else if (obj.GetComponent<Rigidbody>().YVelocity < 0 && checkfall == false && alive == true && stomping == true)
        {


            torso_to_leg = 0.015f;
            obj.GetComponent<Animation>().Width = 0.3f * flip;
            obj.GetComponent<Animation>().Height = 0.8f;
            obj.Texture = pound;


            obj.GetComponent<Animation>().frames = 1;
            obj.GetComponent<Animation>().column = 1;
            obj.GetComponent<Animation>().row = 1;

        }






        if ((obj.X != Startpos.X || obj.Y != Startpos.Y) && poset == false)
        {
            tritime = obj.game.totaltime;
            Stomp = obj.game.Audio.LoadOgg("BOP.ogg");
            boynce = obj.game.Audio.LoadOgg("Bounce.ogg");
            finishy = obj.game.Audio.LoadOgg("FINISH.ogg");
            BG = obj.game.Audio.LoadOgg("BGMusic.ogg");
            dead = obj.game.Audio.LoadOgg("Death.ogg");
            torso = new GameObject(){ X = obj.X, Y = obj.Y, Width = obj.Width, Height = 0.5f, Texture = beanzfallTexture, textname = "Yams!.png", ObjectName = "Yamss" };
            obj.game.IOUjects.Add(torso);

            Tricount = new GameObject { X = -0.85f, Y = 0.45f, Width = 1.2f, Height = 1.2f, Texture = tres, textname = "Tri-3.png", ObjectName = "tric" };
            Tricount.game = obj.game;
            Tricount.UI = true;
            obj.game.IOUjects.Add(Tricount);

            Triclock = new GameObject { X = -0.85f, Y = 0.75f, Width = 1.2f, Height = 1.2f, Texture = trik, textname = "Tri_Clock.png", ObjectName = "tric" };
            Triclock.game = obj.game;
            Triclock.UI = true;
            obj.game.IOUjects.Add(Triclock);

            if (obj.game.Door != null)
            {
                lockk = new GameObject { X = obj.game.Door.X, Y = obj.game.Door.Y, Width = 0.5f, Height = 0.8f, Texture = tril, textname = "Trilock.png", ObjectName = "lock" };
                lockk.game = obj.game;
                obj.game.IOUjects.Add(lockk);
            }
            

            Console.WriteLine("JAJAJAJAJAJAJAJAAJAJAJAJAJAJJAJAAJAJAJJAAJJAJAJAJAJA");
            obj.X = obj.game.PlayerV.X;
            obj.Y = obj.game.PlayerV.Y;
            obj.GetComponent<Rigidbody>().YVelocity = 0;
            poset = true;

            if (lockk != null)
            {
                if (keysleft != null)
                {
                    lockk.Render = true;
                }
                else
                {
                    lockk.Render = false;
                }
            }
            
        }
        if (torso != null)
        {
            torso.Texture = torsoes[chara];
            torso.Width = 0.24f * flip;
            torso.Height = 0.6f;
            torso.X = obj.X - (0.006f * flip);
            torso.Y = obj.Y + torso_to_leg;
            torso.Render = alive;
        }

        
        

        if (Tricount.Width > 1.2 && Tricount.Height > 1.2)
        {
            Tricount.Width -= 0.02f;
            Tricount.Height -= 0.02f;
        }

        if (Tricount.Width < 1.2 && Tricount.Height < 1.2)
        {
            Tricount.Width = 1.2f;
            Tricount.Height = 1.2f;
        }

        if (lockk != null)
        {
            if (keysleft != null)
            {
                lockk.Render = true;
            }
            else
            {
                lockk.Render = false;
            }

        }

        if (obj.game.textrender == false || (obj.GetComponent<Rigidbody>().YVelocity != 0 && obj.GetComponent<Rigidbody>().XVelocity != 0))
        {
            if (obj.game.textrender == true && (obj.GetComponent<Rigidbody>().YVelocity != 0 && obj.GetComponent<Rigidbody>().XVelocity != 0))
            {
                tritime -= ((float)arg.Time * 2);
            }
            else
            {
                tritime -= (float)arg.Time;
            }
            
        }
        

        if (obj.game.inftime == true)
        {
            tritime = 100;
        }

        float newidth = ((tritime * 1.2f) / obj.game.totaltime);
        
        if (newidth > 0)
        {
            Triclock.Width = newidth;
            Triclock.Height = newidth;
        }

        if (tritime <= 0 && poset == true)
        {
            alive = false;
        }

        if (obj.game.infjump == false)
        {
            if (remainingjumps == 3)
            {
                Tricount.Texture = tres;
            }
            else if (remainingjumps == 2)
            {
                Tricount.Texture = duo;
            }
            else if (remainingjumps == 1)
            {
                Tricount.Texture = uno;
            }
            else if (remainingjumps <= 0)
            {
                Tricount.Texture = zro;
            }
        }
        else
        {
            Tricount.Texture = infy;
        }



        


    }



    
}

