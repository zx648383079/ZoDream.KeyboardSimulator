using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.OS.WinApi.Models
{
    /// <summary>
    ///     MapVirtualKeys uMapType
    /// </summary>
    public enum MapKeyType : uint
    {
        /// <summary>
        ///     uCode is a virtual-key code and is translated into an unshifted character value in the low-order word of the return
        ///     value. Dead keys (diacritics) are indicated by setting the top bit of the return value. If there is no translation,
        ///     the function returns 0.
        /// </summary>
        VK_TO_VSC = 0,

        /// <summary>
        ///     uCode is a virtual-key code and is translated into a scan code. If it is a virtual-key code that does not
        ///     distinguish between left- and right-hand keys, the left-hand scan code is returned. If there is no translation, the
        ///     function returns 0.
        /// </summary>
        VSC_TO_VK = 1,

        /// <summary>
        ///     uCode is a scan code and is translated into a virtual-key code that does not distinguish between left- and
        ///     right-hand keys. If there is no translation, the function returns 0.
        /// </summary>
        VK_TO_CHAR = 2,

        /// <summary>
        ///     uCode is a scan code and is translated into a virtual-key code that distinguishes between left- and right-hand
        ///     keys. If there is no translation, the function returns 0.
        /// </summary>
        VSC_TO_VK_EX = 3
    }
}
