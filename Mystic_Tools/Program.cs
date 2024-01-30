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

            IAnimation animation = new AnoGakki();

            // Startメソッドを非同期で実行
            Task startTask = Task.Run(() => animation.Start());

            // 必要に応じて、Stopメソッドを非同期で実行
            Task stopTask = Task.Run(() => animation.Stop());

            // Stopタスクの完了を待つ
            stopTask.Wait();

        }
    }
}