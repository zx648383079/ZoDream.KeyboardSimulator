using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ZoDream.Shared.Utils
{
    public static class ColorHelper
    {
        /// <summary>
        /// 返回大写无#的颜色值
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string To(Color color)
        {
            return ColorTranslator.ToHtml(color).Substring(1);
        }
        /// <summary>
        /// a 比 b 颜色值浅
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Compare(Color a, Color b)
        {
            var ag = Deep(a);
            var bg = Deep(b);
            if (ag == bg)
            {
                return 0;
            }
            return ag < bg ? -1 : 1;
        }

        public static int Compare(string a, string b)
        {
            return Compare(From(a), From(b));
        }

        /// <summary>
        /// 值越小颜色越深
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static double Deep(Color color)
        {
            return color.R * 0.299 + color.G * 0.587 + color.B * 0.114;
        }

        public static Color From(string color)
        {
            return From(color, Color.Black);
        }

        public static Color From(string color, Color def)
        {
            color = color.Trim();
            if (color.Length == 0)
            {
                return def;
            }
            try
            {
                if (color[0] == '#')
                {
                    return ColorTranslator.FromHtml(color);
                }
                if (!color.Contains(","))
                {
                    return ColorTranslator.FromHtml("#" + color);
                }
            }
            catch (Exception)
            {
                return def;
            }
            var i = color.IndexOf('(');
            if (i >= 0)
            {
                color = color.Substring(i + 1);
            }
            i = color.IndexOf(')');
            if (i >= 0)
            {
                color = color.Substring(0, i);
            }
            if (string.IsNullOrEmpty(color))
            {
                return def;
            }
            try
            {
                return ColorTranslator.FromHtml(color);
            }
            catch (Exception)
            {
                return def;
            }
        }

        public static string Format(string color)
        {
            color = color.Trim();
            if (color.Length == 0)
            {
                return color;
            }
            if (color[0] == '#')
            {
                return color.Substring(1).ToUpper();
            }
            if (!color.Contains(","))
            {
                return color.ToUpper();
            }
            var i = color.IndexOf('(');
            if (i >= 0)
            {
                color = color.Substring(i + 1);
            }
            i = color.IndexOf(')');
            if (i >= 0)
            {
                color = color.Substring(0, i);
            }
            if (string.IsNullOrEmpty(color))
            {
                return color;
            }
            try
            {
                return To(ColorTranslator.FromHtml(color));
            }
            catch (Exception)
            {
                return color.ToUpper();
            }
        }
    }
}
