using System;
using Amnesia.classes.Uno;
using Amnesia.events;

namespace Amnesia
{
    internal class Program
    {
        public static readonly Random Random = new Random();
        public static int WindowsWidth = Console.WindowWidth;
        public static int WindowsHeight = Console.WindowHeight;
        
        public static void Main(string[] args)
        {
            
            Writer.Init();
            Console.CursorVisible = false;
            
            var gameManager = new GameManager();
            gameManager.StartGame();
            // ResizeWindow.Init(gameManager);
            
            
            
            Console.ReadKey();

        }
    }
}