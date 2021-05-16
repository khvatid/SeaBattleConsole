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
        static bool allhere = false;
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
                        Byte[] _data = System.Text.Encoding.Unicode.GetBytes("привет пидорас");
                        sender.Write(_data, 0, _data.Length);
                        clients.Add(client);
                    }

                    if (!allhere)
                        try
                        {

                            if (clients[0].Connected && clients[1].Connected)
                            {
                                allhere = true;
                                SendToAllClients("player found");
                            }
                        }
                        catch
                        {
                            SendToAllClients("ждем второго гандона");
                        }

                    // if (allhere)
                    ReciveFromClients(client);
                });
            }
        }

        static async private void AwaitRoom()
        {
            await Task.Factory.StartNew(() =>//отправка всем клиентам
            {
            
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