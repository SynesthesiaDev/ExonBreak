using osu.Framework.Platform;
using osu.Framework;
using ExonBreak.Game;

namespace ExonBreak.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"ExonBreak"))
            using (osu.Framework.Game game = new ExonBreakGame())
                host.Run(game);
        }
    }
}