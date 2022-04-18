using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Utils
{
    public static class Time
    {
        public static string FormatTime(DateTime date)
        {
            return $"{Str.TwoPad(date.Hour)}:{Str.TwoPad(date.Minute)}:{Str.TwoPad(date.Second)}";
        }

        public static string FormatTime()
        {
            return FormatTime(DateTime.Now);
        }

    }
}
