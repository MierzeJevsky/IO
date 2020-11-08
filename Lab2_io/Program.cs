using ServerAPMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SerwerTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            ServerAPM server = new ServerAPM(ipAdr, 2048);
            server.Start();
        }
    }
}
