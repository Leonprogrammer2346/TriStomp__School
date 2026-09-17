using NVorbis;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using StbVorbisSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DrawingPixelFormat = System.Drawing.Imaging.PixelFormat;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;



class Game : GameWindow
{
    public List<GameObject> objects = new();
    List<GameObject> set = new();
    Renderer render;
    public Detection Dect = new Detection();
    public Editor editor;
    public int Whitexture;
    public Vector2 CameraPos;
    public GameObject Playerpos;
    public float RightX;
    public float LeftX;
    public float UpY;
    public float DownY;
    public float CamDestinationX;
    public float CamDestinationY;
    public ALContext AudioContext;
    public int buffer;
    public int source;
    public AudioManager Audio = new AudioManager();
    public float MouseX;
    public float MouseY;
    List<GameButton> buttonz = new();
    public string direct;
    public string Index;
    public string predirect;
    public string preindex;
    public string newdirect;
    public string newindex;
    public List<GameObject> IOUjects = new();
    public Bitmap bmp = new Bitmap(700, 256);
    public int texTexture;
    public GameObject textureobject = new GameObject();
    public List<String> AllText = new();
    public List<String> Talker = new();
    public bool textover;
    public int textndex = 0;
    float timer;
    public bool textrender;
    GameObject textbox = new GameObject();
    GameObject textphoto = new GameObject();
    public string currentevent;
    public List<bool> Eventt = new();
    public List<string> eventtitles = new();
    public int eventindex;
    public bool Trig;
    public int trigint;
    public GameObject mainBG;
    public Vector2 PlayerV;
    public float Faright;
    public float Farup;
    private readonly Dictionary<string, int> textureCache = new();
    public int texiitn;
    public static List<string> texturenames = new();
    public static List<int> Loads = new();
    GameObject lefcol;
    GameObject righcol;
    public FrameEventArgs Arg;
    public static int purpy;
    public bool Reload;
    public bool inftime;
    public int totaltime;
    public GameObject MainKey;
    public bool keyfound;
    public List<GameObject> rejjects = new();
    public GameObject Door;
    public float BaseX;
    public float BaseY;
    public bool shake = false;
    Random rand = new Random();
    public float PS;
    int Fwames;
    public List<float> turns = new();
    public List<float> turnY = new();
    public bool pasteladd;
    public bool sarkadd;
    public bool infjump;
    public static bool startlvl;
    public bool pause;
    public GameObject pause_thing;
    public bool player_exist;
    public static int next;
    public bool loadaudio = false;
    public static string LvlJB = "sigh";
    public string LvlJBS = "sigh";



    public Game()
        : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Tri-Stomp",
                APIVersion = new Version(3, 3)
            })
    { }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }
    public int LoadTexture(string path)
    {
        texiitn += 1;
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
    //first frame in the game

    int LoadAudio(string path)
    {
        int buffer = AL.GenBuffer();

        using var reader = new BinaryReader(File.OpenRead(path));

        

        return buffer;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        
        foreach (var butt in buttonz)
        {
            
            butt.OnMouseDown();
            
        }

        
    }

    public int CreateTextTexture(
    string text,
    string fontName,
    int fontSize,
    int maxWidth, 
    out int textureWidth,
    out int textureHeight
)
    {
        

        

        System.Drawing.Font font =
    new System.Drawing.Font(fontName, fontSize);

        SizeF measuredSize;

        using (Bitmap tempBitmap = new Bitmap(1, 1))
        using (Graphics tempGraphics = Graphics.FromImage(tempBitmap))
        {
            measuredSize = tempGraphics.MeasureString(
                text,
                font,
                maxWidth
            );
        }

        int padding = 20;

        int bitmapWidth = maxWidth;
        int bitmapHeight = (int)Math.Ceiling(measuredSize.Height) + padding;

        bmp = new Bitmap(bitmapWidth, bitmapHeight);
        textureWidth = bmp.Width;
        textureHeight = 496;
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.Transparent);

            // Keep this because your text renderer needs the vertical flip.
            g.TranslateTransform(0, bmp.Height);
            g.ScaleTransform(1, -1);

            g.DrawString(
                text,
                font,
                Brushes.White,
                new RectangleF(
                    0,
                    0,
                    bitmapWidth,
                    bitmapHeight
                )
            );
        }

        int texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture);

        BitmapData data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly,
            DrawingPixelFormat.Format32bppArgb
        );

        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            bmp.Width,
            bmp.Height,
            0,
            GLPixelFormat.Bgra,
            PixelType.UnsignedByte,
            data.Scan0
        );

        bmp.UnlockBits(data);

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear
        );

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear
        );

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS,
            (int)TextureWrapMode.ClampToEdge
        );

        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT,
            (int)TextureWrapMode.ClampToEdge
        );

        bmp.Dispose();

        return texture;
    }


    protected override void OnLoad()
    {
        Reload = false;
        Console.WriteLine("GAME HAS RAN");
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        eventindex = 0;
        if (purpy == 0)
        {
            purpy = LoadTexture("Pspark.png");
        }

      

        GL.Viewport(0, 0, Size.X, Size.Y);

        GL.Enable(EnableCap.Blend);

        GL.BlendFunc(
            BlendingFactor.SrcAlpha,
            BlendingFactor.OneMinusSrcAlpha
        );
        textover = false;
        

        render = new Renderer();
        Console.WriteLine("First frame!");
        GL.ClearColor(0f, 0f, 0f, 1f);

        direct = editor.direct;
        Index = editor.Index;

        newdirect = editor.direct;
        newindex = editor.Index;
        textrender = false;



        Audio.Init();
        AllText = editor.Dialog;
        Talker = editor.Speaker;
        Eventt = editor.Event;
        eventtitles = editor.Eventtitles;
        textover = false;
        foreach (var edi in editor.handle)
        {
            GameObject newObj = new GameObject
            {
                X = edi.son.X,
                Y = edi.son.Y,
                Width = edi.son.Width,
                Height = edi.son.Height,
                textname = edi.son.textname,
                ObjectName = edi.son.ObjectName,
                Comp = new List<Component>(),
                game = this


            };
           
            if (texturenames.Contains(edi.son.textname) == false)
            {
                newObj.Texture = LoadTexture(edi.son.textname);
                texturenames.Add(edi.son.textname);
                Loads.Add(newObj.Texture);
            }
            else
            {
                int inx = texturenames.FindIndex(x => x == edi.son.textname);
                newObj.Texture = Loads[inx];
            }

            GameObject samename = objects.FirstOrDefault(x => x.ObjectName == newObj.ObjectName);
            
            while (samename != null)
            {
                newObj.ObjectName += "_";
                samename = objects.FirstOrDefault(x => x.ObjectName == newObj.ObjectName);
            }

            if (newObj.ObjectName.Contains("turnthing") || newObj.ObjectName.Contains("Triggerthing"))
            {
                newObj.Render = false;
            }
            newObj.game = this;
            newObj.fixwidth = newObj.Width;
            newObj.fixheight = newObj.Height;

            if (newObj.ObjectName == "Player")
            {
                player_exist = true;
                PlayerV.X = newObj.X;
                PlayerV.Y = newObj.Y;
            }

            if (newObj.ObjectName == "pause")
            {
                pause_thing = newObj;
            }


            if (newObj.ObjectName == "Triboxx")
            {
                newObj.axisloop = true;
                newObj.Changewid = false;
                newObj.Changehgt = true;
                newObj.newheight = 0.45f;
                newObj.axispeed = 0.0001f;
                newObj.framelimit = 100;
            }

            if (newObj.ObjectName == "Doorthing" || newObj.ObjectName == "cloudthing")
            {
                Door = newObj;
            }

            if (newObj.ObjectName != null)
            {
                if (newObj.ObjectName.Contains("purp"))
                {
                    newObj.Purp = true;
                }
            }
            loadaudio = false;


            foreach (var c in edi.Cmp)
            {
                newObj.Comp.Add(c.Clone(newObj));
                //Console.WriteLine("Added " + c);           
            }

            GameButton butt = newObj.GetComponent<GameButton>();

            //Console.WriteLine("NewButton is " + butt);
            if (butt != null)
            {
                buttonz.Add(butt);
            }

            if (edi.GetComponent<BoxCollider>() != null)
            {

                newObj.GetComponent<BoxCollider>().Collider = edi.GetComponent<BoxCollider>().Collider;
                newObj.GetComponent<BoxCollider>().ongame = true;
                Console.WriteLine("LOMPYFROMP: " + newObj.GetComponent<BoxCollider>().Collider);
                Console.WriteLine("YOUR GAMEY IS: " + newObj.GetComponent<BoxCollider>().ongame);
            }

            objects.Add(newObj);
            //Console.WriteLine(newObj.textname);
        }

        Playerpos = objects.FirstOrDefault(opc => opc.ObjectName == "Player");
        mainBG = objects.FirstOrDefault(opc => opc.ObjectName == "BGm");
        RightX = 0;
        LeftX = -384378364385;
        CamDestinationX = 0;
        CamDestinationY = 0;
        textbox.UI = true;
        textbox.X = -0.3f;
        textbox.Y = 0.75f;
        textbox.Width = 4f;
        textbox.Height = 1.2f;
        if (texturenames.Contains("TextBoxthing.png") == false)
        {
            textbox.Texture = LoadTexture("TextBoxthing.png");
            texturenames.Add("TextBoxthing.png");
            Loads.Add(textbox.Texture);
        }
        else
        {
            int inx = texturenames.FindIndex(x => x == "TextBoxthing.png");
            textbox.Texture = Loads[inx];
        }
        

        textphoto.UI = true;
        textphoto.game = this;
        textphoto.X = -0.55f;
        textphoto.Y = 0.75f;
        textphoto.Width = 1.5f;
        textphoto.Height = 1f;
        if (texturenames.Contains("Beantext.png") == false)
        {
            textphoto.Texture = LoadTexture("Beantext.png");
            texturenames.Add("Beantext.png");
            Loads.Add(textbox.Texture);
        }
        else
        {
            int inx = texturenames.FindIndex(x => x == "Beantext.png");
            textphoto.Texture = Loads[inx];
        }
        
        textphoto.Comp.Add(new TextBox(this, textphoto));
        textphoto.Comp.Add(new Animation(2, 0.12f, 0, 0, 0.8f, 0.3f));
        textndex = 0;

        Faright = editor.MaXX;
        Farup = editor.MaYY;

        
    }

    public void checkLVL()
    {
        LvlJB = Index;
        LvlJBS = "notnullol";
    }


    public void UpdateText(string text)
    {
        textureobject.Width = 0.75f;
        int maxWidth = 700;
        textureobject.Texture = CreateTextTexture(
            text, 
            "Arial",
            64,
            1500,
            out int textWidth,
            out int textHeight
        );
        float pixelstoworld = 0.0015f;

        textureobject.Width = textWidth * pixelstoworld;
        textureobject.Height = textHeight * pixelstoworld;
        textureobject.X = -0.2f;
        textureobject.Y = 0.75f;
        textureobject.UI = true;
    }

   
    protected override void OnUpdateFrame(FrameEventArgs arg)
    {
        
        if (LvlJBS == "null")
        {
            LvlJB = "null";
        }
        LvlJBS = LvlJB;
        keyfound = false;
        float dt = (float)arg.Time;
        Arg = arg;
        var input = KeyboardState;
        if (input.IsKeyPressed(Keys.P) && player_exist == true && Index != "Menu" && Index != "LvlSlctr")
        {
            pause = !pause;
        }
        PS += (float)arg.Time;
        if (PS >= 1)
        {
            PS = 0;
            //Console.WriteLine(Fwames);
            Fwames = 0;
        }
        Fwames += 1;

        if (loadaudio == false)
        {
            next = Audio.LoadOgg("Next.ogg");
            loadaudio = true;
        }

        infjump = editor.infjump;
        foreach (var obj in objects)
        {
            if (pause == false)
            {
                
                obj.Update(dt, input);
                if (keyfound == false)
                {
                    if (obj.ObjectName != null)
                    {
                        if (obj.ObjectName.Contains("key"))
                        {
                            MainKey = obj;
                            keyfound = true;
                            obj.GetComponent<BoxCollider>().Tag = "Key";
                            obj.ObjectName = "key";
                            obj.Render = true;
                        }
                    }

                }
                else
                {
                    if (obj.ObjectName != null)
                    {
                        if (obj.ObjectName.Contains("key"))
                        {
                            obj.GetComponent<BoxCollider>().Tag = "notkey";
                            obj.ObjectName = "notkey";
                            obj.Render = false;
                        }
                    }

                }
            }
            

        }

        if (pause_thing != null && pause_thing.GetComponent<Pause>().backbutt != null)
        {
            pause_thing.Update(dt, input);
            pause_thing.GetComponent<Pause>().backbutt.Update(dt, input);
        }

        textphoto.Update(dt, input);
        timer += (float)arg.Time;
        if (textover == false && (textndex) <= AllText.Count)
        {
            
            if (timer > 0.3f)
            {

                if (input.IsKeyPressed(Keys.X) && textndex > 0 && startlvl == false)
                {
                    Audio.PlaySound(next);
                    timer = 0;


                    if (textndex >= AllText.Count)
                    {
                        textover = true;
                        startlvl = true;
                        textrender = false;
                    }
                    else
                    {
                        textrender = true;
                        UpdateText(AllText[textndex]);
                        if (Eventt[textndex] == true)
                        {

                            currentevent = eventtitles[eventindex];
                            eventindex += 1;
                        }
                        textndex += 1;

                    }
                }
                else if (textndex == 0 && startlvl == false)
                {
                    timer = 0;


                    if (textndex >= AllText.Count)
                    {
                        textover = true;
                        textrender = false;
                    }
                    else
                    {
                        textrender = true;
                        UpdateText(AllText[textndex]);
                        if (Eventt[textndex] == true)
                        {

                            currentevent = eventtitles[eventindex];
                            eventindex += 1;
                        }
                        textndex += 1;

                    }
                }


            }

        }
        else
        {
            textrender = false;
        }

        if (Index == "1Lvl3.1")
        {
            shake = true;
        }

        
        MouseX = MousePosition.X;
        MouseY = MousePosition.Y;

        predirect = direct;
        preindex = Index;

        direct = newdirect;
        Index = newindex;

        if (mainBG != null)
        {
            mainBG.X = CameraPos.X;
        }
        

        if (Trig == true)
        {
            editor.newTrigger(direct, Index, 2);
            textover = false;
            textndex = 0;
            AllText = editor.Dialog;
            Talker = editor.Speaker;
            Eventt = editor.Event;
            eventtitles = editor.Eventtitles;
            Trig = false;
        }

        if (predirect != direct || preindex != Index)
        {
            Console.WriteLine("Moving scenes!");
            editor.handle.Clear();
            IOUjects.Clear();
            objects.Clear();
            Dect.objects.Clear();
            CameraPos.X = 0;
            CameraPos.Y = 0;
            BaseX = 0;
            BaseY = 0;
            textrender = false;
            startlvl = false;
            editor.LoadNewScene(direct, Index);
            OnLoad();
            return;
        }
        
        if (BaseX - CamDestinationX > 0.015f || BaseX - CamDestinationX < -0.015f)
        {
            if (BaseX < CamDestinationX && BaseX <= Faright)
            {
                BaseX += 0.19f * dt;
            }
            else if (BaseX > CamDestinationX && BaseX >= 0)
            {
                BaseX -= 0.19f * dt;
            }
            else
            {
                BaseX += 0 * dt;
            }
        }
        else
        {
            BaseX += 0 * dt;
        }

        if (BaseY - CamDestinationY > 0.015f || BaseY - CamDestinationY < -0.015f)
        {
            if (BaseY < CamDestinationY && BaseY <= Farup)
            {
                BaseY += 0.19f * dt;
            }
            else if (BaseY > CamDestinationY && BaseY >= 0)
            {
                BaseY -= 0.19f * dt;
            }
            else
            {
                BaseY += 0 * dt;
            }
        }
        else
        {
            BaseY += 0 * dt;
        }

        if (shake == false)
        {
            CameraPos.X = BaseX;
            CameraPos.Y = BaseY;
        }
        else
        {
            float randx = -0.0035f + (float)rand.NextDouble() * (0.0035f -(-0.0035f));
            float randy = -0.0035f + (float)rand.NextDouble() * (0.0035f - (-0.0035f));
            CameraPos.X = BaseX + randx;
            CameraPos.Y = BaseY + randy;
        }

        
        if (currentevent == "leave")
        {
            checkLVL();
            currentevent = "null";
            newdirect = "Scenes";
            newindex = "LvlSlctr";
        }

       if (CameraPos.Y < 0)
        {
            CameraPos.Y = 0;
        }

        if (Playerpos != null)
        {
            if (Playerpos.X >= RightX)
            {
                CamDestinationX = RightX;
                LeftX = RightX - editor.otherXdif;
                RightX += editor.otherXdif;

            }
            else if (Playerpos.X <= LeftX)
            {
                CamDestinationX = LeftX;
                RightX = LeftX + editor.otherXdif;
                LeftX -= editor.otherXdif;
            }

            if (Playerpos.Y >= UpY)
            {
                CamDestinationY = UpY;
                DownY = UpY - editor.otherXdif;
                UpY += editor.otherXdif;

            }
            else if (Playerpos.Y <= DownY)
            {
                CamDestinationY = DownY;
                UpY = DownY + editor.otherXdif;
                DownY -= editor.otherXdif;
            }
        }
        
        foreach (var IOU in IOUjects)
        {
            objects.Add(IOU);
            GameButton butt = IOU.GetComponent<GameButton>();

            Console.WriteLine("NewButton is " + butt);
            if (butt != null)
            {
                buttonz.Add(butt);
            }
        }
        IOUjects.Clear();

        foreach (var rej in rejjects)
        {
            objects.Remove(rej);
            BoxCollider col = rej.GetComponent<BoxCollider>();
            if (col != null)
            {
                Dect.objects.Remove(col.coll);
            }
            

            
        }
        rejjects.Clear();

        if (Reload == true)
        {
            ReloadScene();
        }
    }

    public void ReloadScene()
    {
        Console.WriteLine("THIS IS A RELARD THIS IS A RELARP");
        editor.handle.Clear();
        IOUjects.Clear();
        objects.Clear();
        Dect.objects.Clear();
        editor.LoadNewScene(direct, Index);
        CameraPos.X = 0;
        CameraPos.Y = 0;
        CamDestinationX = 0;
        CamDestinationY = 0;
        currentevent = "null";
        BaseX = 0;
        BaseY = 0;
        OnLoad();
        return;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        render.BeingDraw();
        
        
        foreach (var obj in objects)
        {
            
            if (obj.Render == true)
            {
                if (obj.ObjectName == "purlarp")
                {
                    obj.X = obj.Fatherlarp.X;
                    obj.Y = obj.Fatherlarp.Y;
                }

                if (obj.UI == false)
                {
                    if (obj.ObjectName != null)
                    {
                        if (obj.ObjectName != "pause" || obj.ObjectName != "hock")
                        {
                            render.Draw(obj, this); //'Draws' the GameObject
                        }
                    }
                    
                }
                else
                {
                    render.DrawUI(obj, this);
                }

                if (obj.Purp == true)
                {
                    Console.WriteLine("PURPUDOIEOFJFOEF");
                    GameObject pury = new GameObject();
                    pury.ObjectName = "purlarp";
                    pury.Fatherlarp = obj;
                    pury.X = obj.X;
                    pury.Y = obj.Y;
                    pury.Width = obj.Width + 0.2f;
                    pury.Height = obj.Height + 0.2f;
                    pury.Texture = purpy;
                    IOUjects.Add(pury);
                    obj.Purp = false;
                }
                
            }
            
        }

        if (textrender == true)
        {
            textbox.Opacity = 0.45f;
            render.DrawUI(textbox, this);
            render.DrawUI(textphoto, this);
            render.DrawUI(textureobject, this);
        }

        if (pause_thing != null && pause_thing.GetComponent<Pause>().backbutt != null)
        {
            render.Draw(pause_thing, this);
            render.Draw(pause_thing.GetComponent<Pause>().backbutt, this); //'Draws' the GameObject
        }
       



        GL.ClearColor(0, 0, 0, 1);
        SwapBuffers();
    }


    protected override void OnUnload()
    {
        render.Cleanup();
    }




}
