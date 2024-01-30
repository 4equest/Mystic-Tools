using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools
{
    internal interface IAnimation
    {
        CancellationTokenSource Cts { get; set; }
        void Start();
        void Stop();
    }
}
