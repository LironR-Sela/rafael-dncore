using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppClassic1
{
    class Program
    {
        static void Main(string[] args)
        {
            string registryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            var key = Registry.LocalMachine.OpenSubKey(registryKey);
            var value = key.GetValue("ProductName");

        }
    }
}
