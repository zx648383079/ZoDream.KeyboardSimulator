using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZoDream.Shared.OS.WinApi.Models
{
    /// <summary>
    /// WM_COPYDATA消息所要求的数据结构
    /// </summary>
    public struct CopyDataStruct
    {
        /// <summary>
        /// 自定义类型数据，但只能是4字节整数      
        /// </summary>
        public IntPtr dwData;
        /// <summary>
        /// 数据长度，这里的长度是按字节来算的
        /// </summary>
        public int cbData;
        /// <summary>
        /// 数据字符串
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
}
