using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SeaBattleConsole
{
    internal class Program
    {
        //private static TcpClient client;

        private static void Main(string[] args)
        {
            bool exit = false;
            while (exit == false)
            {
                Board game = new Board();
                game.drawLogo();
                Console.WriteLine("Press 1 to start\n Press 0 to exit\n");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        exit = true;
                        break;

                    case "1":
                        Console.Clear();
                        game.drawLogo();
                        game.start();
                        break;

                    default:
                        Console.Clear();
                        break;
                }
            }
            // game.start();
        }
    }
}