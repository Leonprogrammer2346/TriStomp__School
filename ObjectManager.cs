using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ObjectManager
{
    

    public GameObject son = new();
    public List<Component> Cmp = new();

    public float X;
    public float Y;
    public float Width;
    public float Height;


    public T GetComponent<T>() where T : Component
    {
        return Cmp.OfType<T>().FirstOrDefault();
    }

    public ObjectManager()
    {
        Console.WriteLine("son");


    }
}