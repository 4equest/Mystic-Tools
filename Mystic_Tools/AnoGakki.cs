using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools
{
    internal class AnoGakki : IAnimation
    {
        public void Start()
        {
            while (!Cts.Token.IsCancellationRequested)
            {
                


            }
        }

        public void Stop()
        {
            Cts.Cancel();
        }

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();
    }
}
