using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace YQ.Web.Core
{
    public static class ExtensionUtils
    {
        public static string GetDescription(this Enum Enumeration)
        {
            string Value = Enumeration.ToString();
            Type EnumType = Enumeration.GetType();
            var DescAttribute = (DescriptionAttribute[])EnumType
                .GetField(Value)
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return DescAttribute.Length > 0 ? DescAttribute[0].Description : Value;
        }

        public static Dictionary<int, string> ToDictionary(this Type enumType)
        {
            return Enum.GetValues(enumType)
                .Cast<object>()
                .ToDictionary(k => (int)k, v => ((Enum)v).GetDescription());
        }

        public static List<KeyValuePair<int, string>> ToList(this Type enumType)
        {
            return Enum.GetValues(enumType)
                .Cast<object>()
                .Select(p => new KeyValuePair<int, string>((int)p, ((Enum)p).GetDescription()))
                .ToList();
        }
    }
}