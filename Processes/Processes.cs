using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace System_311.Processes
{
    internal class Processes
    {
        public void Run()
        {
            String? choice;
            do
            {
                Console.WriteLine("Which processes to stert?");
                Console.WriteLine("1 - Notepad");
                Console.WriteLine("2 - Notepad with Program.cs");
                Console.WriteLine("0 - Exit");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": Process.Start("notepad.exe"); break;
                    case "2": 
                        String dir=Directory.GetCurrentDirectory();
                        dir=Regex.Replace(dir, @"\\bin\\.*", @"\");
                        Process.Start("notepad.exe",Path.Combine(dir,"Program.cs"));
                        Console.WriteLine(dir); 
                        break;
                }
            }
            while (choice !="0");
            


        }
    }
}
