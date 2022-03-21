using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Kaits.WEB.Util
{
    public static class Utils
    {
        public static DateTime StringToDatetime(string date)
        {
            DateTime fecha;
            string format = "d";
            CultureInfo provider = CultureInfo.InvariantCulture;
            fecha = DateTime.ParseExact(date, format, provider);
            return fecha;
        }
        public static string DatetimeTOString(DateTime date)
        {
            String fecha;
            fecha = date.ToString("MM/dd/yyyy");
            return fecha;
        }
    }
}
