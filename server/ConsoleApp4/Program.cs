using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerForBattleShip
{
    class Program
    {
        static TcpListener tcpListener = new TcpListener(IPAddress.Any, 420);
        static List<TcpClient> clients = new List<TcpClient>();
        static string data = null;
        static byte[] b = new byte[256];
        private static int log = 0;
        private static void Main(string[] args)
        {
            tcpListener.Start();
            Console.WriteLine("server start");
            int j = 0;
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    if (client.Connected)
                    {
                        j++;
                        Console.WriteLine($"user {j} connect ");
                        NetworkStream sender = client.GetStream();
                        Byte[] _data = System.Text.Encoding.Unicode.GetBytes("waiting game");
                        sender.Write(_data, 0, _data.Length);
                        clients.Add(client);
                    }
                    if (clients.Count == 2)
                    {
                        SendToAllClients("player found");
                        PlayRoom(client);
                    }
                    else
                    {
                        AwaitRoom(client);
                    }
                        
                    //ReciveFromClients(client);
                });
            }
        }

        static async private void AwaitRoom(TcpClient client)
        {
            await Task.Factory.StartNew(() =>//отправка всем клиентам
            {
                bool wait = true;
                while(wait)
                {
                    if(clients.Count == 2)
                    {
                        wait = false;
                        
                    }
                    System.Threading.Thread.Sleep(100);
                }
                PlayRoom(client);
            
            });
        }


        static async private void PlayRoom(TcpClient client)
        {
            await Task.Factory.StartNew(() =>
            {
                NetworkStream stream = client.GetStream();
                Byte[] vs = new Byte[10];
                stream.Read(vs, 0, 10);
                string data = System.Text.Encoding.Unicode.GetString(vs, 0, 10);
                if (data == "done")
                {
                    log++;
                    if (log == 2)
                        SendToAllClients("enemy done");
                }
                while (log != 2)
                {
                    System.Threading.Thread.Sleep(100);
                }



            });
        }





        static private void ReciveFromClients(TcpClient client)
        {
            Task.Factory.StartNew(() =>//принятие писем от клиентов
            {
                NetworkStream a = client.GetStream();
                var i = 0;
                while ((i = a.Read(b, 0, b.Length)) != 0)
                {
                    data = System.Text.Encoding.Unicode.GetString(b, 0, i);
                    Console.WriteLine(data);
                    SendToAllClients(data);
                    if (!client.Connected)
                        break;
                }
            });
        }
        static async void SendToAllClients(string _sa)
        {
            await Task.Factory.StartNew(() =>//отправка всем клиентам
            {
                var send = _sa;
                for (int i = 0; i < clients.Count; i++)
                {
                    NetworkStream sender = clients[i].GetStream();
                    Byte[] _data = System.Text.Encoding.Unicode.GetBytes(send);
                    sender.Write(_data, 0, _data.Length);
                }
            });
        }
    }
}