using System;
using System.Runtime.Loader;

namespace ConsoleCoreApp2
{
    class Program
    {
        //dotnet run -f netcore3.1/net40/net42
        static void Main(string[] args)
        {
#if NET40
            Console.WriteLine("Hello 4.0!");
#elif NET45
var ppp = new PersonNetFX();
            Console.WriteLine("Hello 4.5!");
#elif NETCOREAPP
var ppp = new PersonNetFX();
            Console.WriteLine("Hello core!");
#endif

            AssemblyLoadContext
        }
    }
}
