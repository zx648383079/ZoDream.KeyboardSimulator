using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Utils
{
    public static class Tween
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">开始值</param>
        /// <param name="end">结束值</param>
        /// <param name="step">每次执行的步骤</param>
        /// <param name="func">当前值,当前步骤前进值</param>
        public static void Invoke(int start, int end, int step, Action<int, int> func)
        {
            Invoke(start, end, step, func, i => i);
        }

        public static void Invoke(int start, int end, int step, Action<int, int> func, Func<int, int> easeFunc)
        {
            step = step == 0 ? 1 : Math.Abs(step);
            var abs = start > end ? -1 : 1;
            if (start > end)
            {
                step = -step;
            }
            var last = start;
            while (true)
            {
                if ((abs > 0 && start > end) || (abs < 0 && start < end))
                {
                    start = end;
                }
                func(start, start - last);
                if (start == end)
                {
                    break;
                }
                last = start;
                start += step * abs;
                step = Math.Abs(easeFunc(start));
            }
        }

        public static void Invoke(int start, int end, Action<int, int> func, Func<int, int> easeFunc)
        {
            Invoke(start, end, 1, func, easeFunc);
        }
    }
}
