using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ZoDream.Shared.Utils
{
    public static class Str
    {
        public static string MD5Encode(string source)
        {
            var sor = Encoding.UTF8.GetBytes(source);
            var md5 = MD5.Create();
            var result = md5.ComputeHash(sor);
            md5.Dispose();
            var strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }

        public static int ToInt(string souce)
        {
            if (string.IsNullOrWhiteSpace(souce))
            {
                return 0;
            }
            souce = souce.Trim();
            try
            {
                if (souce.StartsWith("0x"))
                {
                    return Convert.ToInt32(souce.Substring(2), 16);
                }
                return Convert.ToInt32(souce);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool IsInt(string source)
        {
            return Regex.IsMatch(source, @"^(0x[\dA-Za-z]+)|(\d+)$");
        }

        public static string TwoPad(object v)
        {
            return v.ToString().PadLeft(2, '0');
        }
    }
}
