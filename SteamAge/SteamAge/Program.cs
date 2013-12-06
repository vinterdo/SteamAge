using System;

namespace SteamAge
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
      {
            using (SteamAge game = new SteamAge())
            {
                game.Run();
            }
        }
    }
}

