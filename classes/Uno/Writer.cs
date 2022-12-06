using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amnesia.interfaces;

namespace Amnesia.classes.Uno
{
    public static class Writer
    {

        public static readonly List<object> ObjForWrite = new List<object>();
        public static readonly List<object> ObjForClear = new List<object>();
        private static Thread _thread;

        /// <summary>
        /// Init Writer Thread
        /// </summary>
        public static void Init()
        {
            _thread = new Thread(WriteLoop)
            {
                Name = "Writer"
            };
            _thread.Start();
        }

        /// <summary>
        /// Stop Writer Thread
        /// </summary>
        public static void Stop()
        {
            _thread.Abort();
        }
        
        /// <summary>
        /// Add object to write
        /// </summary>
        /// <param name="obj">Object to write</param>
        public static void Write(object obj)
        {
            ObjForWrite.Add(obj);
        }
        
        /// <summary>
        /// Add object to clear
        /// </summary>
        /// <param name="obj">Object to clear</param>
        public static void Clear(object obj)
        {
            ObjForClear.Add(obj);
        }

        /// <summary>
        /// Loop for writer thread
        /// </summary>
        private static void WriteLoop()
        {
            while (true)
            {
                Thread.Sleep(1);
                object obj;
                var count = ObjForClear.Count;
                if (count > 0)
                {
                    obj = ObjForClear.First();
                    ObjForClear.Remove(obj);
                    if (obj.GetType().GetInterface(typeof(IDrawable).ToString()) == null) continue;
                    ((IDrawable) obj).Clear(true);
                }
                else
                {
                    count = ObjForWrite.Count;
                    if (count <= 0) continue;
                    obj = ObjForWrite.First();
                    ObjForWrite.Remove(obj);
                    if (obj.GetType().GetInterface(typeof(IDrawable).ToString()) == null) continue;
                    ((IDrawable) obj).Clear(false);
                    ((IDrawable) obj).Draw();
                }
            }
        }
        
    }
}