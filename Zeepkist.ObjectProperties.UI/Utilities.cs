using System.Globalization;

namespace TNRD.Zeepkist.ObjectProperties.UI
{
    internal class Utilities
    {
        public static float TryParseFloat(string input, CultureInfo cultureInfo)
        {
            return float.TryParse(input, NumberStyles.Float, cultureInfo, out float value) ? value : 0;
        }

        public static int TryParseInt(string input, CultureInfo cultureInfo)
        {
            return int.TryParse(input, NumberStyles.Integer, cultureInfo, out int value) ? value : 0;
        }
    }
}
