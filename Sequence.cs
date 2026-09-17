using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Sequence : Component
{
    GameObject obj;
    public static List<int> images = new();
    public static List<string> names = new();
    public bool animgot;
    public int length;
    public string name;
    public int index;
    public bool gorest;
   
    

    public Sequence(int seqlength, string Name, GameObject Obj)
    {
        length = seqlength;
        name = Name;
        obj = Obj;
    }

    public override Component Clone(GameObject newObj)
    {
        obj = newObj;
            return new Sequence(length, name, obj);
    }

    public override void Update(float dt, KeyboardState input)
    {
        if (animgot == false)
        {
            for (int i=0;i<length;i++)
            {
                if (i == 0 && !names.Contains(name + "_" + i))
                {
                    names.Clear();
                    images.Clear();
                    
                    gorest = true;
                }
                if (gorest == true)
                {
                    Console.WriteLine("Piggytwig");
                    images.Add(obj.game.LoadTexture(name + "_" + i + ".png"));
                    names.Add(name + "_" + i + ".png");
                }
                
            }
            animgot = true;
        }

        obj.Texture = images[index];

        if (input.IsKeyPressed(Keys.X))
        {
            Console.WriteLine("OREYSYS");
            Console.WriteLine(index);
            if (index < images.Count-1)
            {
                index += 1;
            }
            else
            {
                obj.game.newdirect = "Scenes";
                obj.game.newindex = "LvlSlctr";
            }
            
        }

        if (input.IsKeyPressed(Keys.Z))
        {
           
                obj.game.newdirect = "Scenes";
                obj.game.newindex = "LvlSlctr";
            

        }
    }

}