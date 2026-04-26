using osu.Framework;
using osu.Framework.Platform;

namespace ExonBreak.Game.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost("visual-tests"))
            using (var game = new ExonBreakTestBrowser())
                host.Run(game);
        }
    }
}