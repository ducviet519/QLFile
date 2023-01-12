using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebTools.Extensions
{
    public class StaticHelper
    {
        #region Định dạng lại chuỗi dữ liệu
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }
        #endregion
    }
}
