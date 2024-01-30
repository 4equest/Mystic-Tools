using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Tools
{
    public class Keyboard : KeyMapConverter
    {
        public Keyboard()
        {
        }

        public virtual void SetCustomizeRGBColor(int keyCode, byte R, byte G, byte B, byte Brightness = 255)
        {
        }

        public virtual void SetCustomizeRGBColor(int keyCode, Color color, byte Brightness = 255)
        { 
        }
        public virtual void SetCustomize( byte Brightness = 255)
        {
        }
    }
}
