using System;

namespace MatchCutes
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MatchCutesGame game = new MatchCutesGame())
            {
                game.Run();
            }
        }
    }
#endif
}

