using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static OpenTK.Graphics.OpenGL.GL;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

class Editor : GameWindow
{
    List<GameObject> obj = new();
    public static List<ObjectManager> OM = new();
    public ObjectManager objj = new();
    public List<ObjectManager> handle = new();
    public List<float> Xmanager = new();
    public List<float> Ymanager = new();
    public int Whitexture;
    Renderer render;
    int Obchose;
    public bool Grid = true;
    public float MouseX;
    public float MouseY;
    public string path;
    public string[] lines;
    public List<Button> Buttons = new();
    public int objectpage = 0;
    KeyboardState Prevkey;
    KeyboardState Nowkey;
    bool DownDropped;
    public Button Larrow;
    public Button Rarrow;
    public Vector2 CameraPos;
    public Vector2 CameraPivot;
    public float FarLeft = -1;
    public float LowBottom = -1;
    public float firstXdif = 1;
    public float otherXdif = 0.025f;
    public float Ydif = 0.3f;
    public string direct;
    public string Index;
    public List<String> Dialog = new();
    public List<String> Speaker = new();
    public List<bool> Event = new();
    public List<string> Eventtitles = new();
    public bool triggercheck;
    public int Trigindx;
    public int nowtrig = 0;
    public bool stopnow;
    public int currnt = 1;
    public bool newcheck;
    public float MaXX;
    public float MaYY;
    public static bool edib = false;
    public bool infjump = false;
    static bool hasnload = true;
    private readonly Dictionary<string, int> textureCache = new();


    public Game game;
    public Editor()
        : base(GameWindowSettings.Default, new NativeWindowSettings()
          {
            Size = new Vector2i(500, 500),
            Title = "Editor"
          })
        { }

    public int LoadTexture(string path)
    {
        
        Console.WriteLine("poop");
        // Return the already-loaded texture.
        if (textureCache.TryGetValue(path, out int existingTexture))
        {
            return existingTexture;
        }

        // If the exact path doesn't exist, search all subfolders.
        if (!File.Exists(path))
        {
            Console.WriteLine("poop2");
            string fileName = Path.GetFileName(path);

            string? foundPath = Directory
                .GetFiles(AppContext.BaseDirectory, fileName, SearchOption.AllDirectories)
                .FirstOrDefault();

            if (foundPath == null)
            {
                Console.WriteLine("poop3");
                throw new FileNotFoundException(
                    $"Could not find texture: {path}"
                );
            }
            Console.WriteLine("poop4");
            path = foundPath;
        }

        StbImageSharp.StbImage.stbi_set_flip_vertically_on_load(1);

        using FileStream stream = File.OpenRead(path);

        StbImageSharp.ImageResult image =
            StbImageSharp.ImageResult.FromStream(
                stream,
                StbImageSharp.ColorComponents.RedGreenBlueAlpha
            );

        int texture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            image.Width,
            image.Height,
            0,
            GLPixelFormat.Rgba,
            PixelType.UnsignedByte,
            image.Data
        );

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Nearest
        );

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Nearest
        );

        GL.BindTexture(TextureTarget.Texture2D, 0);

        textureCache[path] = texture;

        Console.WriteLine($"Loaded new texture: {path}");

        return texture;
    }

    void MakeGrid()
    {
        
        

        float startX = -1;
        float startY = -1;
        for (int i=0; i<90; i++)
        {
            Xmanager.Add(startX);
            Console.WriteLine("X Grid: " + startX);
            startX += 0.065f;
        }

        for (int y = 0; y < 90; y++)
        {
            Ymanager.Add(startY);
            Console.WriteLine("Y Grid: " + startY);
            startY += 0.065f;
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    ObjectManager CreateObject(ObjectManager prefab)
    {
        var obj = new ObjectManager
        {
            son = new GameObject
            {
                X = prefab.son.X,
                Y = prefab.son.Y,
                Width = prefab.son.Width,
                Height = prefab.son.Height,
                Texture = prefab.son.Texture,
                textname = prefab.son.textname,
                ObjectName = prefab.son.ObjectName
            },
            Cmp = new List<Component>()
        };

        foreach (var c in prefab.Cmp)
        {
            obj.Cmp.Add(c.Clone(obj.son));
        }

        if (prefab.GetComponent<BoxCollider>() != null)
        {
            
            obj.GetComponent<BoxCollider>().Collider = prefab.GetComponent<BoxCollider>().Collider;
            Console.WriteLine("LOLOMPYSHIT: "  + obj.GetComponent<BoxCollider>().Collider);
        }

        return obj;
    }


    protected override void OnLoad()
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);
        render = new Renderer();
        MakeGrid();
        Whitexture = LoadTexture("Whitt.jpg");
        Rarrow = new Button(LoadTexture("Arrow_R.png"), "Arrow_R.png", 0.75499994f, 0.68999994f, 0.8f, 0.5f, this);
        Larrow = new Button(LoadTexture("Arrow_L.png"), "Arrow_L.png", -0.675f, 0.68999994f, 0.8f, 0.5f, this);
        Rarrow.right = true;
        

        {
            //Player_GameObject
            OM.Add(new ObjectManager{ son = new GameObject { X = 0, Y = 0f, Width = 0.3f, Height = 0.8f, Texture = LoadTexture("Beanzie.png"), textname = "Beanzie.png", ObjectName = "Player" } });
            OM[0].Cmp.Add(new Rigidbody(OM[0].son, 1, OM[0].son.X, OM[0].son.Y) { Gravity = 1.9f });
            OM[0].Cmp.Add(new Player(0.20f, OM[0].son) { game = game });
            OM[0].Cmp.Add(new BoxCollider(0.15f, 0.80f, OM[0].son, game.Dect, "One", true, true) { Collider = true, });
            OM[0].Cmp.Add(new Animation(1, 0.12f, 0, 0, OM[0].son.Width, OM[0].son.Height) { column = 1, row = 1, fixwidth = OM[0].son.Width, fixheight = OM[0].son.Height });
            OM[0].Cmp.Add(new Record(OM[0].son));
            //Platform_GameObject
            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.35f, Height = 0.35f, Texture = LoadTexture("Bad_Floor.png"), textname = "Bad_Floor.png", ObjectName = "Platform" } });
            OM[1].Cmp.Add(new Rigidbody(OM[1].son, 1, OM[1].son.X, OM[1].son.Y));
            OM[1].Cmp.Add(new BoxCollider(OM[1].son.Width, OM[1].son.Height, OM[1].son, game.Dect, "Two", false, false) { Collider = true});

            //Fake-Platform_GameObject
            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.35f, Height = 0.35f, Texture = LoadTexture("foiled.png"), textname = "Bad_Floor.png", ObjectName = "Fake_Platform" } });

            //Start game button
            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 1.5f, Height = 0.9f, Texture = LoadTexture("start_butt.png"), textname = "start_butt.png", ObjectName = "starty" } });
            OM[3].Cmp.Add(new GameButton(OM[3].son.Width, OM[3].son.Height, game, OM[3].son));
            OM[3].Cmp.Add(new MenuButton(OM[3].son));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 1.5f, Height = 2f, Texture = LoadTexture("LevelDoor.png"), textname = "LevelDoor.png", ObjectName = "doory" } });
            OM[4].Cmp.Add(new GameButton(OM[4].son.Width, OM[4].son.Height, game, OM[4].son));
            OM[4].Cmp.Add(new LevelSelector(OM[4].son));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 13, Height = 12, Texture = LoadTexture("Tristomp-BG.png"), textname = "Tristomp-BG.png", ObjectName = "BGm" } });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.3f, Height = 1.2f, Texture = LoadTexture("TextBoxthing.png"), textname = "TextBoxthing.png", ObjectName = "Triggerthing" } });
            OM[6].Cmp.Add(new BoxCollider(OM[6].son.Width, OM[6].son.Height, OM[6].son, game.Dect, "Trigger", false, false) );

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 8, Height = 6, Texture = LoadTexture("Trimount.png"), textname = "Trimount.png", ObjectName = "BGmount" } });
            OM[7].son.game = game;
            OM[7].Cmp.Add(new BGpar(OM[7].son, 0.15f));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 4, Height = 7, Texture = LoadTexture("Triflag.png"), textname = "Triflag.png", ObjectName = "BGflag" } });
            OM[8].son.game = game;
            OM[8].Cmp.Add(new BGpar(OM[8].son, 0.10f));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 16, Height = 7, Texture = LoadTexture("Tristomp_Grass.png"), textname = "Tristomp_Grass.png", ObjectName = "BGgrass" } });
            OM[9].son.game = game;
            OM[9].Cmp.Add(new BGpar(OM[9].son, 0.20f));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.8f, Height = 1.2f, Texture = LoadTexture("Tri_Door.png"), textname = "Tri_Door.png", ObjectName = "Doorthing" } });
            OM[10].Cmp.Add(new BoxCollider(0.45f, 0.95f, OM[10].son, game.Dect, "Door", false, false));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.28f, Height = 0.33f, Texture = LoadTexture("Tribox.png"), textname = "Tribox.png", ObjectName = "Triboxx" } });
            OM[11].Cmp.Add(new Rigidbody(OM[11].son, 1, OM[11].son.X, OM[11].son.Y));
            OM[11].Cmp.Add(new BoxCollider(OM[11].son.Width, OM[11].son.Height, OM[11].son, game.Dect, "Tristart", false, false) { Collider = true });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.28f, Height = 0.33f, Texture = LoadTexture("Plock.png"), textname = "Plock.png", ObjectName = "purplocky" } });
            OM[12].Cmp.Add(new Rigidbody(OM[12].son, 1, OM[12].son.X, OM[12].son.Y));
            OM[12].Cmp.Add(new BoxCollider(OM[12].son.Width, OM[12].son.Height, OM[12].son, game.Dect, "PushP", false, false) { Collider = true });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.3f, Height = 0.35f, Texture = LoadTexture("Trispike.png"), textname = "Trispike.png", ObjectName = "spike" } });
            OM[13].Cmp.Add(new Rigidbody(OM[13].son, 1, OM[13].son.X, OM[13].son.Y));
            OM[13].Cmp.Add(new BoxCollider(0.23f, 0.29f, OM[13].son, game.Dect, "Hurt", false, false) { Collider = true });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.28f, Height = 0.33f, Texture = LoadTexture("Trirestart.png"), textname = "Trirestart.png", ObjectName = "Resty" } });
            OM[14].Cmp.Add(new Rigidbody(OM[14].son, 1, OM[14].son.X, OM[14].son.Y));
            OM[14].Cmp.Add(new BoxCollider(OM[14].son.Width, OM[14].son.Height, OM[14].son, game.Dect, "Restart", false, false) { Collider = true });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.3f, Height = 0.8f, Texture = LoadTexture("TriKey.png"), textname = "TriKey.png", ObjectName = "key" } });
            OM[15].Cmp.Add(new BoxCollider(OM[15].son.Width, OM[15].son.Height, OM[15].son, game.Dect, "Key", false, false));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.38f, Height = 0.5f, Texture = LoadTexture("Ememy_sheet.png"), textname = "Ememy_sheet.png", ObjectName = "ememy" } });
            OM[16].Cmp.Add(new Rigidbody(OM[16].son, 1, OM[16].son.X, OM[16].son.Y) );
            OM[16].Cmp.Add(new BoxCollider(0.33f, 0.43f, OM[16].son, game.Dect, "Hurt", true, false) { onlylist = ["Turn"] });
            OM[16].Cmp.Add(new Enemy(OM[16].son, 0.35f));
            OM[16].Cmp.Add(new Animation(1, 0.12f, 0, 0, OM[16].son.Width, OM[16].son.Height) { column = 2, row = 1 });

            OM.Add(new ObjectManager { son = new GameObject { X = 0f, Y = 0f, Width = 0.15f, Height = 1f, Texture = LoadTexture("TextBoxthing.png"), textname = "TextBoxthing.png", ObjectName = "turnthing" } });
            

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = -0.35f, Height = 0.9f, Texture = LoadTexture("Deman.png"), textname = "Deman.png", ObjectName = "Theman" } });
            OM[18].Cmp.Add(new Deman(OM[18].son));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.55f, Height = 0.65f, Texture = LoadTexture("Upcload.png"), textname = "Upcload.png", ObjectName = "cloudthing" } });
            OM[19].Cmp.Add(new BoxCollider(0.55f, 0.65f, OM[19].son, game.Dect, "Cloud", false, false));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.38f, Height = 0.5f, Texture = LoadTexture("Purp_emy.png"), textname = "Purp_emy.png", ObjectName = "purpememy" } });
            OM[20].Cmp.Add(new Rigidbody(OM[20].son, 1, OM[20].son.X, OM[20].son.Y));
            OM[20].Cmp.Add(new BoxCollider(OM[20].son.Width, OM[20].son.Height, OM[20].son, game.Dect, "PushP", true, false) { Collider = true, onlylist  = ["Turn"] } );
            OM[20].Cmp.Add(new Enemy(OM[20].son, 0.25f));
            OM[20].Cmp.Add(new Animation(1, 0.12f, 0, 0, OM[20].son.Width, OM[20].son.Height) { column = 2, row = 1 });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.76f, Height = 0.6f, Texture = LoadTexture("EmyDead.png"), textname = "EmyDead.png", ObjectName = "Deadman" } });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.35f, Height = 0.9f, Texture = LoadTexture("Jeman.png"), textname = "Jeman.png", ObjectName = "Jeman" } });
            OM[22].Cmp.Add(new Deman(OM[22].son) { Jeman = true});

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 0.3f, Height = 0.8f, Texture = LoadTexture("Pastel_standy.png"), textname = "Pastel_standy.png", ObjectName = "Plastyer" } });

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 3f, Height = 6f, Texture = LoadTexture("Pause_main.png"), textname = "Pause_main.png", ObjectName = "pause" } });
            OM[24].Cmp.Add(new Pause(OM[24].son));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 5, Height = 3.5f, Texture = LoadTexture("Tri-Logo.png"), textname = "Tri-Logo.png", ObjectName = "Log" } });
            OM[25].Cmp.Add(new Deman(OM[25].son));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 6, Height = 5, Texture = LoadTexture("Intro_0.png"), textname = "Intro_0.png", ObjectName = "intr" } });
            OM[26].son.game = game;
            OM[26].Cmp.Add(new Sequence(12,"Intro", OM[26].son));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 8, Height = 7f, Texture = LoadTexture("Tri-hill.png"), textname = "Tri-hill.png", ObjectName = "trih" } });
            OM[27].son.game = game;
            OM[27].Cmp.Add(new BGpar(OM[27].son, 0.14f));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 16, Height = 20, Texture = LoadTexture("pale.png"), textname = "pale.png", ObjectName = "pll" } });
            OM[28].son.game = game;
            OM[28].Cmp.Add(new BGpar(OM[28].son, 0.20f));

            OM.Add(new ObjectManager { son = new GameObject { X = 0, Y = 0f, Width = 12, Height = 7f, Texture = LoadTexture("glaf.png"), textname = "glaf.png", ObjectName = "gll" } });
            OM[29].son.game = game;
            OM[29].Cmp.Add(new BGpar(OM[29].son, 0.24f));

        }


        int index = 0;
        for (int i = (objectpage * 5); i < (objectpage * 5) + 5; i++)
        {
            if (index == 5)
            {
                break;
            }
            if (i < (OM.Count()))
            {
                Console.WriteLine(index);
                
                Buttons.Add(new Button(OM[i].son.Texture, OM[i].son.textname, -0.425f + (0.25f * index), 0.68999994f, 0.8f, 0.5f, this) { ButtonName = OM[i].son.ObjectName });
            }
            else
            {
                Buttons.Add(new Button(LoadTexture("Null.png"), "Null.png", -0.425f + (0.25f * index), 0.68999994f, 0.8f, 0.5f, this) { ButtonName = "Nullywully" });
            }
            index += 1;
        }


    }

    public void UpdateRow()
    {
        int index = 0;
        Buttons.Clear();
        for (int i = (objectpage * 5); i < (objectpage * 5) + 5; i++)
        {
            if (index == 5)
            {
                break;
            }
            if (i < (OM.Count()))
            {
                Console.WriteLine(index);
                
                Buttons.Add(new Button(OM[i].son.Texture, OM[i].son.textname, -0.425f + (0.25f * index), 0.68999994f, 0.8f, 0.5f, this) { ButtonName = OM[i].son.ObjectName});
                
            }
            else
            {
                Buttons.Add(new Button(LoadTexture("Null.png"), "Null.png", -0.425f + (0.25f * index), 0.68999994f, 0.8f, 0.5f, this) { ButtonName = "Nullywully" });
            }
            index += 1;
        }
        Console.WriteLine("Button Homany = " + Buttons.Count());
    }

    public void LoadNewScene(string Direct, string index)
    {
        infjump = false;
        direct = Direct;
        Index = index;
        triggercheck = false;
        nowtrig = 0;
        Trigindx = 0;
        Event.Clear();
        Eventtitles.Clear();
        Dialog.Clear();
        Speaker.Clear();
        string folder = Direct;
        Directory.CreateDirectory(folder);
        string[] files = Directory.GetFiles(folder, "*.txt");
        if (files.Length > 0)
        {
            string filename = files.FirstOrDefault(file=>Path.GetFileNameWithoutExtension(file)==index);
            lines = File.ReadAllLines(filename);
        }
        if (index != "Intro")
        {
            ObjectManager bg = OM[5];
            bg.son.X = 0;
            bg.son.Y = 0;
            handle.Add(bg);
        }
        
        if (index.Contains("Lvl"))
        {
           

            ObjectManager ps = CreateObject(OM[24]);
            ps.son.X = 2;
            ps.son.Y = 0;
            handle.Add(ps);

            if (index != "LvlSlctr")
            {

                

                ObjectManager pl = CreateObject(OM[28]);

                pl.son.X = 0;
                pl.son.Y = 0.1f;
                handle.Add(pl);

                

                


                ObjectManager br = CreateObject(OM[9]);

                br.son.X = 0;
                br.son.Y = -0.5f;
                handle.Add(br);

                



            }
            
        }
        
        foreach (string line in lines)
        {
            
            string[] parts = line.Split('/');
            if (parts[0] == "Level")
            {
                int OMint = int.Parse(parts[1]);
                float OMX = float.Parse(parts[2]);
                float OMY = float.Parse(parts[3]);

                ObjectManager newObj = CreateObject(OM[OMint]);


                newObj.son.X = OMX;
                newObj.son.Y = OMY;

                newObj.X = OMX;
                newObj.Y = OMY;


                


                handle.Add(newObj);
            }
            else if (parts[0] == "MaxX")
            {
                MaXX = float.Parse(parts[1]);
            }
            else if (parts[0] == "MaxY")
            {
                MaYY = float.Parse(parts[1]);
            }
            else if (parts[0] == "Time")
            {
                if (parts[1] == "inf")
                {
                    game.inftime = true;
                    game.totaltime = 100;
                }
                else
                {
                    game.inftime = false;
                    game.totaltime = int.Parse(parts[1]);
                }


            }
            else if (parts[0] == "addpastel")
            {
                game.pasteladd = true;
            }
            else if (parts[0] == "addsark")
            {
                game.sarkadd = true;
            }
            else if (parts[0] == "jumpinf")
            {
                infjump = true;
            }
        }
        

    }

/*
    public void newTrigger(string Direct, string index, int total)
    {
        
        direct = Direct;
        Index = index;
        int nowtrig = 0;
        triggercheck = false;
        newcheck = false;
        Event.Clear();
        Eventtitles.Clear();
        Dialog.Clear();
        Speaker.Clear();
        string folder = Direct;
        Directory.CreateDirectory(folder);
        string[] files = Directory.GetFiles(folder, "*.txt");
        if (files.Length > 0)
        {
            string filename = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == index);
            lines = File.ReadAllLines(filename);
        }
        foreach (string line in lines)
        {

            string[] parts = line.Split('/');

            if (parts[0] == "Text" && triggercheck == true)
            {

                Speaker.Add(parts[1]);
                Dialog.Add(parts[2]);
                Event.Add(false);

            }
            else if (parts[0] == "Event" && triggercheck == true)
            {

                Event[Event.Count - 1] = true;
                Eventtitles.Add(parts[1]);
            }
            else if (parts[0] == "Trigger")
            {
                nowtrig += 1;
                if (nowtrig == currnt)
                { 
                    triggercheck = true;  
                }
                

                Console.WriteLine(nowtrig + " " + Trigindx);
            }
            else if (parts[0] == "EndTrigger")
            {
                if (nowtrig == currnt)
                {
                    triggercheck = false;
                    newcheck = true;
                    currnt += 1;
                    break;
                }
                

            }
            


        }
        stopnow = false;
        
    }
    */

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button == MouseButton.Left && DownDropped == false)
        {
            float x = 0;
            float y = 0;
            if (Grid == false)
            {
                x = (MousePosition.X / (float)Size.X) * 2f - 1f;
                y = 0.9f - (MousePosition.Y / (float)Size.Y) * 2f;
                x += CameraPos.X;
                y += CameraPos.Y;
            }
            else
            {

                bool FoundX = false;
                bool FoundY = false;
                for (int i = 1; i < Xmanager.Count; i++)
                {
                    if (FoundX == false)
                    {
                        if (((MousePosition.X / (float)Size.X) * 2f - 1f) + CameraPos.X < Xmanager[i])
                        {
                            FoundX = true;
                            x = Xmanager[i] - (0.065f / 2);
                        }

                    }

                }

                for (int p = 1; p < Ymanager.Count; p++)
                {
                    if (FoundY == false)
                    {
                        if ((0.9f - (MousePosition.Y / (float)Size.Y) * 2f) + CameraPos.Y < Ymanager[p])
                        {
                            FoundY = true;
                            y = Ymanager[p] - (0.065f / 2);
                        }

                    }

                }
            }

            ObjectManager newObj = CreateObject(OM[Obchose]);


            newObj.son.X = x;
            newObj.son.Y = y;

            newObj.X = x;
            newObj.Y = y;





            handle.Add(newObj);
            Console.WriteLine(handle[0].son.X);





        }
        else if (e.Button == MouseButton.Right && DownDropped == false && Grid == true)
        {
            float XX = 0;
            float YY = 0;
            

                bool FoundX = false;
                bool FoundY = false;
                for (int i = 1; i < Xmanager.Count; i++)
                {
                    if (FoundX == false)
                    {
                        if (((MousePosition.X / (float)Size.X) * 2f - 1f) + CameraPos.X < Xmanager[i])
                        {
                            FoundX = true;
                            XX = Xmanager[i] - (0.065f / 2);
                        }

                    }

                }

                for (int p = 1; p < Ymanager.Count; p++)
                {
                    if (FoundY == false)
                    {
                        if ((0.9f - (MousePosition.Y / (float)Size.Y) * 2f) + CameraPos.Y < Ymanager[p])
                        {
                            FoundY = true;
                            YY = Ymanager[p] - (0.065f / 2);
                        }

                    }

                }

            int index = handle.FindIndex(x => x.X == XX && x.Y == YY);
            ObjectManager dum = handle.FirstOrDefault(x => x.X == XX && x.Y == YY);
            
            if (dum != null)
            {
                handle.Remove(handle[index]);
            }
            

        }
        else if (e.Button == MouseButton.Right && DownDropped == false && Grid == false)
        {
            float XX = 0;
            float YY = 0;


            bool FoundX = false;
            bool FoundY = false;

            XX = (MousePosition.X / (float)Size.X) * 2f - 1f;
            YY = 0.9f - (MousePosition.Y / (float)Size.Y) * 2f;
            XX += CameraPos.X;
            YY += CameraPos.Y;


            int index = handle.FindIndex(x => (x.X > (XX-0.025f) && x.X < (XX + 0.025f)) && (x.Y > (YY - 0.025f) && x.Y < (YY + 0.025f)));
            ObjectManager dum = handle.FirstOrDefault(x => (x.X > (XX - 0.025f) && x.X < (XX + 0.025f)) && (x.Y > (YY - 0.025f) && x.Y < (YY + 0.025f)));

            if (dum != null)
            {
                handle.Remove(handle[index]);
            }


        }
        int butindx = 0;
        foreach (var butt in Buttons)
        {
            butt.OnMouseDown();
            if (butt.clicking == true)
            {
                Obchose = (objectpage * 5) + butindx;
                break;
            }
            butindx += 1;
        }
        
        Larrow.OnMouseDown();
        Rarrow.OnMouseDown();
        if (Larrow.clicking == true)
        {
            Console.WriteLine("Left Click");
            objectpage -= 1;
            UpdateRow();
        }

        if (Rarrow.clicking == true)
        {
            Console.WriteLine("Right Click");
            objectpage += 1;
            UpdateRow();
        }

        foreach (var butt in Buttons)
        {
            butt.clicking = false;

        }

        Larrow.clicking = false;
        Rarrow.clicking = false;

        Console.WriteLine(objectpage);
    }

    protected override void OnUpdateFrame(FrameEventArgs arg)
    {
        var input = KeyboardState;
        Prevkey = Nowkey;
        Nowkey = KeyboardState.GetSnapshot();
        
        MouseX = MousePosition.X;
        MouseY = MousePosition.Y;

        

        if (Nowkey.IsKeyDown(Keys.P))
        {
            hasnload = false;
            Event.Clear();
            handle.Clear();
            Dialog.Clear();
            Speaker.Clear();
            LoadNewScene("Scenes", "Menu");
            game.Run();
            Console.WriteLine(handle.Count);
        }

        if (Nowkey.IsKeyDown(Keys.Z) && !Prevkey.IsKeyDown(Keys.Z))
        {
            handle.Remove(handle[handle.Count - 1]);
        }

        if (Nowkey.IsKeyDown(Keys.E) && !Prevkey.IsKeyDown(Keys.E))
        {
            DownDropped = !DownDropped;
            objectpage = 0;
            UpdateRow();
        }

        if (Nowkey.IsKeyDown(Keys.R) && !Prevkey.IsKeyDown(Keys.R))
        {
            Grid = !Grid;
        }



        if (input.IsKeyPressed(Keys.I))
        {
            Directory.CreateDirectory("Scenes/Levels");
            path = Path.Combine("Scenes/Levels/SideLevels", "1Lvl7.1.2.txt");
            File.WriteAllText(path, "");
            
            foreach (var hin in handle)
            {
                foreach (var opp in OM)
                {
                    if (hin.son.ObjectName == opp.son.ObjectName)
                    {
                        int indx = OM.FindIndex(opc => opc.son.ObjectName == hin.son.ObjectName);
                        Console.WriteLine(hin.son.ObjectName);
                        File.AppendAllText(path, "Level/" + indx.ToString() + "/" + hin.son.X + "/" + hin.son.Y + "\n");
                    }
                }
            }


            
            File.AppendAllText(path, "MaxX/0\n");
            File.AppendAllText(path, "MaxY/0\n");
            File.AppendAllText(path, "Time/inf\n");

        }

        if (input.IsKeyPressed(Keys.Y))
        {
            Event.Clear();
            handle.Clear();
            Dialog.Clear();
            Speaker.Clear();
            direct = "Scenes/Levels/SideLevels";
            Index = "1Lvl7.1.2";
            triggercheck = false;
            nowtrig = 0;
            Trigindx = 0;
            Event.Clear();
            Eventtitles.Clear();
            Dialog.Clear();
            Speaker.Clear();
            string folder = direct;
            Directory.CreateDirectory(folder);
            string[] files = Directory.GetFiles(folder, "*.txt");
            if (files.Length > 0)
            {
                string filename = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == Index);
                lines = File.ReadAllLines(filename);
            }
            
            foreach (string line in lines)
            {

                string[] parts = line.Split('/');
                if (parts[0] == "Level")
                {
                    int OMint = int.Parse(parts[1]);
                    float OMX = float.Parse(parts[2]);
                    float OMY = float.Parse(parts[3]);

                    ObjectManager newObj = CreateObject(OM[OMint]);


                    newObj.son.X = OMX;
                    newObj.son.Y = OMY;

                    newObj.X = OMX;
                    newObj.Y = OMY;





                    handle.Add(newObj);
                }
                else if (parts[0] == "MaxX")
                {
                    MaXX = float.Parse(parts[1]);
                }
                else if (parts[0] == "MaxY")
                {
                    MaYY = float.Parse(parts[1]);
                }
                else if (parts[0] == "Time")
                {
                    if (parts[1] == "inf")
                    {
                        game.inftime = true;
                    }
                    else
                    {
                        game.totaltime = int.Parse(parts[1]);
                    }


                }
                else if (parts[0] == "addpastel")
                {
                    game.pasteladd = true;
                }
                else if (parts[0] == "addsark")
                {
                    game.sarkadd = true;
                }
                else if (parts[0] == "jumpinf")
                {
                    infjump = true;
                }

            }
        }
        
        foreach (var but in Buttons)
        {          
            but.Update(input);
        }
        Larrow.Update(input);
        Rarrow.Update(input);
        float dt = (float)arg.Time;
        if (input.IsKeyDown(Keys.Right))
        {
            CameraPos.X += 1.5f * dt;
        }

        if (input.IsKeyDown(Keys.Left))
        {
            CameraPos.X -= 1.5f * dt;
        }

        if (input.IsKeyDown(Keys.Up))
        {
            CameraPos.Y += 1.5f * dt;
        }

        if (input.IsKeyDown(Keys.Down))
        {
            CameraPos.Y -= 1.5f * dt;
        }


    }

    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        foreach (var hnd in handle)
        {
            
            render.DrawH(hnd, this); //'Draws' the GameObject
        }
        
        
        if (Grid == true)
        {
            foreach (var Xnt in Xmanager)
            {

                render.DrawGRX(Xnt, Whitexture, this); //'Draws' the GameObject
            }

            foreach (var Ynt in Ymanager)
            {

                render.DrawGRY(Ynt, Whitexture, this); //'Draws' the GameObject
            }
        }
        
        if (DownDropped == true)
        {
            render.DrawB(Rarrow);
            if (objectpage > 0)
            {
                render.DrawB(Larrow);
                Larrow.canclick = true;
            }
            else
            {
                Larrow.canclick = false;
            }
            foreach (var but in Buttons)
            {
                but.canclick = true;
                render.DrawB(but); //'Draws' the GameObject
            }
        }
        else
        {
            foreach (var but in Buttons)
            {

                but.canclick = false;
            }
            Larrow.canclick = false;
        }


            GL.ClearColor(0, 0, 0, 1);
        SwapBuffers();
    }

}




