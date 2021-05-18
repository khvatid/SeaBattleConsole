using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace ServerForBattleShip
{

   
    class Program
    {
        static TcpListener tcpListener = new TcpListener(IPAddress.Any, 420);
        static List<TcpClient> clients = new List<TcpClient>();
        static string data = null;
        static byte[] b = new byte[256];
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
                        PlayRoom();
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
            });
        }


        static void plan(object obj)
        {
            
            TcpClient client = (TcpClient)obj;
            NetworkStream stream =  client.GetStream();
            Byte[] vs = new Byte[256];
            int i = 0;

            while ((i = stream.Read(vs, 0, vs.Length)) != 0)
            {
                data = System.Text.Encoding.GetEncoding(866).GetString(vs, 0, i);
                if (data == "2588")
                {
                    Console.WriteLine(data);
                    return;
                }
            }
        }
        // 0 не попал  -  1 и 2 попал
       static int playStage(NetworkStream player1, NetworkStream player2)
        {
            int i = 0;
            int j = 0;
            while((i = player1.Read(b,0,b.Length))!=0)
            {
                int data1 = BitConverter.ToInt32(b, 0);
                int data2;
                player2.Write(b,0,i);
                while((j = player2.Read(b,0,b.Length))!=0)
                {
                    data2 = BitConverter.ToInt32(b, 0);
                    player1.Write(b, 0, j);
                    return data2;
                }
            }
            return 0;
        }





        static private void PlayRoom()
        {
            List<Thread> threads = new List <Thread>();
            threads.Add(new Thread(new ParameterizedThreadStart(plan)));
            threads.Add(new Thread(new ParameterizedThreadStart(plan)));
            threads[0].Start(clients[0]);
            threads[1].Start(clients[1]);
            for(int i=0;i<threads.Count;i++)
            {
                threads[i].Join();
            }
            SendToAllClients("enemy done");
            SendToClient(clients[0],"1");
            SendToClient(clients[1], "0");
            threads.Clear();
            bool firstPlayer = true;
            try
            {
                while (true)
                {
                    if (firstPlayer == true)
                    {
                        int turn = playStage(clients[0].GetStream(), clients[1].GetStream());
                        if (turn == 0) firstPlayer = false;

                    }
                    else
                    {
                        int turn = playStage(clients[1].GetStream(), clients[0].GetStream());
                        if (turn == 0) firstPlayer = true;
                    }
                }
            }
           catch(Exception e)
            {
                Console.WriteLine(e.Message);
                clients.Clear();
                return;
            }
            

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

        static async void SendToClient(TcpClient client, string _sa)
        {
            await Task.Factory.StartNew(() =>//отправка всем клиентам
            {
                var send = _sa;
                NetworkStream sender = client.GetStream();
                Byte[] _data = System.Text.Encoding.Unicode.GetBytes(send);
                sender.Write(_data, 0, _data.Length);
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