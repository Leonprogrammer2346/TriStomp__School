using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

internal class LevelSelector : Component
{
    public static GameButton button;
    public static GameObject Obj;
    public int levelcount;
    public static GameObject prebutt = new GameObject();
    public static GameObject nexbutt = new GameObject();
    public static GameObject nexw = new GameObject();
    public static GameObject prew = new GameObject();
    public static bool animgot;
    public static int closed;
    public static int yams;
    public static int egg;
    public static int open;
    public static int bef;
    public static int aft;
    public static int nex;
    public static int prev;
    public static int levelindex;
    public static int worldindex = 1;
    public static int Bop;
    public static List<int> bgt = new();
    public int clickwait;
    public bool slot;
    bool GoL;
    bool GoR;
    bool GoU;
    bool GoD;
    bool hasPort;
    public float strsX;
    public float strsY;
    public string waymove = "Down";
    float movespeed = 0.05f;
    public bool Loadyams;
    public float ccelerate = 0.01f;
    public string path;
    public string[] lines;
    public static int maxworld;
    public static int maxlvl;
    public GameObject bg;


    public LevelSelector(GameObject obj)
    {
        Obj = obj;
        strsX = obj.X;
        strsY = obj.Y;
        string folder = "Scenes/Levels";
        Directory.CreateDirectory(folder);
        string[] files = Directory.GetFiles(folder, "*.txt");
        levelcount = files.Length;
        button = obj.GetComponent<GameButton>();

    }

    public override Component Clone(GameObject newObj)
    {
        return new LevelSelector(newObj);

    }

    public void updatemax()
    {
        Directory.CreateDirectory("Scenes");
        path = Path.Combine("Scenes", "LvlPlace.txt");
        File.WriteAllText(path, "");

        if (Obj.game.LvlJBS.Contains("Lvl10"))
        {
            maxworld += 1;
            maxlvl = 1;
            File.AppendAllText(path, maxworld + "/" + 1);
        }
        else
        {
            maxlvl += 1;
            File.AppendAllText(path, maxworld + "/" + maxlvl);
        }

        Obj.game.Reload = true;
            
    }
    public override void Update(float dt, KeyboardState input)
    {
        if (animgot == false)
        {
            bef = Obj.game.LoadTexture("Arrow_L.png");
            aft = Obj.game.LoadTexture("Arrow_R.png");
            nex = Obj.game.LoadTexture("Arrow_U.png");
            prev = Obj.game.LoadTexture("Arrow_D.png");
            yams = Obj.game.LoadTexture("Yamp!.png");
            egg = Obj.game.LoadTexture("egg.png");
            closed = Obj.game.LoadTexture("LevelDoor.png");
            open = Obj.game.LoadTexture("LevelDoor_Open.png");
            bgt.Add(Obj.game.LoadTexture("Tristomp-BG.png"));
            bgt.Add(Obj.game.LoadTexture("W2BG.png"));

            animgot = true;
        }

        if (Loadyams == false)
        {
            Bop = Obj.game.Audio.LoadOgg("BOP.ogg");

            string[] files = Directory.GetFiles("Scenes", "*.txt");
            if (files.Length > 0)
            {
                string filename = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == "LvlPlace");
                lines = File.ReadAllLines(filename);
            }
          
            foreach(string line in lines)
            {
                string[] parts = line.Split('/');
                maxworld = int.Parse(parts[0]);
                maxlvl = int.Parse(parts[1]);
            }
            bg = Obj.game.objects.FirstOrDefault(x => x.ObjectName == "BGm");
            int whint = 0;
            for (int i=0; i< levelindex; i++)
            {
                GameObject newyam = new GameObject { X = -0.3f + (0.05f * whint), Y = 0.5f, Width = 0.8f, Height = 0.8f, Texture = yams, textname = "Yams!.png", ObjectName = "Yamss" };
                Obj.game.IOUjects.Add(newyam);
                whint += 1;
            }
            int eit = 0;
            for (int i = 0; i < worldindex; i++)
            {
                GameObject newegg = new GameObject { X = -0.6f, Y = -0.3f + (0.05f * whint), Width = 0.8f, Height = 0.8f, Texture = egg, textname = "Yams!.png", ObjectName = "Eff" };
                Obj.game.IOUjects.Add(newegg);
                eit += 1;
            }
            Loadyams = true;
        }

        if (Obj.game.LvlJBS.Contains(maxworld + "Lvl" + maxlvl))
        {
            updatemax();
            Obj.game.LvlJBS = "null";
        }

        Obj.Y -= movespeed * dt;

        
        movespeed -= ccelerate * dt;
        if (movespeed <= -0.05f)
        {
            ccelerate = -0.01f;
        }
        else if (movespeed >= 0.05f)
        {
            ccelerate = 0.01f;
        }

        if (GoL == true)
        {
            Obj.X += 5 * dt;
            if (Obj.X >= strsX + 1 && hasPort == false)
            {
                hasPort = true;
                Obj.X = -1.5f;
            }

            if (hasPort == true && Obj.X > strsX)
            {
                GoL = false;
                GoR = false;
                hasPort = false;
                Obj.X = strsX;
            }
        }


        if (GoR == true)
        {
            Obj.X -= 5 * dt;
            if (Obj.X <= strsX - 1 && hasPort == false)
            {
                hasPort = true;
                Obj.X = 1.5f;
            }

            if (hasPort == true && Obj.X < strsX)
            {
                GoL = false;
                GoR = false;
                hasPort = false;
                Obj.X = strsX;
            }
        }

        if (GoD == true)
        {
            Obj.Y -= 5 * dt;
            if (Obj.Y <= strsX - 1 && hasPort == false)
            {
                hasPort = true;
                Obj.Y = 1.5f;
            }

            if (hasPort == true && Obj.Y < strsY)
            {
                GoU = false;
                GoD = false;
                hasPort = false;
                Obj.Y = strsY;
            }
        }

        if (GoU == true)
        {
            Obj.Y += 5 * dt;
            if (Obj.Y >= strsY + 1 && hasPort == false)
            {
                hasPort = true;
                Obj.Y = -1.5f;
            }

            if (hasPort == true && Obj.Y > strsY)
            {
                GoD = false;
                GoU = false;
                hasPort = false;
                Obj.Y = strsY;
            }
        }

        if (slot == false)
        {
            prebutt = new GameObject { X = Obj.X - 0.3f, Y = Obj.Y, Width = 0.8f, Height = 0.5f, Texture = bef, textname = "Arrow_L.png", ObjectName = "ler" };
            nexbutt = new GameObject { X = Obj.X + 0.3f, Y = Obj.Y, Width = 0.8f, Height = 0.5f, Texture = aft, textname = "Arrow_R.png", ObjectName = "rel" };
            nexw = new GameObject { X = Obj.X, Y = Obj.Y + 0.3f, Width = 0.8f, Height = 0.5f, Texture = nex, textname = "Arrow_U.png", ObjectName = "ler" };
            prew = new GameObject { X = Obj.X, Y = Obj.Y - 0.3f, Width = 0.8f, Height = 0.5f, Texture = prev, textname = "Arrow_D.png", ObjectName = "rel" };
            prebutt.Comp.Add(new GameButton(prebutt.Width, prebutt.Height, Obj.game, prebutt));
            nexbutt.Comp.Add(new GameButton(nexbutt.Width, nexbutt.Height, Obj.game, nexbutt));
            nexw.Comp.Add(new GameButton(nexw.Width, nexw.Height, Obj.game, nexw));
            prew.Comp.Add(new GameButton(prew.Width, prew.Height, Obj.game, prew));
            Obj.game.IOUjects.Add(prebutt);
            Obj.game.IOUjects.Add(nexbutt);
            Obj.game.IOUjects.Add(nexw);
            Obj.game.IOUjects.Add(prew);

            slot = true;
        }

            clickwait++;

        if (button.hovering == false)
        {
            Obj.Texture = closed;
        }
        else
        {
            Obj.Texture = open;
        }

        if (levelindex <= 0)
        {
            prebutt.Render = false;
            prebutt.GetComponent<GameButton>().canclick = false;
        }
        else
        {
            prebutt.Render = true;
            prebutt.GetComponent<GameButton>().canclick = true;
        }

        if ((levelindex >= (maxlvl - 1) && worldindex == maxworld) || levelindex >= (10-1))
        {
                nexbutt.Render = false;
                nexbutt.GetComponent<GameButton>().canclick = false;
        }
        else
        { 
            
                nexbutt.Render = true;
                nexbutt.GetComponent<GameButton>().canclick = true;
     
        }

        if (worldindex <= 1)
        {
            prew.Render = false;
            prew.GetComponent<GameButton>().canclick = false;
        }
        else
        {
            prew.Render = true;
            prew.GetComponent<GameButton>().canclick = true;
        }

        if (worldindex >= maxworld)
        {
            nexw.Render = false;
            nexw.GetComponent<GameButton>().canclick = false;
        }
        else
        {

            nexw.Render = true;
            nexw.GetComponent<GameButton>().canclick = true;

        }

        if (bg != null)
        {
            bg.Texture = bgt[worldindex - 1];
        }

        if (button.clicking == true && nexw.GetComponent<GameButton>().hovering == false)
        {
            Obj.game.newdirect = "Scenes/Levels";
            
            Obj.game.newindex = worldindex + "Lvl" + (levelindex+1).ToString();
        }

        

        if (prebutt.GetComponent<GameButton>().clicking == true && clickwait > 6)
        {
            GoR = false;
            GoL = false;
            Obj.X = strsX;
            GoR = true;
            levelindex -= 1;
            clickwait = 0;
            Obj.game.Audio.PlaySound(Bop);
            GameObject troy = Obj.game.objects.LastOrDefault(x => x.ObjectName == "Yamss");
            Obj.game.rejjects.Add(troy);
        }

        if (nexbutt.GetComponent<GameButton>().clicking == true && clickwait > 6)
        {
            GoR = false;
            GoL = false;
            Obj.X = strsX;
            GoL = true;
            levelindex += 1;
            clickwait = 0;
            Obj.game.Audio.PlaySound(Bop);
            GameObject newyam = new GameObject { X = -0.3f + (0.05f * levelindex), Y = 0.5f, Width = 0.8f, Height = 0.8f, Texture = yams, textname = "Yams!.png", ObjectName = "Yamss" };
            Obj.game.IOUjects.Add(newyam);
        }


        if (nexw.GetComponent<GameButton>().clicking == true && clickwait > 6)
        {
            GoD = false;
            GoU = false;
            Obj.Y = strsY;
            GoD = true;
            levelindex = 0;
            worldindex += 1;
            clickwait = 0;
            Obj.game.Audio.PlaySound(Bop);
            List<GameObject> yams = Obj.game.objects.FindAll(x => x.ObjectName == "Yamss");
            for (int i=0;i<yams.Count();i++)
            {
                Obj.game.rejjects.Add(yams[i]);
            }
            GameObject newegg = new GameObject { X = -0.6f, Y = -0.3f + (0.05f * worldindex), Width = 0.8f, Height = 0.8f, Texture = egg, textname = "Yams!.png", ObjectName = "Eff" };
            Obj.game.IOUjects.Add(newegg);
            
        }

        if (prew.GetComponent<GameButton>().clicking == true && clickwait > 6)
        {
            GoD = false;
            GoU = false;
            Obj.Y = strsY;
            GoU = true;
            levelindex = 0;
            worldindex -= 1;
            clickwait = 0;
            Obj.game.Audio.PlaySound(Bop);
            GameObject troy = Obj.game.objects.LastOrDefault(x => x.ObjectName == "Eff");
            Obj.game.rejjects.Add(troy);

        }

    }
}
