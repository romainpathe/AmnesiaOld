using System;
using System.Runtime.InteropServices;
using System.Threading;
using Amnesia.classes.Uno;

namespace Amnesia.events
{
    public class ResizeWindow
    {
        private static Thread _thread;
        private static GameManager _gameManager;
        public static void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            _thread = new Thread(new ThreadStart(Resize))
            {
                Name = "ResizeWindow"
            };
            _thread.Start();
        }
        
        private static void Resize()
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            while (true)
            {
                Thread.Sleep(1000);
                if (width == Console.WindowWidth && height == Console.WindowHeight) continue;
                width = Console.WindowWidth;
                height = Console.WindowHeight;
                // _gameManager.Redraw();
            }
        }
        
    }
}