using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using Color = System.Drawing.Color;

namespace HuyNKSeries
{
    public class Config
    {
        private static bool initialized = false;
     

        private static MenuWrapper _menu;

        private static Dictionary<string, MenuWrapper.BoolLink> _boolLinks = new Dictionary<string, MenuWrapper.BoolLink>();
        private static Dictionary<string, MenuWrapper.CircleLink> _circleLinks = new Dictionary<string, MenuWrapper.CircleLink>();
        private static Dictionary<string, MenuWrapper.KeyBindLink> _keyLinks = new Dictionary<string, MenuWrapper.KeyBindLink>();
        private static Dictionary<string, MenuWrapper.SliderLink> _sliderLinks = new Dictionary<string, MenuWrapper.SliderLink>();

        public static MenuWrapper Menu { get { return _menu; } }

        public static Dictionary<string, MenuWrapper.BoolLink> BoolLinks { get { return _boolLinks; } }
        public static Dictionary<string, MenuWrapper.CircleLink> CircleLinks { get { return _circleLinks; } }
        public static Dictionary<string, MenuWrapper.KeyBindLink> KeyLinks { get { return _keyLinks; } }
        public static Dictionary<string, MenuWrapper.SliderLink> SliderLinks { get { return _sliderLinks; } }

        private static void ProcessLink(string key, object value)
        {
            if (value is MenuWrapper.BoolLink)
                _boolLinks.Add(key, value as MenuWrapper.BoolLink);
            else if (value is MenuWrapper.CircleLink)
                _circleLinks.Add(key, value as MenuWrapper.CircleLink);
            else if (value is MenuWrapper.KeyBindLink)
                _keyLinks.Add(key, value as MenuWrapper.KeyBindLink);
            else if (value is MenuWrapper.SliderLink)
                _sliderLinks.Add(key, value as MenuWrapper.SliderLink);
        }

        public static void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

           
        }
    }
}
