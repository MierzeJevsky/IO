using ServerLibrary;
using System;

using System.Collections.Generic;

using System.IO;

using System.Linq;

using System.Net;

using System.Net.Sockets;

using System.Text;



namespace ServerAPMLibrary

{

    public class ServerAPM : Server

    {
        static string passwd = "okon";
        static string msg1 = "\n\rpodaj haslo:";
        static string msg2 = "\n\rzle haslo\n\r";

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerAPM(IPAddress IP, int port) : base(IP, port)

        {

        }

        protected override void AcceptClient()

        {

            while (true)

            {

                TcpClient tcpClient = TcpListener.AcceptTcpClient();

                Stream = tcpClient.GetStream();

                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                //callback style

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);

                // async result style

                //IAsyncResult result = transmissionDelegate.BeginInvoke(Stream, null, null);

                ////operacje......

                //while (!result.IsCompleted) ;

                ////sprzątanie

            }

        }



        private void TransmissionCallback(IAsyncResult ar)

        {
            TcpClient client = (TcpClient)ar.AsyncState;
            client.Close();
            // sprzątanie

        }

        protected override void BeginDataTransmission(NetworkStream stream)

        {

            byte[] buffer = new byte[Buffer_size];

            while (true)

            {

                try

                {
                    byte[] msg = Encoding.ASCII.GetBytes(msg1);

                    stream.Write(msg, 0, msg.Length);

                    int message_size = stream.Read(buffer, 0, Buffer_size);

                    string reply = Encoding.ASCII.GetString(buffer);

                    var reply_letters = new String(reply.Where(Char.IsLetter).ToArray());

                    Array.Clear(buffer, 0, buffer.Length);

                    if (passwd == reply_letters)
                    {
                        
                        DateTime localDate = DateTime.Now;

                        string x = localDate.ToString("dd/MM/yyyy HH:mm:ss");

                        byte[] poz_reply = Encoding.ASCII.GetBytes(x);

                        stream.Write(poz_reply, 0, poz_reply.Length);

                        break;

                    }
                    else
                    {
                        byte[] neg_reply = Encoding.ASCII.GetBytes(msg2);

                        stream.Write(neg_reply, 0, neg_reply.Length);

                        break;
                    }

                }

                catch (IOException e)

                {

                    break;

                }

            }

        }

        public override void Start()

        {

            StartListening();

            //transmission starts within the accept function

            AcceptClient();

        }



    }

}

