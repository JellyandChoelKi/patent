using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2GGTT.Utils
{
    public static class NullException
    {
        public static string NullExceptions(this object obj)
        {
            try
            {
                return Convert.ToString(obj ?? "");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string NullExceptions(this object obj, object value)
        {
            string returnValue = obj.NullExceptions().Trim();
            if (returnValue == "" && value != null)
            {
                return value.NullExceptions();
            }
            else
            {
                return returnValue;
            }
        }
    }
}
