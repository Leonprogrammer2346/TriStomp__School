using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;



class Program
{
    static void Main()
    {
        
        using var editor = new Editor();
        
        using var game = new Game();
        game.editor = editor;
        editor.game = game;
        editor.Run();
        //game.Run();
    }
}