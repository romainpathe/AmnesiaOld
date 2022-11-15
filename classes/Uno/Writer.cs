using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amnesia.interfaces;

namespace Amnesia.classes.Uno
{
    public static class Writer
    {

        public static List<object> ObjForWrite = new List<object>();
        public static List<object> ObjForClear = new List<object>();
        private static Thread _thread;
        
        public static void Init()
        {
            _thread = new Thread(Write)
            {
                Name = "Writer"
            };
            _thread.Start();
        }

        private static void Write()
        {
            while (true)
            {
                Thread.Sleep(1);
                if (ObjForClear.Count > 0)
                {
                var obj = ObjForClear.First();
                ObjForClear.Remove(obj);
                if (obj.GetType().GetInterface(typeof(IDrawable).ToString()) == null) continue;
                ((IDrawable) obj).Clear();
                }
                else
                {
                    if (ObjForWrite.Count <= 0) continue;
                    var obj = ObjForWrite.First();
                    ObjForWrite.Remove(obj);
                    if (obj.GetType().GetInterface(typeof(IDrawable).ToString()) == null) continue;
                    ((IDrawable) obj).Clear();
                    ((IDrawable) obj).Draw();
                }
                

            }
        }
        
    }
}