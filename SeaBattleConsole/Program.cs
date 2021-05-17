using System;
using System.Diagnostics;
using System.IO;

namespace SeaBattleConsole
{
    internal class Program
    {
        public static void PlaySound(string file)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"sounds\");

            string pathSTR = path + file;

            Process.Start(@"powershell", $@"-c (New-Object Media.SoundPlayer '{pathSTR}').PlaySync();");
        }

        public static void stopSound()
        {
            //Process.Start("cmd.exe", "/c taskkill /F /IM powershell.exe");

            Process[] proc = Process.GetProcesses();
            foreach (Process process in proc)
                if (process.ProcessName == "powershell")
                {
                    process.Kill();
                }
        }

        private static void Main(string[] args)
        {
            bool exit = false;
            PlaySound("menuMusic.wav");
            while (exit == false)
            {
                Board game = new Board();
                game.drawLogo();
                Console.WriteLine(@"
                    ()
                    ||q',,'
                    ||d,~
         (,---------------------,)
          ',       q888p       ,'
            \       986       /
             \  8p, d8b ,q8  /
              ) 888a888a888 (
             /  8b` q8p `d8  \              O
            /       689       \             |','
           /       d888b       \      (,---------,)
         ,'_____________________',     \   ,8,   /
         (`__________L|_________`)      ) a888a (    _,_
         [___________|___________]     /___`8`___\   }*{
           }:::|:::::}::|::::::{      (,=========,)  -=-
            '|::::}::|:::::{:|'  .,.    \:::|:::/    ~`~=
             '|}:::::|::{:::|'          ~'.,.'~`~
               '|:}::|::::|'~`~'.,.'
           ~`~'.,.'~`~'.,                 '~`~'.,.'~
                          '.,.'~`~

                          ");
                Console.WriteLine("1 - Start\n0 - Exit\n");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        exit = true;
                        stopSound();
                        Console.Clear();
                        break;

                    case "1":
                        stopSound();
                        PlaySound("place.wav");
                        Console.Clear();
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