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

            // Start���\�b�h��񓯊��Ŏ��s
            Task startTask = Task.Run(() => animation.Start());

            // �K�v�ɉ����āAStop���\�b�h��񓯊��Ŏ��s
            Task stopTask = Task.Run(() => animation.Stop());

            // Stop�^�X�N�̊�����҂�
            stopTask.Wait();

        }
    }
}