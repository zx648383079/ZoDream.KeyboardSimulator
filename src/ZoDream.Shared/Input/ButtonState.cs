using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public enum ButtonState
    {
        None,
        /// <summary>
        /// 按键释放
        /// </summary>
        Released,
        /// <summary>
        /// 按键按下
        /// </summary>
        Pressed,
    }
}
