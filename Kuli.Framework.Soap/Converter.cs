using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuli.Framework.Soap
{
    class Converter
    {
        public static dynamic ChangeType(dynamic source, Type dest)
        {
            return Convert.ChangeType(source, dest);
        }
        public static dynamic ConvertList(List<object> value, Type type)
        {
            //ConvertList(objects, typeof(List<int>)).Dump();
            var containedType = type.GenericTypeArguments.First();
            return value.Select(item => Convert.ChangeType(item, containedType));
        }
        public static Object GetInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
        public static Nullable<DateTime> toDateTime(object input)
        {
            DateTime? output;
            try
            {
                if ((input != null) && (!input.Equals(DBNull.Value)))
                {
                    string str_date = String.Format("{0:yyyy-MM-dd}", input);
                    if (str_date.Length > 8)
                    {
                        str_date = str_date.Substring(0, 8);
                    }
                    if (str_date != "0/0/0000")
                    {
                        output = Convert.ToDateTime(input);
                    }
                    else
                    {
                        output = null;
                    }

                }
                else
                {
                    output = null;
                }

            }
            catch (Exception e)
            {
                output = null;
                Console.WriteLine(e.Message);
            }
            return output;
        }
        public static String formatDate(object input, string format = "dd-MM-yyyy")
        {
            string output;
            DateTime? toDateTime;
            try
            {
                if ((input != null) && (!input.Equals(DBNull.Value)))
                {
                    toDateTime = Converter.toDateTime(input);
                    if (toDateTime != null)
                    {
                        output = toDateTime.Value.ToString(format);
                    }
                    else
                    {
                        output = "";
                    }
                }
                else
                {
                    output = "";
                }
            }
            catch (Exception e)
            {
                output = "";
                Console.WriteLine(e.Message);
            }
            return output;
        }
        public static DateTime? CheckDate(DateTime? input)
        {
            DateTime? output = null;
            if (Converter.formatDate(input, "dd/MM/yyyy") != "11/11/2011")
            {
                output = input;
            }
            return output;
        }

    }
}
