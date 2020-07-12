using Microsoft.Win32;
using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;

namespace ConsoleCore1
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayList arr = new ArrayList();
            arr.Add(107);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string registryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                var key = Registry.LocalMachine.OpenSubKey(registryKey);
                var value = key.GetValue("ProductName");
            }

            Console.WriteLine("Hello World!");

            WebClient wc = new WebClient();
            //..
        }
    }
}
