using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.IO;

class Renderer
{
    int texture;
    int vao, vbo;
    int shader;
    int modelLocation;
    int projectionLocation;
    float left;
    float right;
    



    public Renderer()
    {
        //  X      Y      U    V
        float[] vertices =
     {
         0.1f,  0.1f, 1f, 1f,
         0.1f, -0.1f, 1f, 0f,
        -0.1f, -0.1f, 0f, 0f,

        -0.1f, -0.1f, 0f, 0f,
        -0.1f,  0.1f, 0f, 1f,
        0.1f,  0.1f, 1f, 1f
     };

     

        shader = CreateShaderProgram();
        GL.UseProgram(shader);
        Matrix4 projection = Matrix4.Identity;
        projectionLocation = GL.GetUniformLocation(shader, "projection");
        GL.UniformMatrix4(projectionLocation, false, ref projection);
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        modelLocation = GL.GetUniformLocation(shader, "model");
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw);

        // position
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // texture coords
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        int texLocation = GL.GetUniformLocation(shader, "tex");
        GL.Uniform1(texLocation, 0);
        Console.WriteLine("Rendy Bendy");
    }
    
    public void BeingDraw()
    {
        GL.UseProgram(shader);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindVertexArray(vao);
    }
    public void Draw(GameObject obj, Game game)
    {

        

        Animation anim = obj.GetComponent<Animation>();
        if (anim != null)
        {
            int columns = obj.GetComponent<Animation>().column;
            int rows = obj.GetComponent<Animation>().row;

            int frame = anim.currentframe;

            
            int col = frame % columns;
            int row = frame / columns;

            float frameWidth = 1f / columns;
            float frameHeight = 1f / rows;

            float left = col * frameWidth;
            float right = left + frameWidth;


            float top = 1f - row * frameHeight;
            float bottom = top - frameHeight;

            float[] fertices =
         {
         0.1f,  0.1f, right, top,
         0.1f, -0.1f, right, bottom,
        -0.1f, -0.1f, left, bottom,

        -0.1f, -0.1f, left, bottom,
        -0.1f,  0.1f, left, top,
        0.1f,  0.1f, right, top
     };

            int opacityLocation = GL.GetUniformLocation(shader, "opacity");
            GL.Uniform1(opacityLocation, obj.Opacity);

            Matrix4 view =
            Matrix4.CreateTranslation(-game.CameraPos.X, -game.CameraPos.Y, 0f);

            Matrix4 model =
                Matrix4.CreateScale(anim.Width, anim.Height, 1f) *
                Matrix4.CreateTranslation(obj.X, obj.Y, 0f);

            Matrix4 finalMatrix = model * view;

            GL.UniformMatrix4(modelLocation, false, ref finalMatrix);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferSubData(
                BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                fertices.Length * sizeof(float),
                fertices
            );

            GL.BindTexture(TextureTarget.Texture2D, obj.Texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        else
        {
            float[] vertices =
     {
         0.1f,  0.1f, 1f, 1f,
         0.1f, -0.1f, 1f, 0f,
        -0.1f, -0.1f, 0f, 0f,

        -0.1f, -0.1f, 0f, 0f,
        -0.1f,  0.1f, 0f, 1f,
        0.1f,  0.1f, 1f, 1f
     };

            Matrix4 view =
            Matrix4.CreateTranslation(-game.CameraPos.X, -game.CameraPos.Y, 0f);

            Matrix4 model =
                Matrix4.CreateScale(obj.Width, obj.Height, 1f) *
                Matrix4.CreateTranslation(obj.X, obj.Y, 0f);

            Matrix4 finalMatrix = model * view;

            GL.UniformMatrix4(modelLocation, false, ref finalMatrix);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferSubData(
                BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                vertices.Length * sizeof(float),
                vertices
            );

            GL.UseProgram(shader);


            int opacityLocation = GL.GetUniformLocation(shader, "opacity");
            GL.Uniform1(opacityLocation, obj.Opacity);

            GL.BindTexture(TextureTarget.Texture2D, obj.Texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

            

        
    }

    public void DrawH(ObjectManager obj, Editor ed)
    {
        Matrix4 view =
            Matrix4.CreateTranslation(-ed.CameraPos.X, -ed.CameraPos.Y, 0f);

        Matrix4 model =
            Matrix4.CreateScale(obj.son.Width, obj.son.Height, 1f) *
            Matrix4.CreateTranslation(obj.son.X, obj.son.Y, 0f);

        Matrix4 finalMatrix = model * view;

        GL.UniformMatrix4(modelLocation, false, ref finalMatrix);

        GL.BindTexture(TextureTarget.Texture2D, obj.son.Texture);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void DrawUI(GameObject obj, Game ed)
    {
        Animation anim = obj.GetComponent<Animation>();
        if (anim != null)
        {
            int columns = obj.GetComponent<Animation>().column;
            int rows = obj.GetComponent<Animation>().row;

            int frame = anim.currentframe;


            int col = frame % columns;
            int row = frame / columns;

            float frameWidth = 1f / columns;
            float frameHeight = 1f / rows;

            float left = col * frameWidth;
            float right = left + frameWidth;


            float top = 1f - row * frameHeight;
            float bottom = top - frameHeight;

            float[] fertices =
         {
         0.1f,  0.1f, right, top,
         0.1f, -0.1f, right, bottom,
        -0.1f, -0.1f, left, bottom,

        -0.1f, -0.1f, left, bottom,
        -0.1f,  0.1f, left, top,
        0.1f,  0.1f, right, top
     };

            

            Matrix4 model =
                Matrix4.CreateScale(anim.Width, anim.Height, 1f) *
                Matrix4.CreateTranslation(obj.X, obj.Y, 0f);

            Matrix4 finalMatrix = model;

            GL.UniformMatrix4(modelLocation, false, ref finalMatrix);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferSubData(
                BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                fertices.Length * sizeof(float),
                fertices
            );

            GL.BindTexture(TextureTarget.Texture2D, obj.Texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
        else
        {
            float[] vertices =
     {
         0.1f,  0.1f, 1f, 1f,
         0.1f, -0.1f, 1f, 0f,
        -0.1f, -0.1f, 0f, 0f,

        -0.1f, -0.1f, 0f, 0f,
        -0.1f,  0.1f, 0f, 1f,
        0.1f,  0.1f, 1f, 1f
     };

           

            Matrix4 model =
                Matrix4.CreateScale(obj.Width, obj.Height, 1f) *
                Matrix4.CreateTranslation(obj.X, obj.Y, 0f);

            Matrix4 finalMatrix = model;

            GL.UniformMatrix4(modelLocation, false, ref finalMatrix);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferSubData(
                BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                vertices.Length * sizeof(float),
                vertices
            );

            GL.UseProgram(shader);


            int opacityLocation = GL.GetUniformLocation(shader, "opacity");
            GL.Uniform1(opacityLocation, obj.Opacity);

            GL.BindTexture(TextureTarget.Texture2D, obj.Texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }




    }



    public void DrawGRX(float X, int Texture, Editor ed)
    {
        GL.UseProgram(shader);
        Matrix4 view =
            Matrix4.CreateTranslation(-ed.CameraPos.X, -ed.CameraPos.Y, 0f);


        Matrix4 model =
        Matrix4.CreateScale(0.01f, 50, 1f) *
        Matrix4.CreateTranslation(X, 0, 0f);

        Matrix4 finalMatrix = model * view;

        GL.UniformMatrix4(modelLocation, false, ref finalMatrix);
        //GL.ActiveTexture(TextureUnit.Texture0);

        GL.BindTexture(TextureTarget.Texture2D, Texture);
        //GL.BindVertexArray(vao);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void DrawGRY(float Y, int Texture, Editor ed)
    {
        
        GL.UseProgram(shader);
        Matrix4 view =
            Matrix4.CreateTranslation(-ed.CameraPos.X, -ed.CameraPos.Y, 0f);


        Matrix4 model =
        Matrix4.CreateScale(50, 0.01f, 1f) *
        Matrix4.CreateTranslation(0, Y, 0f);

        Matrix4 finalMatrix = model * view;

        GL.UniformMatrix4(modelLocation, false, ref finalMatrix);
        //GL.ActiveTexture(TextureUnit.Texture0);

        GL.BindTexture(TextureTarget.Texture2D, Texture);
        //GL.BindVertexArray(vao);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void DrawB(Button obj)
    {
        GL.UseProgram(shader);

        Matrix4 model =
        Matrix4.CreateScale(obj.wid, obj.hight, 1f) *
        Matrix4.CreateTranslation(obj.x, obj.y, 0f);

        GL.UniformMatrix4(modelLocation, false, ref model);

        GL.UniformMatrix4(modelLocation, false, ref model);
        GL.ActiveTexture(TextureUnit.Texture0);

        GL.BindTexture(TextureTarget.Texture2D, obj.Texture);
        GL.BindVertexArray(vao);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void Drawy(float X, float Y, int Texture)
    {
        GL.UseProgram(shader);

        Matrix4 model =
        Matrix4.CreateScale(0.1f, 0.1f, 0.1f) *
        Matrix4.CreateTranslation(X, Y, 0f);

        GL.UniformMatrix4(modelLocation, false, ref model);

        GL.UniformMatrix4(modelLocation, false, ref model);
        GL.ActiveTexture(TextureUnit.Texture0);

        GL.BindTexture(TextureTarget.Texture2D, Texture);
        GL.BindVertexArray(vao);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

   

    public void Cleanup()
    {
        GL.DeleteBuffer(vbo);
        GL.DeleteVertexArray(vao);
    }


    int CreateShaderProgram()
    {
        string vertexSource = File.ReadAllText("vertex.glsl");
        string fragmentSource = File.ReadAllText("fragment.glsl");

        int vertex = CompileShader(ShaderType.VertexShader, vertexSource);
        int fragment = CompileShader(ShaderType.FragmentShader, fragmentSource);

        int program = GL.CreateProgram();
        GL.AttachShader(program, vertex);
        GL.AttachShader(program, fragment);
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            throw new Exception(GL.GetProgramInfoLog(program));
        }

        return program;
    }


    int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            throw new Exception(GL.GetShaderInfoLog(shader));
        }
        Console.WriteLine("Shadow");
        return shader;
    }

    


    
}