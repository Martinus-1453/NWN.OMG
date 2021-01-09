using NWN.API;

namespace OMG.Util
{
    public static class Colors
    {
        #region BasicColors
        public static Color Red { get; } = new Color(255, 0, 0);
        public static Color Green { get; } = new Color(0, 255, 0);
        public static Color Blue { get; } = new Color(0, 255, 0);
        public static Color Black { get; } = new Color(0, 0, 0);
        public static Color White { get; } = new Color(255, 255, 255);
        public static Color Yellow { get; } = new Color(255, 255, 0);
        public static Color Cyan { get; } = new Color(0, 255, 255);
        public static Color Magenta { get; } = new Color(255, 0, 255);
        public static Color Gray { get; } = new Color(128, 128, 128);
        public static Color Maroon { get; } = new Color(128, 0, 0);
        public static Color Olive { get; } = new Color(128, 128, 0);
        public static Color Purple { get; } = new Color(128, 0, 128);
        public static Color Teal { get; } = new Color(0, 128, 128);
        public static Color Navy { get; } = new Color(0, 0, 128);
        #endregion

        #region ExtendedColors
        public static Color NavyBlue { get; } = new Color(0, 116, 214);
        public static Color Orange { get; } = new Color(255, 165, 0);
        public static Color DarkOrange { get; } = new Color(255, 140, 0);
        public static Color Brown { get; } = new Color(165, 42, 42);
        public static Color SaddleBrown { get; } = new Color(139, 69, 19);
        public static Color DarkRed { get; } = new Color(139, 0, 0);
        public static Color Pink { get; } = new Color(255, 192, 203);
        public static Color Salmon { get; } = new Color(250, 128, 114);
        public static Color Gold { get; } = new Color(255, 215, 0);
        public static Color Silver { get; } = new Color(192, 192, 192);
        #endregion

    }
}