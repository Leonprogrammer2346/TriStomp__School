using OpenTK.Graphics.ES11;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class GameObject 
{
    public int Texture;
    public string textname;
    public float X;
    public float Y;
    public float Height;
    public float Width;
    public List<Component> Comp = new();
    int cont = 0;
    public int ObjID;
    public string ObjectName;
    public Game game;
    public float TrimLeft;
    public float TrimRight;
    public bool Render = true;
    public bool UI;
    public GameObject otherobj;
    public bool Purp;
    public bool purpgot;
    public float fixheight;
    public float fixwidth;
    public bool Changewid;
    public bool Changehgt;
    public float newheight;
    public float newwidth;
    public float axispeed;
    public bool forw = true;
    public Animation ann;
    public bool axisloop = true;
    public int frametime;
    public int framelimit;
    public float Opacity = 1.0f;
    public GameObject Fatherlarp;


    public GameObject()
    {
        Random rand = new Random();
        ObjID = rand.Next(1, 100000);
        Console.WriteLine("New GameObject: " + ObjID);
    }


    public T GetComponent<T>() where T : Component
    {
        return Comp.OfType<T>().FirstOrDefault();
    }


    public void Update(float dt, KeyboardState input)
    {
        frametime += 1;
        if (axisloop == true)
        {
            frametime = 0;
        }
        ann = GetComponent<Animation>();
        if (Changewid && ann == null && (frametime < framelimit))
        {
            // Keep the sprite facing the same direction.
            float widthSign = ann.fixwidth < 0 ? -1f : 1f;

            // Compare positive size values only.
            float currentSize = MathF.Abs(Width);
            float newSize = MathF.Abs(newwidth);
            float originalSize = MathF.Abs(fixwidth);

            // forw = moving toward the new width
            float targetSize = forw ? newSize : originalSize;

            if (currentSize < targetSize)
            {
                currentSize += axispeed;

                if (currentSize >= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else if (currentSize > targetSize)
            {
                currentSize -= axispeed;

                if (currentSize <= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else
            {
                forw = !forw;
            }

            
            Width = currentSize * widthSign;
        }

        if (Changewid && ann != null && (frametime < framelimit))
        {
            
            float widthSign = ann.fixwidth < 0 ? -1f : 1f;

           
            float currentSize = MathF.Abs(ann.Width);
            float newSize = MathF.Abs(newwidth);
            float originalSize = MathF.Abs(ann.fixwidth);

           
            float targetSize = forw ? newSize : originalSize;

            if (currentSize < targetSize)
            {
                currentSize += axispeed;

                if (currentSize >= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else if (currentSize > targetSize)
            {
                currentSize -= axispeed;

                if (currentSize <= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else
            {
                forw = !forw;
            }

            // Restore the negative or positive direction.
            ann.Width = currentSize * widthSign;
        }


        if (Changehgt && ann == null && (frametime < framelimit))
        {
            // Keep the sprite facing the same direction.
            float heightSign = fixwidth < 0 ? -1f : 1f;

            // Compare positive size values only.
            float currentSize = MathF.Abs(Height);
            float newSize = MathF.Abs(newheight);
            float originalSize = MathF.Abs(fixheight);

            // forw = moving toward the new width
            float targetSize = forw ? newSize : originalSize;

            if (currentSize < targetSize)
            {
                currentSize += axispeed;

                if (currentSize >= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else if (currentSize > targetSize)
            {
                currentSize -= axispeed;

                if (currentSize <= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else
            {
                forw = !forw;
            }

            // Restore the negative or positive direction.
            Height = currentSize * heightSign;
        }

        if (Changehgt && ann != null && (frametime < framelimit))
        {
            // Keep the sprite facing the same direction.
            float heightSign = ann.fixheight < 0 ? -1f : 1f;

            // Compare positive size values only.
            float currentSize = MathF.Abs(ann.Height);
            float newSize = MathF.Abs(newheight);
            float originalSize = MathF.Abs(ann.fixheight);

            // forw = moving toward the new width
            float targetSize = forw ? newSize : originalSize;

            if (currentSize < targetSize)
            {
                currentSize += axispeed;

                if (currentSize >= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else if (currentSize > targetSize)
            {
                currentSize -= axispeed;

                if (currentSize <= targetSize)
                {
                    currentSize = targetSize;
                    forw = !forw;
                }
            }
            else
            {
                forw = !forw;
            }

            // Restore the negative or positive direction.
            ann.Height = currentSize * heightSign;
        }

        if (ObjectName != null)
        {
            if ((Width > fixwidth && Height > fixwidth) && ObjectName.Contains("purp"))
            {
                Width -= 0.002f;
                Height -= 0.002f;
            }

            if ((Width < fixwidth && Height < fixwidth) && ObjectName.Contains("purp"))
            {
                Width = fixwidth;
                Height = fixheight;
            }
        }
        



        foreach (Component comp in Comp)
        {
            comp.Update(dt, input);
        }

    }
}

