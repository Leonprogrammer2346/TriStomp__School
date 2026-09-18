using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Record : Component
{
    GameObject obj;
    public float lastVX;
    public float nowVX;
    public float lastVY;
    public float nowVY;
    public bool recording;
    public float time;
    public string path;
    public string path1;
    public string path2;
    public string path3;
    public bool play;
    public string[] lines;
    public List<float> Xvel = new();
    public List<float> Yvel = new();
    public List<float> ximes = new();
    public List<float> yimes = new();

    public List<String> Sprite = new();
    public List<String> text = new();
    public List<String> charr = new();
    public List<float> spritime = new();
    public List<float> textime = new();
    public int Yindx;
    public int Xindx;
    public int Spndx;
    public int Tndx;
    public Component ploy;
    public static List<String> spritelist = new();
    public static List<int> texturelist = new();
    public bool firstframe = true;
    public int y;


    public Record(GameObject Obj)
    {
        obj = Obj;

    }

    public override Component Clone(GameObject newObj)
    {
        obj = newObj;

        return new Record(obj);


    }

    public override void Update(float dt, KeyboardState input)
    {
        if (input.IsKeyPressed(Keys.E))
        {
            recording = !recording;

            if (recording == true)
            {
                if (!play)
                {
                    time = 0;
                }

                Directory.CreateDirectory("RECORDINGS/" + obj.game.Index + "/" + obj.ObjectName);
                path = Path.Combine("RECORDINGS/" + obj.game.Index + "/" + obj.ObjectName, "X.txt");
                path1 = Path.Combine("RECORDINGS/" + obj.game.Index + "/" + obj.ObjectName, "Y.txt");
                path2 = Path.Combine("RECORDINGS/" + obj.game.Index, "Dialog.txt");
                path3 = Path.Combine("RECORDINGS/" + obj.game.Index + "/" + obj.ObjectName, "Sprite.txt");

                ploy = obj.GetComponent<Player>();
                File.WriteAllText(path, obj.X + "\n");
                File.WriteAllText(path1, obj.Y + "\n");
                File.WriteAllText(path2, "");
                File.WriteAllText(path3, "");
                nowVX = obj.GetComponent<Rigidbody>().XVelocity;
                nowVY = obj.GetComponent<Rigidbody>().YVelocity;
            }

        }

        if (input.IsKeyPressed(Keys.T) && recording)
        {
            File.AppendAllText(path2, time + ",Character,Dialog" + "\n");
            File.AppendAllText(path3, time + ",null.png" + "\n");
        }

        if (input.IsKeyPressed(Keys.F))
        {
            time = 0;
            Yindx = 0;
            Xindx = 0;
            y = 0;
            string[] files = Directory.GetFiles("RECORDINGS/" + obj.game.Index + "/" + obj.ObjectName, "*.txt");
            string[] files1 = Directory.GetFiles("RECORDINGS/" + obj.game.Index, "*.txt");
            int i = 0;
            if (files.Length > 0)
            {
                string filename = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == "X");
                lines = File.ReadAllLines(filename);
            }
            foreach (string line in lines)
            {
                if (i != 0)
                {
                    string[] full = line.Split(",");
                    ximes.Add(float.Parse(full[0]));
                    Xvel.Add(float.Parse(full[1]));
                    i++;
                }
                else
                {
                    obj.X = float.Parse(line);
                    i++;
                }

            }
            i = 0;


            if (files.Length > 0)
            {
                string filename = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == "Y");
                lines = File.ReadAllLines(filename);
            }
            foreach (string line in lines)
            {
                if (i != 0)
                {
                    string[] full = line.Split(",");
                    yimes.Add(float.Parse(full[0]));
                    Yvel.Add(float.Parse(full[1]));
                    i++;
                }
                else
                {
                    obj.Y = float.Parse(line);
                    i++;
                }
            }

            if (files.Length > 0)
            {
                string filename = files1.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == "Dialog");
                lines = File.ReadAllLines(filename);
            }
            foreach (string line in lines)
            {
                string[] full = line.Split(",");
                textime.Add(float.Parse(full[0]));
                charr.Add(full[1]);
                text.Add(full[2]);
            }

            if (files.Length > 0)
            {
                string filename = files.FirstOrDefault(file => Path.GetFileNameWithoutExtension(file) == "Sprite");
                lines = File.ReadAllLines(filename);
            }
            foreach (string line in lines)
            {
                string[] full = line.Split(",");
                if (full[1] != "null.png")
                {
                    spritime.Add(float.Parse(full[0]));
                    Sprite.Add(full[1]);
                }

            }
            play = !play;

        }

        if (recording == true)
        {
            time += dt;

            nowVX = obj.GetComponent<Rigidbody>().XVelocity;
            if (nowVX != lastVX)
            {
                File.AppendAllText(path, time + "," + obj.GetComponent<Rigidbody>().XVelocity + "\n");
            }
            lastVX = nowVX;

            nowVY = obj.GetComponent<Rigidbody>().YVelocity;
            if (nowVY != lastVY && ploy == null)
            {
                File.AppendAllText(path1, time + "," + obj.GetComponent<Rigidbody>().YVelocity + "\n");
            }
            else if (ploy != null)
            {
                if (obj.GetComponent<Player>().checkfall == false)
                {
                    File.AppendAllText(path1, time + "," + obj.GetComponent<Rigidbody>().YVelocity + "\n");
                }
            }
            lastVY = nowVY;

        }

        if (play == true)
        {
            time += dt;
           
            if (Xindx != ximes.Count - 1)
            {
                if (time >= ximes[Xindx])
                {
                    obj.GetComponent<Rigidbody>().XVelocity = Xvel[Xindx];
                    Xindx++;
                }
            }


            if (Yindx != yimes.Count - 1)
            {
                if (time >= yimes[Yindx])
                {
                    obj.GetComponent<Rigidbody>().YVelocity = Yvel[Yindx];
                    Yindx++;
                }
            }

            if (spritime.Count > 0)
            {
                if (Spndx != spritime.Count - 1)
                {
                    if (time >= spritime[Spndx])
                    {
                        int inx = spritelist.FindIndex(x => x == Sprite[Spndx]);
                        if (inx > -1)
                        {
                            obj.Texture = texturelist[inx];
                        }
                        else
                        {
                            texturelist.Add(obj.game.LoadTexture(Sprite[Spndx]));
                            spritelist.Add(Sprite[Spndx]);
                            obj.Texture = texturelist[texturelist.Count() - 1];
                        }

                        Spndx++;
                    }
                }
            }
            

            if (Tndx != textime.Count - 1)
            {
                if (time >= textime[Tndx])
                {
                    obj.game.Audio.PlaySound(obj.game.nnext);
                    obj.game.SceneChar = charr[Tndx];
                    obj.game.SceneText = text[Tndx];
                    obj.game.textover = false;
                }
            }



            if (Yindx == yimes.Count - 1 && Xindx == ximes.Count - 1 && Tndx == textime.Count - 1)
            {
                obj.game.textover = true;
                play = false;
            }

        }
    }

}
