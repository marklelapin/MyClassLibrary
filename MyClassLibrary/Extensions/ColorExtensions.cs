﻿using System.Drawing;

namespace MyClassLibrary.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color c)
        {
            return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        }
    }
}
