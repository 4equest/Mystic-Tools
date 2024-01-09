namespace Mystic_Tools
{
    internal static class Program
    {

        static void Main()
        {
            KeyboardManager keyboardManager = new();

            //keyboardManager.StartWindowWatcher();

            // if you do not want to play video, comment out the following line
            keyboardManager.VideoPlay("badapple.mp4");
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}