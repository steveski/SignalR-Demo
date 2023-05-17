using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Timers;

namespace SignalRThingy_Service
{
    internal class Program
    {
        private static Timer _timer;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting SignalR Hub");
            var url = "http://localhost:8080";
            using(WebApp.Start(url))
            {
                _timer = new Timer(10000);
                _timer.Elapsed += Elapsed;
                _timer.AutoReset = true;
                _timer.Enabled = true;

                Console.ReadKey();
            }
        }

        private static void Elapsed(object source, ElapsedEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<StatusHub>();
            context.Clients.All.SendStatus(Environment.MachineName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));

        }
    }
}
