using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Parser
{
    public class ParserOption
    {
        /// <summary>
        /// 最大合并间隔
        /// </summary>
        public int MaxSpace { get; set; } = 150;

        /// <summary>
        /// 记录鼠标移动
        /// </summary>
        public bool HasMove { get; set; } = false;
        /// <summary>
        /// 基本坐标
        /// </summary>
        public int BaseX { get; set; } = 0;
        public int BaseY { get; set; } = 0;
    }
}
