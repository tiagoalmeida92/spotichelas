using System;
using UI.Controllers;
using WebGarten2;

namespace UI
{
    public static class Server
    {
        private static readonly HttpHost Host = new HttpHost("http://localhost:8080");

        private static readonly Type[] Controllers =
            {
                typeof (HomeController),
                typeof (PlaylistController),
                typeof (SearchController)
            };

        static Server()
        {
            Host.Add(DefaultMethodBasedCommandFactory.GetCommandsFor(Controllers));
        }

        public static void RunServer()
        {
            Host.Open();
        }

        public static void StopServer()
        {
            Host.Close();
        }

        private static void Main()
        {
            RunServer();
            Console.WriteLine("Running...");
            Console.Read();
            StopServer();
        }
    }
}